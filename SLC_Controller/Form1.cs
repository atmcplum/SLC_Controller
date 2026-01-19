using Abeo.HW;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace SLC_Controller {
    public partial class Form1 : Form {

        // 1. 필드 및 속성
        private AbeoLightCon lc = new AbeoLightCon();
        private System.Timers.Timer testTimer;
        private Dictionary<int, ChannelSettings> channelSettings = new Dictionary<int, ChannelSettings>();
        public bool isTesting = false;
        private bool isSimulationMode = false;
        private bool isConnected = false;
        private int crrChannel;
        const int MAX_Ima = 1000;
        string crrLbIsCon;

        private class ChannelSettings {
            public int Ima { get; set; } // 전류 설정값
            public int Wus { get; set; } // 온 시간(us)
            public int Dus { get; set; } // 오프 시간(us)
        }

        private ComboBox[] modes => new[] { cbMode1, cbMode2, cbMode3, cbMode4 };

        // 2. 생성자
        public Form1() {
            InitializeComponent();
            InitUI();
            cbIP.SelectedIndex = 1;
            cbIP.Visible = false;

            cbSimulationMode.CheckedChanged += (s, e) => {
                isSimulationMode = cbSimulationMode.Checked;
            };

            testTimer = new System.Timers.Timer();
            testTimer.Elapsed += TestTimer_Elapsed;
            testTimer.AutoReset = true;

            lc.Name = "LC"; //lc.IP = "172.28.37.101"; // 장치 이름 설정
            lc.SetTriggerMode(0);

            SetModesTo(0); // 모든 채널 모드를 Off로 초기화
        }

        // 3. UI 초기화 및 설정
        private void InitUI() {
            lbLog.Items.Add("Initializing UI...");
            for (int i = 1; i <= 4; i++) { 
                foreach (var name in new[] { "tbIma", "tbDus", "tbWus" }) {
                    (Controls.Find(name + i, true).FirstOrDefault() as TextBox)?.Hide();
                }
                (Controls.Find("btnTrigger" + i, true).FirstOrDefault() as Button)?.Hide();
            }

            tbCycleTime.Text = "5000"; //총 사이클 시간 (초기값) // 기본 사이클 5초
            tbDelay.Text = "1250"; //채널별 할당되는 시간 (초기값) // 기본 딜레이 1.25초
        }

        private void SetControlsEnabled(bool enabled) {
            tbCycleTime.Enabled = enabled;
            tbDelay.Enabled = enabled;
            btnConnect.Enabled = enabled;
        }

        private void SetTbState(TextBox tb, bool visible) {
            tb.Visible = visible;
            tb.Enabled = visible;
        }

        private void SetModesTo(int n) {
            foreach (var cb in modes) { 
                cb.SelectedIndex = n; 
            }
        }

        private void ModeSetting(string mode, TextBox tbIma, TextBox tbWus, TextBox tbDus, Button btnTrigger) {
            bool imaVis = false, wusVis = false, dusVis = false, triggerVis = false;

            if (mode == "Pulse") {
                imaVis = wusVis = dusVis = triggerVis = true;

                if (string.IsNullOrWhiteSpace(tbIma.Text)) tbIma.Text = "500"; // 기본 전류
                if (string.IsNullOrWhiteSpace(tbWus.Text)) tbWus.Text = "400000"; // 기본 온 시간
                if (string.IsNullOrWhiteSpace(tbDus.Text)) tbDus.Text = "0"; // 기본 오프 시간
            }
            else if (mode == "Continuous") {
                imaVis = triggerVis = true; 

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
                //MessageBox.Show("Error occurred while validating delay value: " + ex.Message);
                lbLog.Items.Add("Error occurred while validating delay value: " + ex.Message);
            }
        }

        // 4. 연결, 네트워크
        private void btnConnect_Click(object sender, EventArgs e) {
            lbLog.Items.Add("Connecting...");
            try {
                lc.IP = "172.28.37.101"; //lc.IP = cbIP.SelectedItem?.ToString(); // 장치 IP 설정
                if (string.IsNullOrEmpty(lc.IP)) { // IP 비어있는지 검사
                    //MessageBox.Show("IP Address Not Found", "Alert");
                    lbLog.Items.Add("IP Address Not Found");
                    return; 
                }

                lc.Connect(); // 컨트롤러 연결 시도

                if (!lc.Connected) {
                    lbIsConn.Text = "DISCONNECTED"; 
                    lbIsConn.ForeColor = Color.Red; 
                    //MessageBox.Show("Connection failed", "Error");
                    lbLog.Items.Add("Connection failed");
                    isConnected = false;
                    ShowIP(); 
                    return; 
                }
                else { 
                    lbIsConn.Text = "CONNECTED"; 
                    lbIsConn.ForeColor = Color.Green; 
                    //MessageBox.Show("Connected");
                    lbLog.Items.Add("Connected");
                    isConnected = true;
                    ShowIP(); 
                }
            }
            catch (Exception ex) {
                //MessageBox.Show("An error occurred during connection:\n" + ex.Message, "Error");
                lbLog.Items.Add($"An error occurred during connection:\n" + ex.Message);
            }
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

            //MessageBox.Show("Host IP Address: " + localIP + "\nController IP Address: " + lc.IP, "IP STATUS"); // 결과 표시
            lbLog.Items.Add("Host IP Address: " + localIP + "\tController IP Address: " + lc.IP);
        }

        // 5. 테스트 모드
        private void btnTest_Click(object sender, EventArgs e) { // TEST 버튼 클릭
            if (!isTesting) { // 테스트 시작 분기
                if (!isSimulationMode && !lc.Connected) { // 실장치 모드인데 미연결이면
                    //MessageBox.Show("Not connected"); // 안내 후
                    lbLog.Items.Add("Not connected");
                    return; // 종료
                }

                if (int.TryParse(tbCycleTime.Text, out int cTime)) { // 사이클 시간 파싱
                    testTimer.Interval = cTime; // 타이머 주기 설정
                }
                else {
                    //MessageBox.Show("Invalid cycle time");
                    lbLog.Items.Add("Invalid cycle time");// 숫자 아님
                    return; // 종료
                }

                isTesting = true; // 테스트 중 플래그
                SetControlsEnabled(false); // 주요 입력 비활성화
                btnTest.Text = "TESTING"; // 버튼 텍스트 변경
                crrChannel = 1; // 첫 채널부터 시작

                TrigChannels(); // 즉시 한번 실행
                testTimer.Start(); // 타이머 시작
            }
            else { // 테스트 종료 분기
                isTesting = false; // 플래그 해제
                SetControlsEnabled(true); // 입력 활성화
                btnTest.Text = "TEST"; // 버튼 텍스트 복원
                testTimer.Stop(); // 타이머 정지
                SetModesTo(0); // 모드 선택 초기화
                channelSettings.Clear(); // 채널 설정 초기화
            }
        }

        private void TestTimer_Elapsed(object sender, ElapsedEventArgs e) { // 타이머 틱마다
            TrigChannels(); // 채널 순차 실행
        }

        private void TrigChannels() { // 4개 채널을 순회하며 트리거
            Invoke((MethodInvoker)(async () => { // UI 스레드에서 실행
                try {
                    if (!int.TryParse(tbDelay.Text, out int delay)) // 딜레이 파싱
                        delay = 1000; // 실패 시 기본값

                    for (int ch = 1; ch <= 4; ch++) { // 채널 1~4 반복
                        string mode = GetChannelMode(ch); // 현재 모드 가져오기

                        if (!channelSettings.TryGetValue(ch, out var setting)) // 설정 없으면
                            continue; // 건너뜀

                        if (!isSimulationMode && !lc.Connected) // 실장치 모드인데 미연결이면
                            continue; // 실행하지 않음

                        if (mode == "Pulse") { // 펄스 모드
                            if (!isSimulationMode) { // 실장치면
                                lc.SetPulseOutput(ch, setting.Ima, setting.Wus, setting.Dus); // 파라미터 설정
                                lc.Trigger(ch); // 트리거 명령
                            }

                            int durationMs = Math.Max(1, setting.Wus / 1000); // LED 표시 시간 계산
                            _ = HighlightChannel(ch, Color.Lime, durationMs); // 채널 라벨 하이라이트
                            await Task.Delay(delay); // 다음 채널 전 대기
                        }
                        else if (mode == "Continuous") { // 연속 모드
                            if (!isSimulationMode)
                                lc.SetContinousOutput(ch, setting.Ima); // 지속 출력 설정

                            _ = HighlightChannel(ch, Color.Lime, delay); // 라벨 하이라이트
                            await Task.Delay(delay); // 지정 딜레이 만큼 대기

                            if (!isSimulationMode)
                                SetOffMode(ch); // 연속 출력 종료
                        }
                    }
                }
                catch (Exception ex) {
                    //MessageBox.Show("Trigger failed during test: " + ex.Message); // 테스트 중 예외 알림
                    lbLog.Items.Add("Trigger failed during test: " + ex.Message);
                }
            }));
        }

        // 6. 채널 트리거
        private void btnTrigger_Click(object sender, EventArgs e) { // 개별 채널 트리거 버튼
            //TestCapture(0); // 캡처 테스트 (비활성)
            if (!isSimulationMode && !lc.Connected) { // 실장치 모드에서 미연결이면
                //MessageBox.Show("Not connected");
                lbLog.Items.Add("Not connected");
                return; // 중단
            }

            if (sender is Button btn) { // 눌린 버튼 확인
                int index = int.Parse(btn.Name.Substring("btnTrigger".Length)); // 버튼 이름에서 채널 번호 추출

                if (!channelSettings.ContainsKey(index)) { // 설정 존재 여부
                    //MessageBox.Show($"Channel {index} has no setting value. Please press 'Set' button."); // 설정 요청
                    lbLog.Items.Add($"Channel {index} has no setting value. Please press 'Set' button.");
                    return; // 중단
                }

                var setting = channelSettings[index]; // 채널 설정 가져오기
                string mode = GetChannelMode(index); // 채널 모드 확인

                if (mode == "Pulse") { // 펄스 모드
                    if (!isSimulationMode) {
                        lc.SetPulseOutput(index, setting.Ima, setting.Wus, setting.Dus); // 장치에 값 설정
                        lc.Trigger(index); // 트리거 실행
                    }

                    int duration = Math.Max(1, setting.Wus / 1000); // 표시 시간 계산
                    _ = HighlightChannel(index, Color.Orange, duration); // 라벨 강조
                }
                else if (mode == "Continuous") { // 연속 모드
                    if (!isSimulationMode)
                        lc.SetContinousOutput(index, setting.Ima); // 연속 출력 설정

                    _ = HighlightChannel(index, Color.Orange, 1000); // 1초간 강조

                    if (!isSimulationMode)
                        SetOffMode(index); // 출력 종료
                }
                else {
                    //MessageBox.Show($"Channel {index} mode is Null or Off."); // 모드가 설정되지 않음
                    lbLog.Items.Add($"Channel {index} mode is Null or Off.");
                }
            }
        }

        private void SetOffMode(int ch) { // 출력 종료를 위한 최소 펄스 세팅
            lc.SetPulseOutput(ch, 1, 1, 0); // 미미한 펄스로 꺼짐 처리
        }

        private async Task HighlightChannel(int ch, Color color, int durationMs) { // 채널 라벨 임시 색상 변경
            Label lb = Controls.Find("lbCh" + ch, true).FirstOrDefault() as Label; // 라벨 찾기
            if (lb != null) {
                Color original = lb.BackColor; // 기존 색상 저장
                lb.BackColor = color; // 새 색상 적용
                await Task.Delay(durationMs);  // duration in ms // 지정 시간 대기
                lb.BackColor = original; // 색상 복구
            }
        }

        // 7. 설정 버튼
        private void btnSet_Click(object sender, EventArgs e) { // Set 버튼 클릭
            try {
                if (sender is Button btn) { // 버튼 확인
                    int ch = int.Parse(btn.Name.Substring("btnSet".Length)); // 채널 번호 추출
                    string mode = GetChannelMode(ch);

                    TextBox tbIma = Controls.Find($"tbIma{ch}", true).FirstOrDefault() as TextBox; // 전류 입력
                    TextBox tbWus = Controls.Find($"tbWus{ch}", true).FirstOrDefault() as TextBox; // 온 시간 입력
                    TextBox tbDus = Controls.Find($"tbDus{ch}", true).FirstOrDefault() as TextBox; // 오프 시간 입력

                    if (!int.TryParse(tbIma?.Text, out int ima)) { // 전류 값 검증
                        //MessageBox.Show($"Channel {ch}: Ima value is invalid."); // 잘못된 값 안내
                        lbLog.Items.Add($"Channel {ch}: Ima value is invalid.");
                        return;
                    }

                    if (ima > MAX_Ima) { // 최대 전류 초과 시
                        //MessageBox.Show($"Ima value cannot exceed {MAX_Ima}."); // 경고
                        lbLog.Items.Add($"Ima value cannot exceed {MAX_Ima}.");
                        tbIma.Text = MAX_Ima.ToString(); // 최대값으로 보정
                        return;
                    }

                    if (mode == "Continuous") { // 연속 모드 저장
                        channelSettings[ch] = new ChannelSettings { Ima = ima, Wus = 0, Dus = 0 }; // 설정 딕셔너리에 저장
                        //MessageBox.Show($"Channel {ch} (Continuous mode) setting saved"); // 저장 완료 안내
                        lbLog.Items.Add($"Channel {ch} (Continuous mode) setting saved");
                    }
                    else if (mode == "Pulse") { // 펄스 모드 저장
                        if (!int.TryParse(tbWus?.Text, out int wus) || !int.TryParse(tbDus?.Text, out int dus)) { // 온/오프 검증
                            //MessageBox.Show($"Channel {ch}: Wus or Dus value is invalid."); // 잘못된 값 안내
                            lbLog.Items.Add($"Channel {ch}: Wus or Dus value is invalid.");
                            return;
                        }

                        channelSettings[ch] = new ChannelSettings { Ima = ima, Wus = wus, Dus = dus }; // 설정 저장
                        //MessageBox.Show($"Channel {ch} (Pulse mode) setting saved"); // 저장 알림
                        lbLog.Items.Add($"Channel {ch} (Pulse mode) setting saved");
                    }
                    else { // 모드가 Off 또는 미설정
                        //MessageBox.Show($"Channel {ch} mode cannot be set. Current mode: {mode}"); // 설정 불가 안내
                        lbLog.Items.Add($"Channel {ch} mode cannot be set. Current mode: {mode}");
                    }
                }
            }
            catch (Exception ex) {
                //MessageBox.Show($"Error occurred during setting: {ex.Message}"); // 예외 처리
                lbLog.Items.Add($"Error occurred during setting: {ex.Message}");
            }
        }
        private void btnSetAll_Click(object sender, EventArgs e) {
            try {
                if (sender is Button btn) { // 버튼 확인
                    for (int ch = 1; ch < 5; ch++) {
                        string mode = GetChannelMode(ch);

                        TextBox tbIma = Controls.Find($"tbIma{ch}", true).FirstOrDefault() as TextBox; // 전류 입력
                        TextBox tbWus = Controls.Find($"tbWus{ch}", true).FirstOrDefault() as TextBox; // 온 시간 입력
                        TextBox tbDus = Controls.Find($"tbDus{ch}", true).FirstOrDefault() as TextBox; // 오프 시간 입력

                        if (!int.TryParse(tbIma?.Text, out int ima)) { // 전류 값 검증
                            continue;
                        }

                        if (ima > MAX_Ima) { // 최대 전류 초과 시
                            //MessageBox.Show($"Ima value cannot exceed {MAX_Ima}."); // 경고
                            lbLog.Items.Add($"Ima value cannot exceed {MAX_Ima}.");
                            tbIma.Text = MAX_Ima.ToString(); // 최대값으로 보정
                            return;
                        }

                        if (mode == "Continuous") { // 연속 모드 저장
                            channelSettings[ch] = new ChannelSettings { Ima = ima, Wus = 0, Dus = 0 }; // 설정 딕셔너리에 저장
                        }
                        else if (mode == "Pulse") { // 펄스 모드 저장
                            if (!int.TryParse(tbWus?.Text, out int wus) || !int.TryParse(tbDus?.Text, out int dus)) { // 온/오프 검증
                                //MessageBox.Show($"Channel {ch}: Wus or Dus value is invalid."); // 잘못된 값 안내
                                lbLog.Items.Add($"Channel {ch}: Wus or Dus value is invalid.");
                                return;
                            }

                            channelSettings[ch] = new ChannelSettings { Ima = ima, Wus = wus, Dus = dus }; // 설정 저장
                        }
                        else { // 모드가 Off 또는 미설정
                            //MessageBox.Show($"Channel {ch} mode cannot be set. Current mode: {mode}"); // 설정 불가 안내
                            lbLog.Items.Add($"Channel {ch} mode cannot be set. Current mode: {mode}");
                        }
                    }
                }
            }
            catch (Exception ex) {
                //MessageBox.Show($"Error occurred during setting: {ex.Message}"); // 예외 처리
                lbLog.Items.Add($"Error occurred during setting: {ex.Message}");
            }
            //MessageBox.Show($"All Channels setting saved");
            lbLog.Items.Add($"All Channels setting saved");
            lbLog.Items.Add("");
        }

        // 8. UI 이벤트 핸들러
        private void cbSimulationMode_CheckedChanged(object sender, EventArgs e) { // 시뮬레이션 모드
            if (isSimulationMode) {
                btnConnect.Enabled = true;
                lbIsConn.Text = crrLbIsCon;
            }
            else {
                btnConnect.Enabled = false;
                crrLbIsCon = lbIsConn.Text;
                lbIsConn.Text = "SIMULATION MODE";
            }
        }

        private void cbMode_SelectedIndexChanged(object sender, EventArgs e) { // 모드 콤보 변경 시
            if (sender is ComboBox cb && cb.Name.StartsWith("cbMode")) {
                if (int.TryParse(cb.Name.Substring("cbMode".Length), out int ch)) { // 채널 번호 추출
                    var tbIma = Controls.Find($"tbIma{ch}", true).FirstOrDefault() as TextBox; // 전류 입력 컨트롤
                    var tbWus = Controls.Find($"tbWus{ch}", true).FirstOrDefault() as TextBox; // 온 시간 컨트롤
                    var tbDus = Controls.Find($"tbDus{ch}", true).FirstOrDefault() as TextBox; // 오프 시간 컨트롤
                    var btnTrig = Controls.Find($"btnTrigger{ch}", true).FirstOrDefault() as Button; // 트리거 버튼

                    if (tbIma != null && tbWus != null && tbDus != null && btnTrig != null) { // 컨트롤 유효성 확인
                        ModeSetting(cb.Text, tbIma, tbWus, tbDus, btnTrig);
                    }
                }
            }
        }

        private void tbCycleTime_TextChanged(object sender, EventArgs e) { // 사이클 시간 변경 시
            UpdateMaxDelayAndValidate(); // 최대 딜레이 재계산
        }

        private void tbDelay_TextChanged(object sender, EventArgs e) { // 딜레이 변경 시
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
        private string GetChannelMode(int ch) {
            if (ch < 1 || ch > 4) return "Off";
            return modes[ch - 1]?.Text ?? "Off";
        }
    }
}

