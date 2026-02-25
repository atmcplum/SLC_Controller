using System.Drawing;

namespace SLC_Controller {
    internal static class AppConstants {
        // 기본값 및 범위
        public const int MaxIma = 1000;
        public const int MaxDusUs = 300000;
        public const int PingIntervalMs = 2000;
        public const int PingTimeoutMs = 300;
        public const int CycleTimeDefaultMs = 4000;
        public const int DelayDefaultMs = 1000;
        public const int ImaDefault = 500;
        public const int WusDefault = 400000;
        public const int DusDefault = 0;
        public const int ClockTimerIntervalMs = 500;
        public const int RunTimerIntervalMs = 500;
        public const int PingFailThreshold = 3;
        public const int DefaultDelayMs = 1000;
        public const int MinDurationMs = 1;
        public const int MinPulseIma = 1;
        public const int MinPulseWus = 1;
        public const int MinPulseDus = 0;

        // 채널
        public const int ChannelMin = 1;
        public const int ChannelMax = 4;
        public const int ChannelCount = 4;

        // 모드
        public const string ModePulse = "Pulse";
        public const string ModeContinuous = "Continuous";
        public const string ModeOff = "Off";

        // UI 텍스트
        public const string TextTrig = "TRIG";
        public const string TextOn = "ON";
        public const string TextOff = "OFF";
        public const string TextTest = "TEST";
        public const string TextTesting = "TESTING";
        public const string TextConnect = "CONNECT";
        public const string TextConnecting = "CONNECTING...";
        public const string TextConnected = "CONNECTED";
        public const string TextDisconnected = "DISCONNECTED";
        public const string TextSimulation = "SIMULATION MODE";
        public const string TextIpNotFound = "IP Address NOT FOUND";
        public const string TextMaxPrefix = "MAX: ";
        public const string TextErrorPrefix = "ERROR: ";

        // 컨트롤러 기본 IP 주소
        public const string DefaultControllerIp = "172.28.37.101";

        // 상황별 로그 메세지
        public const string LogInitUi = "Initializing UI...";
        public const string LogInitCycleDefaults = "[InitUI] CycleTime/Delay Default Setting...";
        public const string LogUiDefaults = "[UI] Set defaults: CycleTime={0}, Delay={1}";
        public const string LogInitDone = "[InitUI] UI initialization completed.";
        public const string LogDelayValidateError = "Error occurred while validating delay value: {0}";

        public const string LogNetConnectStart = "[NET] Connect start";
        public const string LogNetIpNotFound = "[NET] IP Address not found";
        public const string LogNetTargetIp = "[NET] Target IP: {0}";
        public const string LogNetConnectionFailed = "[NET] Connection failed";
        public const string LogNetConnected = "[NET] Connected";
        public const string LogNetConnectException = "[ERR] Connect exception: {0}";
        public const string LogNetHostIp = "[NET] Host IP: {0}";
        public const string LogNetControllerIp = "[NET] Controller IP: {0}";

        public const string LogTestStartRequested = "[TEST] Start requested";
        public const string LogTestNotConnected = "[TEST] Not connected";
        public const string LogTestTimerInterval = "[TEST] Timer interval set to {0} ms";
        public const string LogTestInvalidCycle = "[TEST] Invalid cycle time";
        public const string LogTestStarted = "[TEST] Started";
        public const string LogTestStopRequested = "[TEST] Stop requested";
        public const string LogTestStopped = "[TEST] Stopped ({0})";

        public const string LogChTrigger = "[CH] Ch{0} -> {1}";
        public const string LogTriggerFailed = "[ERR] Trigger failed during test: {0}";
        public const string LogChNotConnected = "[CH] Not connected";
        public const string LogChNoSetting = "[CH] Ch{0} has no setting value. Please press 'Set' button.";
        public const string LogChManualTrigger = "[CH] Manual trigger requested: Ch{0}, Mode={1}";
        public const string LogChPulse = "[CH] Ch{0} Pulse: Ima={1}, Wus={2}, Dus={3}";
        public const string LogChContinuous = "[CH] Ch{0} Continuous: Ima={1}";
        public const string LogChModeNullOff = "[CH] Ch{0} mode is Null or Off.";

        public const string LogChSetRequested = "[CH] Set requested: Ch{0}, Mode={1}";
        public const string LogChSetImaInvalid = "[CH] Set failed: Ch{0} Ima is invalid";
        public const string LogChSetImaExceeds = "[CH] Set failed: Ch{0} Ima exceeds MAX ({1})";
        public const string LogChSetSavedContinuous = "[CH] Set saved: Ch{0} Continuous (Ima={1})";
        public const string LogChSetWusDusInvalid = "[CH] Set failed: Ch{0} Wus/Dus is invalid";
        public const string LogChSetDusAdjust = "[CH] Set adjust: Ch{0} Dus exceeds MAX ({1})";
        public const string LogChSetSavedPulse = "[CH] Set saved: Ch{0} Pulse (Ima={1}, Wus={2}, Dus={3})";
        public const string LogChSetSkipped = "[CH] Set skipped: Ch{0} mode cannot be set (Mode={1})";
        public const string LogSetException = "[ERR] Set exception: {0}";

        public const string LogChSetAllRequested = "[CH] SetAll requested";
        public const string LogChSetAllImaInvalid = "[CH] SetAll skip: Ch{0} Ima is invalid";
        public const string LogChSetAllImaExceeds = "[CH] SetAll failed: Ch{0} Ima exceeds MAX ({1})";
        public const string LogChSetAllSavedContinuous = "[CH] SetAll saved: Ch{0} Continuous (Ima={1})";
        public const string LogChSetAllWusDusInvalid = "[CH] SetAll failed: Ch{0} Wus/Dus is invalid";
        public const string LogChSetAllDusAdjust = "[CH] SetAll adjust: Ch{0} Dus exceeds MAX ({1})";
        public const string LogChSetAllSavedPulse = "[CH] SetAll saved: Ch{0} Pulse (Ima={1}, Wus={2}, Dus={3})";
        public const string LogChSetAllSkipped = "[CH] SetAll skip: Ch{0} mode cannot be set (Mode={1})";
        public const string LogSetAllException = "[ERR] SetAll exception: {0}";
        public const string LogChSetAllDone = "[CH] SetAll done";

        public const string LogSimOff = "[SIM] Simulation mode: OFF";
        public const string LogSimOn = "[SIM] Simulation mode: ON";
        public const string LogChModeChanged = "[CH] Ch{0} mode changed -> {1}";
        public const string LogChModeSettingSkip = "[CH] Skip mode setting (controls missing): Ch{0}";

        // 테스트 종료
        public const string ReasonManual = "manual";
        public const string ReasonDisconnected = "disconnected";

        // 시간 포맷
        public const string LogTimestampFormat = "yyyy-MM-dd HH:mm:ss.fff";
        public const string ClockTimestampFormat = "yyyy-MM-dd HH:mm:ss";

        // 색상
        public static readonly Color ColorConnectBase = Color.FromArgb(0, 33, 44);
        public static readonly Color ColorSimulationBack = Color.FromArgb(133, 44, 0);
        public static readonly Color ColorHighlightTest = Color.Lime;
        public static readonly Color ColorHighlightManual = Color.Orange;
        public static readonly Color ColorDisconnected = Color.Red;
        public static readonly Color ColorConnected = Color.Green;
        public static readonly Color ColorConnectedBlink = Color.Lime;
    }
}
