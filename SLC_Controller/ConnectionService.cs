using Abeo.HW;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SLC_Controller {
    internal sealed class ConnectionService {
        private readonly AbeoLightCon lc;
        private long lastPingTick;
        private int pingInFlight;
        private int pingFailCount;

        public bool IsConnecting { get; private set; }
        public bool IsConnected { get; private set; }
        public bool PingEnabled { get; private set; }

        public event Action StateChanged;
        public event Action ConnectionLost;
        public event Action<string, object[]> LogMessage;

        public ConnectionService(AbeoLightCon lightCon) {
            lc = lightCon ?? throw new ArgumentNullException(nameof(lightCon));
        }

        /// <summary>
        /// 지정 IP로 컨트롤러에 연결을 시도
        /// </summary>
        /// <returns>성공 <see langword="true"/>, 실패 <see langword="false"/></returns>
        public async Task<bool> ConnectAsync(string ip) {
            if (IsConnecting) return false;

            IsConnecting = true;
            StateChanged?.Invoke();

            lc.IP = ip; //lc.IP = cbIP.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(lc.IP)) {
                Log(AppConstants.LogNetIpNotFound);
                IsConnecting = false;
                StateChanged?.Invoke();
                return false;
            }

            Log(AppConstants.LogNetTargetIp, lc.IP);

            bool ok = false;
            try {
                ok = await Task.Run(() => lc.Connect());
            }
            catch (Exception ex) {
                Log(AppConstants.LogNetConnectException, ex.Message);
            }

            if (ok) {
                IsConnected = true;
                PingEnabled = true;
                Interlocked.Exchange(ref pingFailCount, 0);
                Log(AppConstants.LogNetConnected);
            }
            else {
                IsConnected = false;
                PingEnabled = false;
                Interlocked.Exchange(ref pingFailCount, 0);
                Log(AppConstants.LogNetConnectionFailed);
            }

            IsConnecting = false;
            StateChanged?.Invoke();

            LogHostAndControllerIp();
            return ok;
        }

        /// <summary>
        /// Initiates a network ping to the configured IP address if pinging is enabled and the connection is idle. Used
        /// to monitor connection health and detect connectivity issues.
        /// </summary>
        /// <remarks>This method performs a ping operation only if certain conditions are met: the IP
        /// address is set, pinging is enabled, and the connection is not in the process of connecting. If multiple
        /// consecutive ping failures occur, the connection is considered lost and the appropriate handler is invoked.
        /// The method is non-blocking and returns immediately; ping operations are performed asynchronously. Calling
        /// this method repeatedly is safe, as it will not initiate overlapping ping operations.</remarks>
        public void TickPing() {
            if (string.IsNullOrWhiteSpace(lc.IP)) return;
            if (!PingEnabled) return;
            if (IsConnecting) return;

            long nowTick = Environment.TickCount;
            if (nowTick - lastPingTick < AppConstants.PingIntervalMs) return;
            if (Interlocked.Exchange(ref pingInFlight, 1) != 0) return;

            lastPingTick = nowTick;
            _ = Task.Run(() => {
                bool ok = false;
                try {
                    using (var ping = new Ping()) {
                        var reply = ping.Send(lc.IP, AppConstants.PingTimeoutMs);
                        if (reply != null && reply.Status == IPStatus.Success) {
                            ok = true;
                        }
                    }
                }
                catch { ok = false; }

                if (ok) {
                    Interlocked.Exchange(ref pingFailCount, 0);
                }
                else {
                    int fails = Interlocked.Increment(ref pingFailCount);
                    if (fails >= AppConstants.PingFailThreshold) {
                        OnConnectionLost();
                    }
                }
            }).ContinueWith(_ => Interlocked.Exchange(ref pingInFlight, 0));
        }

        private void OnConnectionLost() {
            if (!IsConnected) return;

            IsConnected = false;
            PingEnabled = false;
            StateChanged?.Invoke();
            ConnectionLost?.Invoke();
        }

        private void LogHostAndControllerIp() {
            string localIP = AppConstants.TextIpNotFound;

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
                localIP = AppConstants.TextErrorPrefix + ex.Message;
            }

            Log(AppConstants.LogNetHostIp, localIP);
            Log(AppConstants.LogNetControllerIp, lc.IP);
        }

        private void Log(string format, params object[] args) {
            LogMessage?.Invoke(format, args);
        }
    }
}