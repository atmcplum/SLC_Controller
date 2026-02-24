using Abeo.HW;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace SLC_Controller {
    public partial class Form1 : Form {

        // 1. 필드 및 속성
        private AbeoLightCon lc = new AbeoLightCon();
        private System.Timers.Timer testTimer;
        private readonly Dictionary<int, ChannelSettings> channelSettings = new Dictionary<int, ChannelSettings>();
        private readonly Dictionary<int, int> highlightVersions = new Dictionary<int, int>();
        private readonly Dictionary<int, Color> highlightBaseColors = new Dictionary<int, Color>();
        private readonly Dictionary<int, bool> continuousOn = new Dictionary<int, bool>();
        private bool isTesting = false;
        private bool isConnecting = false;
        private bool isConnected = false;
        private bool pingEnabled = false;
        private bool runStatus = false;
        private int crrChannel;
        private long lastPingTick = 0;
        private int pingInFlight = 0;
        private int pingFailCount = 0;

        const int MAX_Ima = 1000;
        const int MAX_DUS_US = 300000;
        const int PING_INTERVAL_MS = 2000;
        const int PING_TIMEOUT_MS = 300;
        private ComboBox[] modes => new[] { cbMode1, cbMode2, cbMode3, cbMode4 };

        //private class ChannelSettings {
        //    public int Ima { get; set; } // 전류 설정값
        //    public int Wus { get; set; } // 온 시간(us)
        //    public int Dus { get; set; } // 오프 시간(us)
        //}


        // 2. 생성자
        public Form1() {
            InitializeComponent();
            InitUI();
            cbIP.SelectedIndex = 1;
            cbIP.Visible = false;

            testTimer = new System.Timers.Timer();
            testTimer.Elapsed += TestTimer_Elapsed;
            testTimer.AutoReset = true;

            clockTimer.Interval = 500;
            clockTimer.Start();

            lc.Name = "LC"; //lc.IP = "172.28.37.101"; // 장치 이름 설정            
        }

        // 3. UI 초기화 및 설정
        private void InitUI() {
            Log("Initializing UI...");
            SetModesTo(0);
            lc.SetTriggerMode(0);
            for (int i = 1; i <= 4; i++) {
                foreach (var name in new[] { "tbIma", "tbDus", "tbWus" }) {
                    (Controls.Find(name + i, true).FirstOrDefault() as TextBox)?.Hide();
                }
                (Controls.Find("btnTrigger" + i, true).FirstOrDefault() as Button)?.Hide();
            }
            Log("[InitUI] CycleTime/Delay Default Setting...");
            tbCycleTime.Text = "5000"; //총 사이클 시간 (초기값) // 기본 사이클 5초
            tbDelay.Text = "1250"; //채널별 할당되는 시간 (초기값) // 기본 딜레이 1.25초
            Log("[UI] Set defaults: CycleTime=5000, Delay=1250");
            UpdateConnStatusLabel();

            Log("[InitUI] UI initialization completed.");
        }

        private void SetControlsEnabled(bool enabled) { // 컨트롤 활성화/비활성화
            tbCycleTime.Enabled = enabled;
            tbDelay.Enabled = enabled;
            btnConnect.Enabled = enabled;
        }

        private void SetTbState(TextBox tb, bool visible) { // 텍스트박스 상태 설정 visible: 표시 여부
            tb.Visible = visible;
            tb.Enabled = visible;
        }

        private void SetModesTo(int n) { // 모든 채널 모드 설정
            foreach (var cb in modes) {
                cb.SelectedIndex = n;
            }
        }

        private void ModeSetting(string mode, TextBox tbIma, TextBox tbWus, TextBox tbDus, Button btnTrigger) { // 모드에 따른 UI 설정
            bool imaVis = false, wusVis = false, dusVis = false, triggerVis = false;

            if (mode == "Pulse") {
                imaVis = wusVis = dusVis = triggerVis = true;
                btnTrigger.Text = "TRIG";

                if (string.IsNullOrWhiteSpace(tbIma.Text)) tbIma.Text = "500"; // 기본 전류
                if (string.IsNullOrWhiteSpace(tbWus.Text)) tbWus.Text = "400000"; // 기본 온 시간
                if (string.IsNullOrWhiteSpace(tbDus.Text)) tbDus.Text = "0"; // 기본 오프 시간
            }
            else if (mode == "Continuous") {
                imaVis = triggerVis = true;
                btnTrigger.Text = "ON";

                if (string.IsNullOrWhiteSpace(tbIma.Text)) tbIma.Text = "500"; // 기본 전류
            }

            SetTbState(tbIma, imaVis);
            SetTbState(tbWus, wusVis);
            SetTbState(tbDus, dusVis);
            btnTrigger.Visible = triggerVis;
        }

        private void UpdateMaxDelayAndValidate() { // 사이클 시간에 따른 최대 딜레이 계산 및 검증
            try {
                if (!int.TryParse(tbCycleTime.Text, out int cycleTime))
                    return; // 숫자 아니면 종료

                int maxDelay = cycleTime / 4;
                lbMaxDelay.Text = "MAX: " + maxDelay.ToString();

                if (int.TryParse(tbDelay.Text, out int delay)) { // 딜레이 입력 파싱
                    if (delay > maxDelay) { // 최대치 초과 시
                        tbDelay.Text = maxDelay.ToString(); // 최대값으로 보정
                    }
                }
            }
            catch (Exception ex) {
                Log("Error occurred while validating delay value: " + ex.Message);
            }
        }

        // 4. 연결, 네트워크
        private void btnConnect_Click(object sender, EventArgs e) {
            Log("[NET] Connect start");
            btnConnect.Enabled = false;
            btnConnect.BackColor = Color.FromArgb(0, 33, 44);
            btnConnect.Text = "CONNECTING...";
            isConnecting = true;
            UpdateConnStatusLabel();
            Task.Run(() => {
                try {
                    lc.IP = "172.28.37.101"; //lc.IP = cbIP.SelectedItem?.ToString(); // 장치 IP 설정
                    if (string.IsNullOrEmpty(lc.IP)) { // IP 비어있는지 검사
                        Log("[NET] IP Address not found");
                        return;
                    }
                    Log($"[NET] Target IP: {lc.IP}");

                    var ok = lc.Connect();

                    if (!ok) {
                        Invoke((MethodInvoker)(() => {
                            isConnected = false;
                            pingEnabled = false;
                            pingFailCount = 0;
                            isConnecting = false;
                            lbIsConn.ForeColor = Color.Red;
                            btnConnect.Text = "CONNECT";
                            btnConnect.BackColor = Color.FromArgb(0, 33, 44);
                            UpdateConnStatusLabel();
                            runTimer.Stop();
                        }));
                        Log("[NET] Connection failed");
                        ShowIP(); 
                        return; 
                    }
                    else {
                        Invoke((MethodInvoker)(() => {
                            isConnected = true;
                            pingEnabled = true;
                            pingFailCount = 0;
                            isConnecting = false;
                            lbIsConn.ForeColor = Color.Green;
                            UpdateConnStatusLabel();
                            runTimer.Interval = 500;
                            runTimer.Stop();
                            runTimer.Start();
                        }));
                        Log("[NET] Connected");
                        ShowIP();
                    }
                }
                catch (Exception ex) {
                    Log($"[ERR] Connect exception: {ex.Message}");
                }
                finally {
                    Invoke((MethodInvoker)(() => {
                        isConnecting = false;
                        UpdateConnStatusLabel();
                        btnConnect.Enabled = true;
                    }));
                }
            });
        }

        private void ShowIP() { // PC와 컨트롤러 IP 표시
            string localIP = "IP Address NOT FOUND"; 

            try {
                string hostName = Dns.GetHostName(); // 호스트 이름 조회
                IPHostEntry hostEntry = Dns.GetHostEntry(hostName); // DNS 정보 가져오기

                foreach (IPAddress ip in hostEntry.AddressList) { // IP 목록 순회
                    if (ip.AddressFamily == AddressFamily.InterNetwork) { // IPv4만 선택
                        localIP = ip.ToString(); // 발견한 IP 저장
                        break; // 첫 IPv4만 사용
                    }
                }
            }
            catch (Exception ex) {
                localIP = "ERROR: " + ex.Message; // 실패 시 메시지
            }

            Log($"[NET] Host IP: {localIP}");
            Log($"[NET] Controller IP: {lc.IP}");
        }

        // 5. 테스트 모드
        private void btnTest_Click(object sender, EventArgs e) { // TEST 버튼 클릭
            if (!isTesting) { // 테스트 시작 분기
                Log("[TEST] Start requested");
                if (!cbSimulationMode.Checked && !lc.Connected) { // 실장치 모드인데 미연결이면
                    Log("[TEST] Not connected");
                    return; // 종료
                }

                if (int.TryParse(tbCycleTime.Text, out int cTime)) { // 사이클 시간 파싱
                    testTimer.Interval = cTime; // 타이머 주기 설정
                    Log($"[TEST] Timer interval set to {cTime} ms");
                }
                else {
                    Log("[TEST] Invalid cycle time");
                    return; // 종료
                }

                isTesting = true; // 테스트 중 플래그
                SetControlsEnabled(false); // 주요 입력 비활성화
                btnTest.Text = "TESTING"; // 버튼 텍스트 변경
                crrChannel = 1; // 첫 채널부터 시작

                Log("[TEST] Started");
                TrigChannels(); // 즉시 한번 실행
                testTimer.Start(); // 타이머 시작
            }
            else { // 테스트 종료 분기
                Log("[TEST] Stop requested");
                StopTest("manual");
            }
        }

        private void TestTimer_Elapsed(object sender, ElapsedEventArgs e) { // 타이머 틱마다
            TrigChannels(); // 채널 순차 실행
        }

        private void StopTest(string reason) { // 테스트 종료 공통 처리
            if (!isTesting) return;
            Log($"[TEST] Stopped ({reason})");
            isTesting = false; // 플래그 해제
            SetControlsEnabled(true); // 입력 활성화
            btnTest.Text = "TEST"; // 버튼 텍스트 복원
            testTimer.Stop(); // 타이머 정지
            SetModesTo(0); // 모드 선택 초기화
            channelSettings.Clear(); // 채널 설정 초기화
        }

        private void TrigChannels() { // 4개 채널을 순회하며 트리거
            Invoke((MethodInvoker)(async () => { // UI 스레드에서 실행
                try {
                    if (!cbSimulationMode.Checked && !isConnected) { // 연결 해제 시 테스트 중단
                        StopTest("disconnected");
                        return;
                    }
                    if (!int.TryParse(tbDelay.Text, out int delay)) // 딜레이 파싱
                        delay = 1000; // 실패 시 기본값

                    for (int ch = 1; ch <= 4; ch++) { // 채널 1~4 반복
                        string mode = GetChannelMode(ch); // 현재 모드 가져오기

                        if (!channelSettings.TryGetValue(ch, out var setting)) // 설정 없으면
                            continue; // 건너뜀

                        if (!cbSimulationMode.Checked && !lc.Connected) // 실장치 모드인데 미연결이면
                            continue; // 실행하지 않음

                        Log($"[CH] Ch{ch} -> {mode}");

                        if (mode == "Pulse") { // 펄스 모드
                            if (!cbSimulationMode.Checked) { // 실장치면
                                lc.SetPulseOutput(ch, setting.Ima, setting.Wus, setting.Dus); // 파라미터 설정
                                lc.Trigger(ch); // 트리거 명령
                            }

                            int dusMs = Math.Max(0, setting.Dus / 1000); // 딜레이 시간 계산
                            if (dusMs > 0) {
                                await Task.Delay(dusMs); // 딜레이 대기
                            }
                            int durationMs = Math.Max(1, setting.Wus / 1000); // LED 표시 시간 계산
                            _ = HighlightChannel(ch, Color.Lime, durationMs); // 채널 라벨 하이라이트
                            await Task.Delay(delay); // 다음 채널 전 대기
                        }
                        else if (mode == "Continuous") { // 연속 모드
                            if (!cbSimulationMode.Checked)
                                lc.SetContinousOutput(ch, setting.Ima); // 지속 출력 설정

                            _ = HighlightChannel(ch, Color.Lime, delay); // 라벨 하이라이트
                            await Task.Delay(delay); // 지정 딜레이 만큼 대기

                            if (!cbSimulationMode.Checked)
                                SetOffMode(ch);
                        }
                    }
                }
                catch (Exception ex) {
                    Log($"[ERR] Trigger failed during test: {ex.Message}");
                }
            }));
        }

        // 6. 채널 트리거
        private async void btnTrigger_Click(object sender, EventArgs e) { // 개별 채널 트리거 버튼
            //TestCapture(0); // 캡처 테스트 (비활성)
            if (!cbSimulationMode.Checked && !lc.Connected) { // 실장치 모드에서 미연결이면
                Log("[CH] Not connected");
                return; // 중단
            }

            if (sender is Button btn) { // 눌린 버튼 확인
                int index = int.Parse(btn.Name.Substring("btnTrigger".Length)); // 버튼 이름에서 채널 번호 추출

                if (!channelSettings.ContainsKey(index)) { // 설정 존재 여부
                    Log($"[CH] Ch{index} has no setting value. Please press 'Set' button.");
                    return; // 중단
                }

                var setting = channelSettings[index]; // 채널 설정 가져오기
                string mode = GetChannelMode(index); // 채널 모드 확인

                Log($"[CH] Manual trigger requested: Ch{index}, Mode={mode}");

                if (mode == "Pulse") { // 펄스 모드
                    Log($"[CH] Ch{index} Pulse: Ima={setting.Ima}, Wus={setting.Wus}, Dus={setting.Dus}");

                    if (!cbSimulationMode.Checked) {
                        lc.SetPulseOutput(index, setting.Ima, setting.Wus, setting.Dus); // 장치에 값 설정
                        lc.Trigger(index); // 트리거 실행
                    }

                    int dusMs = Math.Max(0, setting.Dus / 1000); // 딜레이 시간 계산
                    if (dusMs > 0) {
                        await Task.Delay(dusMs); // 딜레이 대기
                    }
                    int duration = Math.Max(1, setting.Wus / 1000); // 표시 시간 계산
                    _ = HighlightChannel(index, Color.Orange, duration); // 라벨 강조
                }
                else if (mode == "Continuous") { // 연속 모드
                    Log($"[CH] Ch{index} Continuous: Ima={setting.Ima}");

                    bool isOn = continuousOn.TryGetValue(index, out bool currentOn) && currentOn;
                    bool nextOn = !isOn;
                    continuousOn[index] = nextOn;

                if (!cbSimulationMode.Checked) {
                    if (nextOn) {
                        lc.SetContinousOutput(index, setting.Ima); // 연속 출력 설정
                    }
                    else {
                        SetOffMode(index); // 출력 종료
                    }
                    }

                    if (nextOn) {
                        SetChannelLabelColor(index, Color.Orange); // ON 상태 표시
                    }
                    else {
                        RestoreChannelLabelColor(index); // OFF 상태 복구
                    }
                    btn.Text = nextOn ? "OFF" : "ON";
                }
                else {
                    Log($"[CH] Ch{index} mode is Null or Off.");
                }
            }
        }

        private void SetOffMode(int ch) { // 출력 종료를 위한 최소 펄스 세팅
            lc.SetPulseOutput(ch, 1, 1, 0); // 미미한 펄스로 꺼짐 처리
        }

        private async Task HighlightChannel(int ch, Color color, int durationMs) { // 채널 라벨 하이라이트
            if (!highlightVersions.ContainsKey(ch)) {
                highlightVersions[ch] = 0;
            }
            int token = ++highlightVersions[ch];

            Label lb = null;
            Color baseColor = default;
            // UI 스레드에서만 라벨 접근/색 변경
            if (InvokeRequired) {
                Invoke((MethodInvoker)(() => {
                    lb = Controls.Find("lbCh" + ch, true).FirstOrDefault() as Label; // 라벨 찾기
                    if (lb == null) {
                        return;
                    }
                    // 최초 1회만 기본색 저장(연속 하이라이트 시 원래색 유지)
                    if (!highlightBaseColors.ContainsKey(ch)) {
                        highlightBaseColors[ch] = lb.BackColor; // 최초 색상 저장
                    }
                    baseColor = highlightBaseColors[ch];
                    lb.BackColor = color; // 새 색상 적용
                }));
            }
            else {
                // 이미 UI 스레드면 바로 처리
                lb = Controls.Find("lbCh" + ch, true).FirstOrDefault() as Label; // 라벨 찾기
                if (lb == null) {
                    return;
                }
                // 최초 1회만 기본색 저장
                if (!highlightBaseColors.ContainsKey(ch)) {
                    highlightBaseColors[ch] = lb.BackColor; // 최초 색상 저장
                }
                baseColor = highlightBaseColors[ch];
                lb.BackColor = color; // 새 색상 적용
            }

            // 라벨을 못 찾았으면 종료
            if (lb == null) {
                return;
            }

            // 지정 시간만큼 하이라이트 유지
            await Task.Delay(durationMs);  // duration in ms // 지정 시간 대기

            // 최신 호출만 복원 (중간 호출은 무시)
            if (highlightVersions[ch] == token) {
                if (InvokeRequired) {
                    BeginInvoke((MethodInvoker)(() => lb.BackColor = baseColor)); // 기본색 복구
                }
                else {
                    lb.BackColor = baseColor; // 기본색 복구
                }
            }
        }

        private void SetChannelLabelColor(int ch, Color color) { // 채널 라벨 색상 설정
            void Apply() {
                Label lb = Controls.Find("lbCh" + ch, true).FirstOrDefault() as Label; // 라벨 찾기
                if (lb == null) return;
                if (!highlightBaseColors.ContainsKey(ch)) {
                    highlightBaseColors[ch] = lb.BackColor; // 최초 색상 저장
                }
                lb.BackColor = color;
            }

            if (InvokeRequired) {
                Invoke((MethodInvoker)Apply);
            }
            else {
                Apply();
            }
        }

        private void RestoreChannelLabelColor(int ch) { // 채널 라벨 색상 복원
            void Apply() {
                if (!highlightBaseColors.TryGetValue(ch, out var baseColor)) return;
                Label lb = Controls.Find("lbCh" + ch, true).FirstOrDefault() as Label; // 라벨 찾기
                if (lb == null) return;
                lb.BackColor = baseColor;
            }

            if (InvokeRequired) {
                Invoke((MethodInvoker)Apply);
            }
            else {
                Apply();
            }
        }

        // 7. 설정 버튼
        private void btnSet_Click(object sender, EventArgs e) { // Set 버튼 클릭
            try {
                if (sender is Button btn) { // 버튼 확인
                    int ch = int.Parse(btn.Name.Substring("btnSet".Length)); // 채널 번호 추출
                    string mode = GetChannelMode(ch);

                    Log($"[CH] Set requested: Ch{ch}, Mode={mode}");

                    TextBox tbIma = Controls.Find($"tbIma{ch}", true).FirstOrDefault() as TextBox; // 전류 입력
                    TextBox tbWus = Controls.Find($"tbWus{ch}", true).FirstOrDefault() as TextBox; // 온 시간 입력
                    TextBox tbDus = Controls.Find($"tbDus{ch}", true).FirstOrDefault() as TextBox; // 오프 시간 입력

                    if (!int.TryParse(tbIma?.Text, out int ima)) { // 전류 값 검증
                        Log($"[CH] Set failed: Ch{ch} Ima is invalid");
                        return;
                    }

                    if (ima > MAX_Ima) { // 최대 전류 초과 시
                        Log($"[CH] Set failed: Ch{ch} Ima exceeds MAX ({MAX_Ima})");
                        tbIma.Text = MAX_Ima.ToString(); // 최대값으로 보정
                        return;
                    }

                    if (mode == "Continuous") { // 연속 모드 저장
                        channelSettings[ch] = new ChannelSettings { Ima = ima, Wus = 0, Dus = 0 }; // 설정 딕셔너리에 저장
                        Log($"[CH] Set saved: Ch{ch} Continuous (Ima={ima})");
                    }
                    else if (mode == "Pulse") { // 펄스 모드 저장
                        if (!int.TryParse(tbWus?.Text, out int wus) || !int.TryParse(tbDus?.Text, out int dus)) { // 온/오프 검증
                            Log($"[CH] Set failed: Ch{ch} Wus/Dus is invalid");
                            return;
                        }
                        if (dus > MAX_DUS_US) {
                            Log($"[CH] Set adjust: Ch{ch} Dus exceeds MAX ({MAX_DUS_US})");
                            dus = MAX_DUS_US;
                            tbDus.Text = dus.ToString();
                        }

                        channelSettings[ch] = new ChannelSettings { Ima = ima, Wus = wus, Dus = dus }; // 설정 저장
                        Log($"[CH] Set saved: Ch{ch} Pulse (Ima={ima}, Wus={wus}, Dus={dus})");
                    }
                    else { // 모드가 Off 또는 미설정
                        Log($"[CH] Set skipped: Ch{ch} mode cannot be set (Mode={mode})");
                    }
                }
            }
            catch (Exception ex) {
                Log($"[ERR] Set exception: {ex.Message}");
            }
        }
        private void btnSetAll_Click(object sender, EventArgs e) { // Set All 버튼 클릭
            try {
                if (sender is Button btn) { // 버튼 확인
                    Log("[CH] SetAll requested");
                    for (int ch = 1; ch < 5; ch++) {
                        string mode = GetChannelMode(ch);

                        TextBox tbIma = Controls.Find($"tbIma{ch}", true).FirstOrDefault() as TextBox; // 전류 입력
                        TextBox tbWus = Controls.Find($"tbWus{ch}", true).FirstOrDefault() as TextBox; // 온 시간 입력
                        TextBox tbDus = Controls.Find($"tbDus{ch}", true).FirstOrDefault() as TextBox; // 오프 시간 입력

                        if (!int.TryParse(tbIma?.Text, out int ima)) { // 전류 값 검증
                            Log($"[CH] SetAll skip: Ch{ch} Ima is invalid");
                            continue;
                        }

                        if (ima > MAX_Ima) { // 최대 전류 초과 시
                            Log($"[CH] SetAll failed: Ch{ch} Ima exceeds MAX ({MAX_Ima})");
                            tbIma.Text = MAX_Ima.ToString(); // 최대값으로 보정
                            return;
                        }

                        if (mode == "Continuous") { // 연속 모드 저장
                            channelSettings[ch] = new ChannelSettings { Ima = ima, Wus = 0, Dus = 0 }; // 설정 딕셔너리에 저장
                            Log($"[CH] SetAll saved: Ch{ch} Continuous (Ima={ima})");
                        }
                        else if (mode == "Pulse") { // 펄스 모드 저장
                            if (!int.TryParse(tbWus?.Text, out int wus) || !int.TryParse(tbDus?.Text, out int dus)) { // 온/오프 검증
                                Log($"[CH] SetAll failed: Ch{ch} Wus/Dus is invalid");
                                return;
                            }
                            if (dus > MAX_DUS_US) {
                                Log($"[CH] SetAll adjust: Ch{ch} Dus exceeds MAX ({MAX_DUS_US})");
                                dus = MAX_DUS_US;
                                tbDus.Text = dus.ToString();
                            }

                            channelSettings[ch] = new ChannelSettings { Ima = ima, Wus = wus, Dus = dus }; // 설정 저장
                            Log($"[CH] SetAll saved: Ch{ch} Pulse (Ima={ima}, Wus={wus}, Dus={dus})");
                        }
                        else { // 모드가 Off 또는 미설정
                            Log($"[CH] SetAll skip: Ch{ch} mode cannot be set (Mode={mode})");
                        }
                    }
                }
            }
            catch (Exception ex) {
                Log($"[ERR] SetAll exception: {ex.Message}");
            }
            Log("[CH] SetAll done");
        }

        // 8. UI 이벤트 핸들러
        private void cbSimulationMode_CheckedChanged(object sender, EventArgs e) { // 시뮬레이션 모드
            if (!cbSimulationMode.Checked) {
                btnConnect.Enabled = true;
                this.BackColor = Color.FromArgb(0, 33, 44);
                UpdateConnStatusLabel();
                Log("[SIM] Simulation mode: OFF");
            }
            else {
                btnConnect.Enabled = false;
                lbIsConn.Text = "SIMULATION MODE";
                this.BackColor = Color.FromArgb(133, 44, 0);
                UpdateConnStatusLabel();
                Log("[SIM] Simulation mode: ON");
            }
        }

        private void cbMode_SelectedIndexChanged(object sender, EventArgs e) {
            if (sender is ComboBox cb && cb.Name.StartsWith("cbMode")) {
                if (int.TryParse(cb.Name.Substring("cbMode".Length), out int ch)) { // 채널 번호 추출
                    Log($"[CH] Ch{ch} mode changed -> {cb.Text}");
                    var tbIma = Controls.Find($"tbIma{ch}", true).FirstOrDefault() as TextBox; // 전류 입력 컨트롤
                    var tbWus = Controls.Find($"tbWus{ch}", true).FirstOrDefault() as TextBox; // 온 시간 컨트롤
                    var tbDus = Controls.Find($"tbDus{ch}", true).FirstOrDefault() as TextBox; // 오프 시간 컨트롤
                    var btnTrig = Controls.Find($"btnTrigger{ch}", true).FirstOrDefault() as Button; // 트리거 버튼

                    if (tbIma != null && tbWus != null && tbDus != null && btnTrig != null) { // 컨트롤 유효성 확인
                        ModeSetting(cb.Text, tbIma, tbWus, tbDus, btnTrig);
                    }
                    else {
                        Log($"[CH] Skip mode setting (controls missing): Ch{ch}");
                    }
                }
            }
        }

        private void tbCycleTime_TextChanged(object sender, EventArgs e) {
            UpdateMaxDelayAndValidate(); // 최대 딜레이 재계산
        }

        private void tbDelay_TextChanged(object sender, EventArgs e) {
            UpdateMaxDelayAndValidate(); // 값 검증 및 보정
        }

        private void tbIma_TextChanged(object sender, EventArgs e) { // 전류 입력 변경 시
            if (sender is TextBox tb) {
                if (int.TryParse(tb.Text, out int value)) {
                    if (value > MAX_Ima) { // 최대치 초과 시
                        tb.Text = MAX_Ima.ToString();
                        tb.SelectionStart = tb.Text.Length;
                    }
                }
            }
        }

        // 9. 헬퍼 메서드
        private void Log(string message) {
            if (rtbLog.InvokeRequired) {
                rtbLog.BeginInvoke(new Action<string>(Log), message);
                return;
            }

            string ts = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            rtbLog.AppendText($"[{ts}] {message}{Environment.NewLine}");
            rtbLog.SelectionStart = rtbLog.TextLength;
            rtbLog.ScrollToCaret();
        }

        private string GetChannelMode(int ch) {
            if (ch < 1 || ch > 4) return "Off";
            return modes[ch - 1]?.Text ?? "Off";
        }

        private void UpdateConnStatusLabel() {
            if (cbSimulationMode.Checked) {
                lbIsConn.Text = "SIMULATION MODE";
                return;
            }

            if (isConnecting) {
                lbIsConn.Text = "CONNECTING...";
                return;
            }

            lbIsConn.Text = isConnected ? "" : "DISCONNECTED";
        }

        private void clockTimer_Tick(object sender, EventArgs e) {
            lblClock.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            TryPingLog();
        }

        private void TryPingLog() {
            if (cbSimulationMode.Checked) return;
            if (string.IsNullOrWhiteSpace(lc.IP)) return;
            if (!pingEnabled) return;
            if (isConnecting) return;

            long nowTick = Environment.TickCount;
            if (nowTick - lastPingTick < PING_INTERVAL_MS) return;
            if (Interlocked.Exchange(ref pingInFlight, 1) != 0) return;

            lastPingTick = nowTick;
            _ = Task.Run(() => {
                bool ok = false;
                long rtt = -1;
                try {
                    using (var ping = new Ping()) {
                        var reply = ping.Send(lc.IP, PING_TIMEOUT_MS);
                        if (reply != null && reply.Status == IPStatus.Success) {
                            ok = true;
                            rtt = reply.RoundtripTime;
                        }
                    }
                }
                catch {
                    ok = false;
                }

                //Log($"[PING] {lc.IP} {(ok ? "OK" : "FAIL")}, rtt={rtt}ms");
                if (ok) {
                    Interlocked.Exchange(ref pingFailCount, 0);
                }
                else {
                    int fails = Interlocked.Increment(ref pingFailCount);
                    if (fails >= 3) {
                        UI(() => {
                            if (!isConnected) return;
                            isConnected = false;
                            pingEnabled = false;
                            lbIsConn.ForeColor = Color.Red;
                            UpdateConnStatusLabel();
                            runTimer.Stop();
                            btnConnect.Text = "CONNECT";
                            btnConnect.BackColor = Color.FromArgb(0, 33, 44);
                            StopTest("disconnected");
                        });
                    }
                }
            }).ContinueWith(_ => Interlocked.Exchange(ref pingInFlight, 0));
        }

        private void runTimer_Tick(object sender, EventArgs e) {
            if (runStatus) {
                btnConnect.BackColor = Color.FromArgb(0, 33, 44);
                runStatus = false;
            }
            else {
                btnConnect.BackColor = Color.Lime;
                btnConnect.Text = "CONNECTED";
                runStatus = true;
            }
        }
        private void UI(Action action) {
            if (IsDisposed || Disposing) return;
            if (InvokeRequired) Invoke(action);
            else action();
        }
    }
}
