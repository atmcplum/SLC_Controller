using Abeo.HW;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace SLC_Controller {
    public partial class Form1 : Form {

        // 1. 필드 및 속성
        // 상수는 AppConstants 클래스로 분리
        private AbeoLightCon lc = new AbeoLightCon();
        private System.Timers.Timer testTimer;
        private readonly ConnectionService connectionService;
        private readonly Dictionary<int, ChannelSettings> channelSettings = new Dictionary<int, ChannelSettings>();
        private readonly Dictionary<int, int> highlightVersions = new Dictionary<int, int>();
        private readonly Dictionary<int, Color> highlightBaseColors = new Dictionary<int, Color>();
        private readonly Dictionary<int, bool> continuousOn = new Dictionary<int, bool>();
        private bool isTesting = false;
        private bool runStatus = false;
        private int crrChannel;

        private ComboBox[] modes => new[] { cbMode1, cbMode2, cbMode3, cbMode4 };

        // 2. 생성자
        public Form1() {
            InitializeComponent();
            connectionService = new ConnectionService(lc);
            connectionService.LogMessage += Log;
            connectionService.StateChanged += ConnectionService_StateChanged;
            connectionService.ConnectionLost += ConnectionService_ConnectionLost;

            InitUI();
            cbIP.SelectedIndex = 1;
            cbIP.Visible = false;

            testTimer = new System.Timers.Timer();
            testTimer.Elapsed += TestTimer_Elapsed;
            testTimer.AutoReset = true;

            clockTimer.Interval = AppConstants.ClockTimerIntervalMs;
            clockTimer.Start();

            lc.Name = "LC"; //lc.IP = "172.28.37.101"; // 장치 이름 설정            
        }

        // 3. UI 초기화 및 설정
        private void InitUI() {
            Log(AppConstants.LogInitUi);
            SetModesTo(0);
            lc.SetTriggerMode(0);
            for (int i = AppConstants.ChannelMin; i <= AppConstants.ChannelMax; i++) {
                foreach (var name in new[] { "tbIma", "tbDus", "tbWus" }) {
                    (Controls.Find(name + i, true).FirstOrDefault() as TextBox)?.Hide();
                }
                (Controls.Find("btnTrigger" + i, true).FirstOrDefault() as Button)?.Hide();
            }
            Log(AppConstants.LogInitCycleDefaults);
            tbCycleTime.Text = AppConstants.CycleTimeDefaultMs.ToString();
            tbDelay.Text = AppConstants.DelayDefaultMs.ToString();
            Log(AppConstants.LogUiDefaults, AppConstants.CycleTimeDefaultMs, AppConstants.DelayDefaultMs);
            UpdateConnStatusLabel();

            Log(AppConstants.LogInitDone);
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

        private void SetModesTo(int n) { // 모든 채널 모드 설정
            foreach (var cb in modes) {
                cb.SelectedIndex = n;
            }
        }

        private void ModeSetting(string mode, TextBox tbIma, TextBox tbWus, TextBox tbDus, Button btnTrigger) { // 모드에 따른 UI 설정
            bool imaVis = false, wusVis = false, dusVis = false, triggerVis = false;

            if (mode == AppConstants.ModePulse) {
                imaVis = wusVis = dusVis = triggerVis = true;
                btnTrigger.Text = AppConstants.TextTrig;

                if (string.IsNullOrWhiteSpace(tbIma.Text)) tbIma.Text = AppConstants.ImaDefault.ToString(); // 기본 전류
                if (string.IsNullOrWhiteSpace(tbWus.Text)) tbWus.Text = AppConstants.WusDefault.ToString(); // 기본 온 시간
                if (string.IsNullOrWhiteSpace(tbDus.Text)) tbDus.Text = AppConstants.DusDefault.ToString(); // 기본 오프 시간
            }
            else if (mode == AppConstants.ModeContinuous) {
                imaVis = triggerVis = true;
                btnTrigger.Text = AppConstants.TextOn;

                if (string.IsNullOrWhiteSpace(tbIma.Text)) tbIma.Text = AppConstants.ImaDefault.ToString(); // 기본 전류
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
                lbMaxDelay.Text = AppConstants.TextMaxPrefix + maxDelay.ToString();

                if (int.TryParse(tbDelay.Text, out int delay)) { // 딜레이 입력 파싱
                    if (delay > maxDelay) { // 최대치 초과 시
                        tbDelay.Text = maxDelay.ToString(); // 최대값으로 보정
                    }
                }
            }
            catch (Exception ex) {
                Log(AppConstants.LogDelayValidateError, ex.Message);
            }
        }

        // 4. 연결, 네트워크
        private async void btnConnect_Click(object sender, EventArgs e) {
            Log(AppConstants.LogNetConnectStart);
            btnConnect.Enabled = false;
            btnConnect.BackColor = AppConstants.ColorConnectBase;
            btnConnect.Text = AppConstants.TextConnecting;
            UpdateConnStatusLabel();

            try {
                await connectionService.ConnectAsync(AppConstants.DefaultControllerIp);
            }
            finally {
                btnConnect.Enabled = true;
            }
        }

        // 5. 테스트 모드
        private void btnTest_Click(object sender, EventArgs e) {
            if (!isTesting) {
                Log(AppConstants.LogTestStartRequested);
                if (!cbSimulationMode.Checked && !connectionService.IsConnected) { // 실장치 모드인데 미연결이면
                    Log(AppConstants.LogTestNotConnected);
                    return;
                }

                if (int.TryParse(tbCycleTime.Text, out int cTime)) {
                    testTimer.Interval = cTime;
                    Log(AppConstants.LogTestTimerInterval, cTime);
                }
                else {
                    Log(AppConstants.LogTestInvalidCycle);
                    return;
                }

                isTesting = true;
                SetControlsEnabled(false); // 주요 입력 비활성화
                btnTest.Text = AppConstants.TextTesting;
                crrChannel = 1;

                Log(AppConstants.LogTestStarted);
                TrigChannels(); // 즉시 한번 실행
                testTimer.Start();
            }
            else {
                Log(AppConstants.LogTestStopRequested);
                StopTest(AppConstants.ReasonManual);
            }
        }

        private void TestTimer_Elapsed(object sender, ElapsedEventArgs e) {
            TrigChannels();
        }

        private void StopTest(string reason) { // 테스트 종료 공통 처리
            if (!isTesting) return;
            Log(AppConstants.LogTestStopped, reason);
            isTesting = false;
            SetControlsEnabled(true); // 입력 활성화
            btnTest.Text = AppConstants.TextTest;
            testTimer.Stop();
            SetModesTo(0);
            channelSettings.Clear();
        }

        /// <summary>
        /// 각 채널을 순차적으로 트리거하며 설정된 출력 모드와 지연 시간을 적용
        /// </summary>
        /// <remarks>
        /// UI 스레드에서 실행되며, 실행 전에 연결 상태와 시뮬레이션 모드를 확인
        /// 각 채널의 설정에 따라 Pulse 또는 Continuous 출력을 수행하고, 지정된 시간 동안 해당 채널 표시를 강조
        /// </remarks>
        private void TrigChannels() { // 4개 채널을 순회하며 트리거
            Invoke((MethodInvoker)(async () => {
                try {
                    if (!cbSimulationMode.Checked && !connectionService.IsConnected) { // 연결 해제 시 테스트 중단
                        StopTest(AppConstants.ReasonDisconnected);
                        return;
                    }
                    if (!int.TryParse(tbDelay.Text, out int delay))
                        delay = AppConstants.DefaultDelayMs;

                    for (int ch = AppConstants.ChannelMin; ch <= AppConstants.ChannelMax; ch++) {
                        string mode = GetChannelMode(ch);

                        if (!channelSettings.TryGetValue(ch, out var setting)) // 설정 없으면
                            continue;

                        if (!cbSimulationMode.Checked && !connectionService.IsConnected)
                            continue;

                        Log(AppConstants.LogChTrigger, ch, mode);

                        if (mode == AppConstants.ModePulse) {
                            if (!cbSimulationMode.Checked) {
                                lc.SetPulseOutput(ch, setting.Ima, setting.Wus, setting.Dus);
                                lc.Trigger(ch);
                            }

                            int dusMs = Math.Max(0, setting.Dus / 1000);
                            if (dusMs > 0) {
                                await Task.Delay(dusMs);
                            }
                            int durationMs = Math.Max(AppConstants.MinDurationMs, setting.Wus / 1000);
                            _ = HighlightChannel(ch, AppConstants.ColorHighlightTest, durationMs);
                            await Task.Delay(delay); // 다음 채널 전 대기
                        }
                        else if (mode == AppConstants.ModeContinuous) {
                            if (!cbSimulationMode.Checked)
                                lc.SetContinousOutput(ch, setting.Ima); // 지속 출력 설정

                            _ = HighlightChannel(ch, AppConstants.ColorHighlightTest, delay);
                            await Task.Delay(delay); // 지정 딜레이 만큼 대기

                            if (!cbSimulationMode.Checked)
                                SetOffMode(ch);
                        }
                    }
                }
                catch (Exception ex) {
                    Log(AppConstants.LogTriggerFailed, ex.Message);
                }
            }));
        }

        // 6. 채널 트리거
        private async void btnTrigger_Click(object sender, EventArgs e) {
            //TestCapture(0); // 캡처 테스트 (비활성)
            if (!cbSimulationMode.Checked && !connectionService.IsConnected) { // 실장치 모드에서 미연결이면
                Log(AppConstants.LogChNotConnected);
                return;
            }

            if (sender is Button btn) {
                int index = int.Parse(btn.Name.Substring("btnTrigger".Length)); // 버튼 이름에서 채널 번호 추출

                if (!channelSettings.ContainsKey(index)) {
                    Log(AppConstants.LogChNoSetting, index);
                    return;
                }

                var setting = channelSettings[index]; // 채널 설정 가져오기
                string mode = GetChannelMode(index); // 채널 모드 확인

                Log(AppConstants.LogChManualTrigger, index, mode);

                if (mode == AppConstants.ModePulse) { // 펄스 모드
                    Log(AppConstants.LogChPulse, index, setting.Ima, setting.Wus, setting.Dus);

                    if (!cbSimulationMode.Checked) {
                        lc.SetPulseOutput(index, setting.Ima, setting.Wus, setting.Dus);
                        lc.Trigger(index);
                    }

                    int dusMs = Math.Max(0, setting.Dus / 1000);
                    if (dusMs > 0) {
                        await Task.Delay(dusMs);
                    }
                    int duration = Math.Max(AppConstants.MinDurationMs, setting.Wus / 1000);
                    _ = HighlightChannel(index, AppConstants.ColorHighlightManual, duration);
                }
                else if (mode == AppConstants.ModeContinuous) { // 연속 모드
                    Log(AppConstants.LogChContinuous, index, setting.Ima);

                    bool isOn = continuousOn.TryGetValue(index, out bool currentOn) && currentOn;
                    bool nextOn = !isOn;
                    continuousOn[index] = nextOn;

                if (!cbSimulationMode.Checked) {
                    if (nextOn) {
                        lc.SetContinousOutput(index, setting.Ima);
                    }
                    else {
                        SetOffMode(index);
                    }
                    }

                    if (nextOn) {
                        SetChannelLabelColor(index, AppConstants.ColorHighlightManual); // ON 상태 표시
                    }
                    else {
                        RestoreChannelLabelColor(index); // OFF 상태 복구
                    }
                    btn.Text = nextOn ? AppConstants.TextOff : AppConstants.TextOn;
                }
                else {
                    Log(AppConstants.LogChModeNullOff, index);
                }
            }
        }

        private void SetOffMode(int ch) { // 출력 종료를 위한 최소 펄스 세팅
            lc.SetPulseOutput(ch, AppConstants.MinPulseIma, AppConstants.MinPulseWus, AppConstants.MinPulseDus); // 미미한 펄스로 꺼짐 처리
        }

        private async Task HighlightChannel(int ch, Color color, int durationMs) {
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
                if (lb == null) { return;  }
                if (!highlightBaseColors.ContainsKey(ch)) {
                    highlightBaseColors[ch] = lb.BackColor;
                }
                baseColor = highlightBaseColors[ch];
                lb.BackColor = color;
            }

            if (lb == null) { return; }

            // 지정 시간만큼 하이라이트 유지
            await Task.Delay(durationMs);  // duration in ms // 지정 시간 대기

            if (highlightVersions[ch] == token) {
                if (InvokeRequired) {
                    BeginInvoke((MethodInvoker)(() => lb.BackColor = baseColor));
                }
                else {
                    lb.BackColor = baseColor;
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

            if (InvokeRequired) { Invoke((MethodInvoker)Apply); }
            else { Apply(); }
        }

        private void RestoreChannelLabelColor(int ch) { // 채널 라벨 색상 복원
            void Apply() {
                if (!highlightBaseColors.TryGetValue(ch, out var baseColor)) return;
                Label lb = Controls.Find("lbCh" + ch, true).FirstOrDefault() as Label; // 라벨 찾기
                if (lb == null) return;
                lb.BackColor = baseColor;
            }

            if (InvokeRequired) { Invoke((MethodInvoker)Apply); }
            else { Apply(); }
        }

        // 7. 설정 버튼
        private void btnSet_Click(object sender, EventArgs e) {
            try {
                if (sender is Button btn) {
                    int ch = int.Parse(btn.Name.Substring("btnSet".Length)); // 채널 번호 추출
                    string mode = GetChannelMode(ch);

                    Log(AppConstants.LogChSetRequested, ch, mode);

                    TextBox tbIma = Controls.Find($"tbIma{ch}", true).FirstOrDefault() as TextBox; // 전류 입력
                    TextBox tbWus = Controls.Find($"tbWus{ch}", true).FirstOrDefault() as TextBox; // 온 시간 입력
                    TextBox tbDus = Controls.Find($"tbDus{ch}", true).FirstOrDefault() as TextBox; // 오프 시간 입력

                    if (!int.TryParse(tbIma?.Text, out int ima)) { // 전류 값 검증
                        Log(AppConstants.LogChSetImaInvalid, ch);
                        return;
                    }

                    if (ima > AppConstants.MaxIma) { // 최대 전류 초과 시
                        Log(AppConstants.LogChSetImaExceeds, ch, AppConstants.MaxIma);
                        tbIma.Text = AppConstants.MaxIma.ToString(); // 최대값으로 보정
                        return;
                    }

                    if (mode == AppConstants.ModeContinuous) { // 연속 모드 저장
                        channelSettings[ch] = new ChannelSettings { Ima = ima, Wus = 0, Dus = 0 }; // 설정 딕셔너리에 저장
                        Log(AppConstants.LogChSetSavedContinuous, ch, ima);
                    }
                    else if (mode == AppConstants.ModePulse) { // 펄스 모드 저장
                        if (!int.TryParse(tbWus?.Text, out int wus) || !int.TryParse(tbDus?.Text, out int dus)) { 
                            Log(AppConstants.LogChSetWusDusInvalid, ch);
                            return;
                        }
                        if (dus > AppConstants.MaxDusUs) {
                            Log(AppConstants.LogChSetDusAdjust, ch, AppConstants.MaxDusUs);
                            dus = AppConstants.MaxDusUs;
                            tbDus.Text = dus.ToString();
                        }

                        channelSettings[ch] = new ChannelSettings { Ima = ima, Wus = wus, Dus = dus }; // 설정 저장
                        Log(AppConstants.LogChSetSavedPulse, ch, ima, wus, dus);
                    }
                    else { 
                        Log(AppConstants.LogChSetSkipped, ch, mode);
                    }
                }
            }
            catch (Exception ex) {
                Log(AppConstants.LogSetException, ex.Message);
            }
        }
        private void btnSetAll_Click(object sender, EventArgs e) { 
            try {
                if (sender is Button btn) { 
                    Log(AppConstants.LogChSetAllRequested);
                    for (int ch = AppConstants.ChannelMin; ch <= AppConstants.ChannelMax; ch++) {
                        string mode = GetChannelMode(ch);

                        TextBox tbIma = Controls.Find($"tbIma{ch}", true).FirstOrDefault() as TextBox; // 전류 입력
                        TextBox tbWus = Controls.Find($"tbWus{ch}", true).FirstOrDefault() as TextBox; // 온 시간 입력
                        TextBox tbDus = Controls.Find($"tbDus{ch}", true).FirstOrDefault() as TextBox; // 오프 시간 입력

                        if (!int.TryParse(tbIma?.Text, out int ima)) { // 전류 값 검증
                            Log(AppConstants.LogChSetAllImaInvalid, ch);
                            continue;
                        }

                        if (ima > AppConstants.MaxIma) { // 최대 전류 초과 시
                            Log(AppConstants.LogChSetAllImaExceeds, ch, AppConstants.MaxIma);
                            tbIma.Text = AppConstants.MaxIma.ToString(); // 최대값으로 보정
                            return;
                        }

                        if (mode == AppConstants.ModeContinuous) { // 연속 모드 저장
                            channelSettings[ch] = new ChannelSettings { Ima = ima, Wus = 0, Dus = 0 }; // 설정 딕셔너리에 저장
                            Log(AppConstants.LogChSetAllSavedContinuous, ch, ima);
                        }
                        else if (mode == AppConstants.ModePulse) { // 펄스 모드 저장
                            if (!int.TryParse(tbWus?.Text, out int wus) || !int.TryParse(tbDus?.Text, out int dus)) { // 온/오프 검증
                                Log(AppConstants.LogChSetAllWusDusInvalid, ch);
                                return;
                            }
                            if (dus > AppConstants.MaxDusUs) {
                                Log(AppConstants.LogChSetAllDusAdjust, ch, AppConstants.MaxDusUs);
                                dus = AppConstants.MaxDusUs;
                                tbDus.Text = dus.ToString();
                            }

                            channelSettings[ch] = new ChannelSettings { Ima = ima, Wus = wus, Dus = dus }; // 설정 저장
                            Log(AppConstants.LogChSetAllSavedPulse, ch, ima, wus, dus);
                        }
                        else { 
                            Log(AppConstants.LogChSetAllSkipped, ch, mode);
                        }
                    }
                }
            }
            catch (Exception ex) {
                Log(AppConstants.LogSetAllException, ex.Message);
            }
            Log(AppConstants.LogChSetAllDone);
        }

        // 8. UI 이벤트 핸들러
        private void cbSimulationMode_CheckedChanged(object sender, EventArgs e) { 
            if (!cbSimulationMode.Checked) {
                btnConnect.Enabled = true;
                this.BackColor = AppConstants.ColorConnectBase;
                UpdateConnStatusLabel();
                Log(AppConstants.LogSimOff);
            }
            else {
                btnConnect.Enabled = false;
                lbIsConn.Text = AppConstants.TextSimulation;
                this.BackColor = AppConstants.ColorSimulationBack;
                UpdateConnStatusLabel();
                Log(AppConstants.LogSimOn);
            }
        }

        private void cbMode_SelectedIndexChanged(object sender, EventArgs e) {
            if (sender is ComboBox cb && cb.Name.StartsWith("cbMode")) {
                if (int.TryParse(cb.Name.Substring("cbMode".Length), out int ch)) { // 채널 번호 추출
                    Log(AppConstants.LogChModeChanged, ch, cb.Text);
                    var tbIma = Controls.Find($"tbIma{ch}", true).FirstOrDefault() as TextBox; // 전류 입력 컨트롤
                    var tbWus = Controls.Find($"tbWus{ch}", true).FirstOrDefault() as TextBox; // 온 시간 컨트롤
                    var tbDus = Controls.Find($"tbDus{ch}", true).FirstOrDefault() as TextBox; // 오프 시간 컨트롤
                    var btnTrig = Controls.Find($"btnTrigger{ch}", true).FirstOrDefault() as Button; // 트리거 버튼

                    if (tbIma != null && tbWus != null && tbDus != null && btnTrig != null) { // 컨트롤 유효성 확인
                        ModeSetting(cb.Text, tbIma, tbWus, tbDus, btnTrig);
                    }
                    else {
                        Log(AppConstants.LogChModeSettingSkip, ch);
                    }
                }
            }
        }

        private void tbCycleTime_TextChanged(object sender, EventArgs e) {
            UpdateMaxDelayAndValidate();
        }
        private void tbDelay_TextChanged(object sender, EventArgs e) {
            UpdateMaxDelayAndValidate();
        }

        private void tbIma_TextChanged(object sender, EventArgs e) {
            if (sender is TextBox tb) {
                if (int.TryParse(tb.Text, out int value)) {
                    if (value > AppConstants.MaxIma) {
                        tb.Text = AppConstants.MaxIma.ToString();
                        tb.SelectionStart = tb.Text.Length;
                    }
                }
            }
        }

        // 9. 헬퍼 메서드
        private void Log(string format, params object[] args) { // 로그 다양성을 위한 가변 파라미터
            if (rtbLog.InvokeRequired) {
                rtbLog.BeginInvoke(new Action<string, object[]>(Log), format, args);
                return;
            }

            string message = args?.Length > 0
                ? string.Format(format, args)
                : format;

            string ts = DateTime.Now.ToString(AppConstants.LogTimestampFormat);

            rtbLog.AppendText($"[{ts}] {message}\n");
            rtbLog.ScrollToCaret();
        }

        private string GetChannelMode(int ch) {
            if (ch < AppConstants.ChannelMin || ch > AppConstants.ChannelMax) return AppConstants.ModeOff;
            return modes[ch - 1]?.Text ?? AppConstants.ModeOff;
        }

        private void UpdateConnStatusLabel() {
            if (cbSimulationMode.Checked) {
                lbIsConn.Text = AppConstants.TextSimulation;
                return;
            }

            if (connectionService.IsConnecting) {
                lbIsConn.Text = AppConstants.TextConnecting;
                return;
            }
            lbIsConn.Text = connectionService.IsConnected ? "" : AppConstants.TextDisconnected;
        }

        private void clockTimer_Tick(object sender, EventArgs e) {
            lblClock.Text = DateTime.Now.ToString(AppConstants.ClockTimestampFormat);
            TryPingLog();
        }

        private void TryPingLog() {
            if (cbSimulationMode.Checked) return;
            connectionService.TickPing();
        }

        private void runTimer_Tick(object sender, EventArgs e) {
            if (runStatus) {
                btnConnect.BackColor = AppConstants.ColorConnectBase;
                runStatus = false;
            }
            else {
                btnConnect.BackColor = AppConstants.ColorConnectedBlink;
                btnConnect.Text = AppConstants.TextConnected;
                runStatus = true;
            }
        }

        private void ConnectionService_StateChanged() {
            UI(() => {
                if (connectionService.IsConnecting) {
                    UpdateConnStatusLabel();
                    return;
                }

                if (connectionService.IsConnected) {
                    lbIsConn.ForeColor = AppConstants.ColorConnected;
                    runTimer.Interval = AppConstants.RunTimerIntervalMs;
                    runTimer.Stop();
                    runTimer.Start();
                }
                else {
                    lbIsConn.ForeColor = AppConstants.ColorDisconnected;
                    runTimer.Stop();
                    btnConnect.Text = AppConstants.TextConnect;
                    btnConnect.BackColor = AppConstants.ColorConnectBase;
                }

                UpdateConnStatusLabel();
            });
        }

        private void ConnectionService_ConnectionLost() {
            UI(() => {
                if (cbSimulationMode.Checked) return;
                StopTest(AppConstants.ReasonDisconnected);
            });
        }
        private void UI(Action action) {
            if (IsDisposed || Disposing) return;
            if (InvokeRequired) Invoke(action);
            else action();
        }
    }
}
