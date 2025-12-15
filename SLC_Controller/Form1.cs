using Abeo.HW;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
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
        private int crrChannel;
        const int MAX_Ima = 1000;
        string crrLbIsCon;

        private class ChannelSettings {
            public int Ima { get; set; }
            public int Wus { get; set; }
            public int Dus { get; set; }
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

            lc.Name = "LC"; //lc.IP = "172.28.37.101";
            lc.SetTriggerMode(0);

            SetModesTo(0);
        }

        // 3. UI 초기화 및 설정
        private void InitUI() {
            for (int i = 1; i <= 4; i++) {
                foreach (var name in new[] { "tbIma", "tbDus", "tbWus" }) {
                    (Controls.Find(name + i, true).FirstOrDefault() as TextBox)?.Hide();
                }
                (Controls.Find("btnTrigger" + i, true).FirstOrDefault() as Button)?.Hide();
            }

            tbCycleTime.Text = "5000";
            tbDelay.Text = "1250";
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

                if (string.IsNullOrWhiteSpace(tbIma.Text)) tbIma.Text = "1000";
                if (string.IsNullOrWhiteSpace(tbWus.Text)) tbWus.Text = "400000";
                if (string.IsNullOrWhiteSpace(tbDus.Text)) tbDus.Text = "0";
            }
            else if (mode == "Continuous") {
                imaVis = triggerVis = true;

                if (string.IsNullOrWhiteSpace(tbIma.Text)) tbIma.Text = "1000";
            }

            SetTbState(tbIma, imaVis);
            SetTbState(tbWus, wusVis);
            SetTbState(tbDus, dusVis);
            btnTrigger.Visible = triggerVis;
        }

        private void UpdateMaxDelayAndValidate() {
            try {
                if (!int.TryParse(tbCycleTime.Text, out int cycleTime))
                    return;

                int maxDelay = cycleTime / 4;
                lbMaxDelay.Text = "MAX: " + maxDelay.ToString();

                if (int.TryParse(tbDelay.Text, out int delay)) {
                    if (delay > maxDelay) {
                        tbDelay.Text = maxDelay.ToString();
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Error occurred while validating delay value: " + ex.Message);
            }
        }

        // 4. 연결, 네트워크
        private void btnConnect_Click(object sender, EventArgs e) {
            try {
                lc.IP = "172.28.37.101"; //lc.IP = cbIP.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(lc.IP)) {
                    MessageBox.Show("IP Address Not Found", "Alert");
                    return;
                }

                lc.Connect();

                if (!lc.Connected) {
                    lbIsConn.Text = "DISCONNECTED";
                    lbIsConn.ForeColor = Color.Red;
                    MessageBox.Show("Connection failed", "Error");
                    ShowIP();
                    return;
                }
                else {
                    lbIsConn.Text = "CONNECTED";
                    lbIsConn.ForeColor = Color.Green;
                    MessageBox.Show("Connected");
                    ShowIP();
                }
            }
            catch (Exception ex) {
                MessageBox.Show("An error occurred during connection:\n" + ex.Message, "Error");
            }
        }

        private void ShowIP() {
            string localIP = "IP Address NOT FOUND";

            try {
                string hostName = Dns.GetHostName();
                IPHostEntry hostEntry = Dns.GetHostEntry(hostName);

                foreach (IPAddress ip in hostEntry.AddressList) {
                    if (ip.AddressFamily == AddressFamily.InterNetwork) {
                        localIP = ip.ToString();
                        break;
                    }
                }
            }
            catch (Exception ex) {
                localIP = "ERROR: " + ex.Message;
            }

            MessageBox.Show("Host IP Address: " + localIP + "\nController IP Address: " + lc.IP, "IP STATUS");
        }

        // 5. 테스트 모드
        private void btnTest_Click(object sender, EventArgs e) {
            if (!isTesting) {
                if (!isSimulationMode && !lc.Connected) {
                    MessageBox.Show("Not connected");
                    return;
                }

                if (int.TryParse(tbCycleTime.Text, out int cTime)) {
                    testTimer.Interval = cTime;
                }
                else {
                    MessageBox.Show("Invalid cycle time");
                    return;
                }

                isTesting = true;
                SetControlsEnabled(false);
                btnTest.Text = "TESTING";
                crrChannel = 1;

                TrigChannels();
                testTimer.Start();
            }
            else {
                isTesting = false;
                SetControlsEnabled(true);
                btnTest.Text = "TEST";
                testTimer.Stop();
                SetModesTo(0);
                channelSettings.Clear();
            }
        }

        private void TestTimer_Elapsed(object sender, ElapsedEventArgs e) {
            TrigChannels();
        }

        private void TrigChannels() {
            Invoke((MethodInvoker)(async () => {
                try {
                    if (!int.TryParse(tbDelay.Text, out int delay))
                        delay = 1000;

                    for (int ch = 1; ch <= 4; ch++) {
                        string mode = GetChannelMode(ch);

                        if (!channelSettings.TryGetValue(ch, out var setting))
                            continue;

                        if (!isSimulationMode && !lc.Connected)
                            continue;

                        if (mode == "Pulse") {
                            if (!isSimulationMode) {
                                lc.SetPulseOutput(ch, setting.Ima, setting.Wus, setting.Dus);
                                lc.Trigger(ch);
                            }

                            int durationMs = Math.Max(1, setting.Wus / 1000);
                            _ = HighlightChannel(ch, Color.Lime, durationMs);
                            await Task.Delay(delay);
                        }
                        else if (mode == "Continuous") {
                            if (!isSimulationMode)
                                lc.SetContinousOutput(ch, setting.Ima);

                            _ = HighlightChannel(ch, Color.Lime, delay);
                            await Task.Delay(delay);

                            if (!isSimulationMode)
                                SetOffMode(ch);
                        }
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show("Trigger failed during test: " + ex.Message);
                }
            }));
        }

        // 6. 채널 트리거
        private void btnTrigger_Click(object sender, EventArgs e) {
            //TestCapture(0);
            if (!isSimulationMode && !lc.Connected) {
                MessageBox.Show("Not connected");
                return;
            }

            if (sender is Button btn) {
                int index = int.Parse(btn.Name.Substring("btnTrigger".Length));

                if (!channelSettings.ContainsKey(index)) {
                    MessageBox.Show($"Channel {index} has no setting value. Please press 'Set' button.");
                    return;
                }

                var setting = channelSettings[index];
                string mode = GetChannelMode(index);

                if (mode == "Pulse") {
                    if (!isSimulationMode) {
                        lc.SetPulseOutput(index, setting.Ima, setting.Wus, setting.Dus);
                        lc.Trigger(index);
                    }

                    int duration = Math.Max(1, setting.Wus / 1000);
                    _ = HighlightChannel(index, Color.Orange, duration);
                }
                else if (mode == "Continuous") {
                    if (!isSimulationMode)
                        lc.SetContinousOutput(index, setting.Ima);

                    _ = HighlightChannel(index, Color.Orange, 1000);

                    if (!isSimulationMode)
                        SetOffMode(index);
                }
                else {
                    MessageBox.Show($"Channel {index} mode is Null or Off.");
                }
            }
        }

        private void SetOffMode(int ch) {
            lc.SetPulseOutput(ch, 1, 1, 0);
        }

        private async Task HighlightChannel(int ch, Color color, int durationMs) {
            Label lb = Controls.Find("lbCh" + ch, true).FirstOrDefault() as Label;
            if (lb != null) {
                Color original = lb.BackColor;
                lb.BackColor = color;
                await Task.Delay(durationMs);  // duration in ms
                lb.BackColor = original;
            }
        }

        // 7. 설정 버튼
        private void btnSet_Click(object sender, EventArgs e) {
            try {
                if (sender is Button btn) {
                    int ch = int.Parse(btn.Name.Substring("btnSet".Length));
                    string mode = GetChannelMode(ch);

                    TextBox tbIma = Controls.Find($"tbIma{ch}", true).FirstOrDefault() as TextBox;
                    TextBox tbWus = Controls.Find($"tbWus{ch}", true).FirstOrDefault() as TextBox;
                    TextBox tbDus = Controls.Find($"tbDus{ch}", true).FirstOrDefault() as TextBox;

                    if (!int.TryParse(tbIma?.Text, out int ima)) {
                        MessageBox.Show($"Channel {ch}: Ima value is invalid.");
                        return;
                    }

                    if (ima > MAX_Ima) {
                        MessageBox.Show($"Ima value cannot exceed {MAX_Ima}.");
                        tbIma.Text = MAX_Ima.ToString();
                        return;
                    }

                    if (mode == "Continuous") {
                        channelSettings[ch] = new ChannelSettings { Ima = ima, Wus = 0, Dus = 0 };
                        MessageBox.Show($"Channel {ch} (Continuous mode) setting saved");
                    }
                    else if (mode == "Pulse") {
                        if (!int.TryParse(tbWus?.Text, out int wus) || !int.TryParse(tbDus?.Text, out int dus)) {
                            MessageBox.Show($"Channel {ch}: Wus or Dus value is invalid.");
                            return;
                        }

                        channelSettings[ch] = new ChannelSettings { Ima = ima, Wus = wus, Dus = dus };
                        MessageBox.Show($"Channel {ch} (Pulse mode) setting saved");
                    }
                    else {
                        MessageBox.Show($"Channel {ch} mode cannot be set. Current mode: {mode}");
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"Error occurred during setting: {ex.Message}");
            }
        }

        // 8. UI 이벤트 핸들러
        private void cbSimulationMode_CheckedChanged(object sender, EventArgs e) {
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

        private void cbMode_SelectedIndexChanged(object sender, EventArgs e) {
            if (sender is ComboBox cb && cb.Name.StartsWith("cbMode")) {
                if (int.TryParse(cb.Name.Substring("cbMode".Length), out int ch)) {
                    var tbIma = Controls.Find($"tbIma{ch}", true).FirstOrDefault() as TextBox;
                    var tbWus = Controls.Find($"tbWus{ch}", true).FirstOrDefault() as TextBox;
                    var tbDus = Controls.Find($"tbDus{ch}", true).FirstOrDefault() as TextBox;
                    var btnTrig = Controls.Find($"btnTrigger{ch}", true).FirstOrDefault() as Button;

                    if (tbIma != null && tbWus != null && tbDus != null && btnTrig != null) {
                        ModeSetting(cb.Text, tbIma, tbWus, tbDus, btnTrig);
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
                    if (value > 1000) {
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