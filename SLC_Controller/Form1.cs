using Abeo.HW; // 하드웨어 제어 라이브러리 참조
using System; // 기본 시스템 기능
using System.Collections.Generic; // 제네릭 컬렉션 사용
using System.ComponentModel; // 컴포넌트 모델 지원
using System.Data; // 데이터 관련 타입
using System.Drawing; // UI 색상, 폰트 등
using System.Linq; // LINQ 확장 메서드
using System.Net; // 네트워크 기능
using System.Net.Sockets; // 소켓 통신
using System.Security.Cryptography; // 암호화 기능
using System.Text; // 인코딩 처리
using System.Threading.Tasks; // 비동기/병렬 처리
using System.Timers; // 타이머 이벤트
using System.Windows.Forms; // WinForms UI

namespace SLC_Controller {
    public partial class Form1 : Form {

        // 1. 필드 및 속성
        private AbeoLightCon lc = new AbeoLightCon(); // 조명 컨트롤러 인스턴스
        private System.Timers.Timer testTimer; // 주기 테스트용 타이머
        private Dictionary<int, ChannelSettings> channelSettings = new Dictionary<int, ChannelSettings>(); // 채널별 설정 저장
        public bool isTesting = false; // 테스트 진행 여부
        private bool isSimulationMode = false; // 시뮬레이션 모드 플래그
        private int crrChannel; // 현재 실행 채널 번호
        const int MAX_Ima = 1000; // 전류 최대값 제한
        string crrLbIsCon; // 연결 상태 라벨 백업값

        private class ChannelSettings {
            public int Ima { get; set; } // 전류 설정값
            public int Wus { get; set; } // 온 시간(us)
            public int Dus { get; set; } // 오프 시간(us)
        }

        private ComboBox[] modes => new[] { cbMode1, cbMode2, cbMode3, cbMode4 }; // 채널 모드 콤보 박스 캐시

        // 2. 생성자
        public Form1() { // 폼 생성 시 초기화
            InitializeComponent(); // 디자이너 구성 요소 초기화

            InitUI(); // UI 기본 설정

            cbIP.SelectedIndex = 1; // 기본 IP 선택 인덱스
            cbIP.Visible = false; // IP 선택 콤보 숨김

            cbSimulationMode.CheckedChanged += (s, e) => { // 시뮬레이션 체크 변경 시
                isSimulationMode = cbSimulationMode.Checked; // 플래그 동기화
            };

            testTimer = new System.Timers.Timer(); // 타이머 생성
            testTimer.Elapsed += TestTimer_Elapsed; // 주기 이벤트 핸들러 등록
            testTimer.AutoReset = true; // 자동 반복 설정

            lc.Name = "LC"; //lc.IP = "172.28.37.101"; // 장치 이름 설정
            lc.SetTriggerMode(0); // 트리거 모드 설정

            SetModesTo(0); // 모든 채널 모드를 Off로 초기화
        }

        // 3. UI 초기화 및 설정
        private void InitUI() { // 시작 시 입력 박스 숨김
            for (int i = 1; i <= 4; i++) { // 4개 채널 반복
                foreach (var name in new[] { "tbIma", "tbDus", "tbWus" }) { // 각 설정 텍스트박스
                    (Controls.Find(name + i, true).FirstOrDefault() as TextBox)?.Hide(); // 컨트롤 찾아 숨김
                }
                (Controls.Find("btnTrigger" + i, true).FirstOrDefault() as Button)?.Hide(); // 트리거 버튼 숨김
            }

            tbCycleTime.Text = "5000"; //총 사이클 시간 (초기값) // 기본 사이클 5초
            tbDelay.Text = "1250"; //채널별 할당되는 시간 (초기값) // 기본 딜레이 1.25초
        }

        private void SetControlsEnabled(bool enabled) { // 주요 입력 컨트롤 활성/비활성
            tbCycleTime.Enabled = enabled; // 사이클 입력 활성 여부
            tbDelay.Enabled = enabled; // 딜레이 입력 활성 여부
            btnConnect.Enabled = enabled; // 연결 버튼 활성 여부
        }

        private void SetTbState(TextBox tb, bool visible) { // 텍스트박스 표시/활성 처리
            tb.Visible = visible; // 가시성 변경
            tb.Enabled = visible; // 입력 가능 여부 동기화
        }

        private void SetModesTo(int n) { // 모든 모드 콤보 선택값 설정
            foreach (var cb in modes) { // 채널별 콤보 순회
                cb.SelectedIndex = n; // 동일 인덱스로 설정
            }
        }

