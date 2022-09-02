using RobotArmAPP.Classes;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Devices.WiFi;
using Windows.Security.Credentials;
using Windows.UI.Popups;

namespace RobotArmAPP
{
    class WiFiAPConnection
    {
        WiFiAdapter wifiAdapter;
        HTTPRequests httpRequests = new HTTPRequests();
        private WiFiAccessStatus wiFiaccess;
        private AdaptersStatus adapterStatus = new AdaptersStatus();

        public string SSID { get; set; } = "robotarm";
        public string Password { get; set; } = "0xcrossbots";

        public enum Status
        {
            Disconnected,
            Connected,
            Connecting
        }
        public enum AdaptersStatus
        {
            noAdapters,
            hasAdapters
        }

        public async Task RequestWifiAccess()
        {
            try
            {
                wiFiaccess = await WiFiAdapter.RequestAccessAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("RequestWifiAccess() Exception: " + ex.Message);
                throw;
            }
        }

        public async Task<AdaptersStatus> GetWifiAdaptors()
        {
            try
            {
                if (wiFiaccess == WiFiAccessStatus.Allowed)
                {
                    var wifiAdaptersColletion = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());
                    if (wifiAdaptersColletion.Count >= 1)
                    {
                        wifiAdapter = await WiFiAdapter.FromIdAsync(wifiAdaptersColletion[0].Id);
                        return AdaptersStatus.hasAdapters;
                    }
                }
                return AdaptersStatus.noAdapters;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("RequestWifiAccess() Exception: " + ex.Message);
                //return AdaptersStatus.hasAdapters;
                throw;
            }
        }

        public async Task<bool> IsCorrectNetworkConnected(bool needVerifySSID, bool needErrorDialog)
        {
            try
            {
                var currentNetwork = await wifiAdapter.NetworkAdapter.GetConnectedProfileAsync();
                if (needVerifySSID == true)
                    return (currentNetwork != null && currentNetwork.ProfileName == SSID);
                else
                    return currentNetwork != null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("IsCorrectNetworkConnected() Exception: " + ex.Message);
                if (needErrorDialog == true)
                    throw;
                return false;
            }
        }

        public async Task<Status> WifiConnectionStatus(bool isDisconnected, bool isConnecting)
        {
            bool rightNetwork = await IsCorrectNetworkConnected(needVerifySSID: true, needErrorDialog: false);
            if (rightNetwork == true)
            {
                if (isDisconnected == true)
                    return Status.Disconnected;
                else if (isConnecting)
                    return Status.Connecting;
                else
                {
                    string code = await httpRequests.VerifyWifiAPConnection();
                    if (code == "OK")
                        return Status.Connected;
                }
            }
            return Status.Disconnected;
        }

        public async Task ConnectToWifi()
        {
            try
            {
                if ((adapterStatus = await GetWifiAdaptors()) == AdaptersStatus.hasAdapters)
                {
                    await wifiAdapter.ScanAsync();
                    var network = wifiAdapter.NetworkReport.AvailableNetworks.Where(y => y.Ssid == SSID).FirstOrDefault();
                    var credential = new PasswordCredential
                    {
                        Password = this.Password
                    };
                    WiFiReconnectionKind reconnectionKind = WiFiReconnectionKind.Automatic;
                    await wifiAdapter.ConnectAsync(network, reconnectionKind, credential);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ConnectToWifi() Exception: " + ex.Message);
                throw;
            }
        }

        public void DisconnectWifi()
        {
            wifiAdapter.Disconnect();
        }
    }
}