        private void ModeSetting(string mode, TextBox tbIma, TextBox tbWus, TextBox tbDus, Button btnTrigger) { // 모드에 따른 UI 표시
            bool imaVis = false, wusVis = false, dusVis = false, triggerVis = false; // 표시 여부 플래그

            if (mode == "Pulse") { // 펄스 모드일 때
                imaVis = wusVis = dusVis = triggerVis = true; // 모든 입력/버튼 활성

                if (string.IsNullOrWhiteSpace(tbIma.Text)) tbIma.Text = "500"; // 기본 전류
                if (string.IsNullOrWhiteSpace(tbWus.Text)) tbWus.Text = "400000"; // 기본 온 시간
                if (string.IsNullOrWhiteSpace(tbDus.Text)) tbDus.Text = "0"; // 기본 오프 시간
            }
            else if (mode == "Continuous") { // 연속 모드일 때
                imaVis = triggerVis = true; // 전류와 트리거만 필요

                if (string.IsNullOrWhiteSpace(tbIma.Text)) tbIma.Text = "500"; // 기본 전류
            }

            SetTbState(tbIma, imaVis); // 전류 입력 표시 여부
            SetTbState(tbWus, wusVis); // 온 시간 입력 표시 여부
            SetTbState(tbDus, dusVis); // 오프 시간 입력 표시 여부
            btnTrigger.Visible = triggerVis; // 트리거 버튼 표시
        }

        private void UpdateMaxDelayAndValidate() { // 사이클 시간에 따른 최대 딜레이 계산 및 검증
            try {
                if (!int.TryParse(tbCycleTime.Text, out int cycleTime)) // 사이클 입력 파싱
                    return; // 숫자 아니면 종료

                int maxDelay = cycleTime / 4; // 채널 4개 기준 최대 딜레이
                lbMaxDelay.Text = "MAX: " + maxDelay.ToString(); // 라벨 업데이트

                if (int.TryParse(tbDelay.Text, out int delay)) { // 딜레이 입력 파싱
                    if (delay > maxDelay) { // 최대치 초과 시
                        tbDelay.Text = maxDelay.ToString(); // 최대값으로 보정
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Error occurred while validating delay value: " + ex.Message); // 검증 실패 알림
            }
        }

        // 4. 연결, 네트워크
        private void btnConnect_Click(object sender, EventArgs e) { // Connect 버튼 클릭
            try {
                lc.IP = "172.28.37.101"; //lc.IP = cbIP.SelectedItem?.ToString(); // 장치 IP 설정
                if (string.IsNullOrEmpty(lc.IP)) { // IP 비어있는지 검사
                    MessageBox.Show("IP Address Not Found", "Alert"); // 경고 표시
                    return; // 처리 중단
                }

                lc.Connect(); // 컨트롤러 연결 시도

                if (!lc.Connected) { // 실패 시
                    lbIsConn.Text = "DISCONNECTED"; // 라벨 표시
                    lbIsConn.ForeColor = Color.Red; // 빨간색으로 표시
                    MessageBox.Show("Connection failed", "Error"); // 실패 메시지
                    ShowIP(); // 현재 PC IP 표시
                    return; // 더 진행하지 않음
                }
                else { // 성공 시
                    lbIsConn.Text = "CONNECTED"; // 연결 라벨 갱신
                    lbIsConn.ForeColor = Color.Green; // 초록색 표시
                    MessageBox.Show("Connected"); // 성공 메시지
                    ShowIP(); // IP 정보 표시
                }
            }
            catch (Exception ex) {
                MessageBox.Show("An error occurred during connection:\n" + ex.Message, "Error"); // 예외 처리
            }
        }

        private void ShowIP() { // PC와 컨트롤러 IP 표시
            string localIP = "IP Address NOT FOUND"; // 기본 메시지

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

            MessageBox.Show("Host IP Address: " + localIP + "\nController IP Address: " + lc.IP, "IP STATUS"); // 결과 표시
        }

        // 5. 테스트 모드
        private void btnTest_Click(object sender, EventArgs e) { // TEST 버튼 클릭
            if (!isTesting) { // 테스트 시작 분기
                if (!isSimulationMode && !lc.Connected) { // 실장치 모드인데 미연결이면
                    MessageBox.Show("Not connected"); // 안내 후
                    return; // 종료
                }

                if (int.TryParse(tbCycleTime.Text, out int cTime)) { // 사이클 시간 파싱
                    testTimer.Interval = cTime; // 타이머 주기 설정
                }
                else {
                    MessageBox.Show("Invalid cycle time"); // 숫자 아님
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
                    MessageBox.Show("Trigger failed during test: " + ex.Message); // 테스트 중 예외 알림
                }
            }));
        }

        // 6. 채널 트리거
        private void btnTrigger_Click(object sender, EventArgs e) { // 개별 채널 트리거 버튼
            //TestCapture(0); // 캡처 테스트 (비활성)
            if (!isSimulationMode && !lc.Connected) { // 실장치 모드에서 미연결이면
                MessageBox.Show("Not connected"); // 안내
                return; // 중단
            }

            if (sender is Button btn) { // 눌린 버튼 확인
                int index = int.Parse(btn.Name.Substring("btnTrigger".Length)); // 버튼 이름에서 채널 번호 추출

                if (!channelSettings.ContainsKey(index)) { // 설정 존재 여부
                    MessageBox.Show($"Channel {index} has no setting value. Please press 'Set' button."); // 설정 요청
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
                    MessageBox.Show($"Channel {index} mode is Null or Off."); // 모드가 설정되지 않음
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
                    string mode = GetChannelMode(ch); // 현재 모드 확인

                    TextBox tbIma = Controls.Find($"tbIma{ch}", true).FirstOrDefault() as TextBox; // 전류 입력
                    TextBox tbWus = Controls.Find($"tbWus{ch}", true).FirstOrDefault() as TextBox; // 온 시간 입력
                    TextBox tbDus = Controls.Find($"tbDus{ch}", true).FirstOrDefault() as TextBox; // 오프 시간 입력

                    if (!int.TryParse(tbIma?.Text, out int ima)) { // 전류 값 검증
                        MessageBox.Show($"Channel {ch}: Ima value is invalid."); // 잘못된 값 안내
                        return; // 중단
                    }

                    if (ima > MAX_Ima) { // 최대 전류 초과 시
                        MessageBox.Show($"Ima value cannot exceed {MAX_Ima}."); // 경고
                        tbIma.Text = MAX_Ima.ToString(); // 최대값으로 보정
                        return; // 중단
                    }

                    if (mode == "Continuous") { // 연속 모드 저장
                        channelSettings[ch] = new ChannelSettings { Ima = ima, Wus = 0, Dus = 0 }; // 설정 딕셔너리에 저장
                        MessageBox.Show($"Channel {ch} (Continuous mode) setting saved"); // 저장 완료 안내
                    }
                    else if (mode == "Pulse") { // 펄스 모드 저장
                        if (!int.TryParse(tbWus?.Text, out int wus) || !int.TryParse(tbDus?.Text, out int dus)) { // 온/오프 검증
                            MessageBox.Show($"Channel {ch}: Wus or Dus value is invalid."); // 잘못된 값 안내
                            return; // 중단
                        }

                        channelSettings[ch] = new ChannelSettings { Ima = ima, Wus = wus, Dus = dus }; // 설정 저장
                        MessageBox.Show($"Channel {ch} (Pulse mode) setting saved"); // 저장 알림
                    }
                    else { // 모드가 Off 또는 미설정
                        MessageBox.Show($"Channel {ch} mode cannot be set. Current mode: {mode}"); // 설정 불가 안내
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"Error occurred during setting: {ex.Message}"); // 예외 처리
            }
        }

        // 8. UI 이벤트 핸들러
        private void cbSimulationMode_CheckedChanged(object sender, EventArgs e) { // 시뮬레이션 모드 토글
            if (isSimulationMode) { // 체크됨 = 시뮬레이션 On
                btnConnect.Enabled = true; // 연결 버튼 활성
                lbIsConn.Text = crrLbIsCon; // 기존 연결 상태 복원
            }
            else { // 체크 해제 = 실장치 모드
                btnConnect.Enabled = false; // 연결 버튼 비활성
                crrLbIsCon = lbIsConn.Text; // 현재 상태 백업
                lbIsConn.Text = "SIMULATION MODE"; // 라벨에 모드 표시
            }
        }

        private void cbMode_SelectedIndexChanged(object sender, EventArgs e) { // 모드 콤보 변경 시
            if (sender is ComboBox cb && cb.Name.StartsWith("cbMode")) { // 모드 콤보인지 확인
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
                if (int.TryParse(tb.Text, out int value)) { // 숫자 파싱
                    if (value > 1000) { // 최대치 초과 시
                        tb.Text = MAX_Ima.ToString(); // 최대값으로 교체
                        tb.SelectionStart = tb.Text.Length; // 커서 끝으로 이동
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

