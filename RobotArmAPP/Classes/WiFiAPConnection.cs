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
        public string SSID { get; set; } = "robotarm";
        public string Password { get; set; } = "0xcrossbots";

        public enum Status
        {
            Disconnected,
            Connected,
            Connecting
        }

        WiFiAdapter wifiAdapter;

        public async void RequestWifiAccess()
        {
            try
            {
                var wiFiaccess = await WiFiAdapter.RequestAccessAsync();
                if (wiFiaccess == WiFiAccessStatus.Allowed)
                {
                    var result = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());
                    if (result.Count >= 1)
                    {
                        wifiAdapter = await WiFiAdapter.FromIdAsync(result[0].Id);
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("RequestWifiAccess() Exception: " + ex.Message);
                throw;
            }
        }  //Method that needs to be called in the Page_Loaded or OnNavigatedTo function to request access to the wifi controls

        private async Task<string> VerifyAP(int appConnectionOK)
        {
            try
            {
                HttpClient cliente = new HttpClient();
                HttpResponseMessage response = new HttpResponseMessage();
                await cliente.GetStringAsync("http://10.10.10.10/?param=" + appConnectionOK);
                string statusCode = response.StatusCode.ToString(); // Retorna "OK" quando da certo
                return statusCode;
            }
            catch(Exception ex)
            {
                Debug.WriteLine("VerifyAP() Exception: " + ex.Message);
                return null;
            }
        } //Checks if ESP32 and computer are exchanging data

        public async Task<bool> IsCorrectNetworkConnected(bool verifySSID, bool needDialog)
        {
            try
            {
                var currentNetwork = await wifiAdapter.NetworkAdapter.GetConnectedProfileAsync();
                if (verifySSID == true)
                {
                    return (currentNetwork != null && currentNetwork.ProfileName == SSID);
                }
                else
                {
                    return currentNetwork != null;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("IsCorrectNetworkConnected() Exception: " + ex.Message);
                if (needDialog == true)
                {
                    throw;
                }
                //await WifiStatus(isDisconnected: true, isConnecting: false);
                return false;
            }
        } //Search the networks and call the wifistatus to see if it is the right network, if yes it returns true. 

        public async Task<Status> WifiStatus(bool isDisconnected, bool isConnecting)
        {
                bool rightNetwork = await IsCorrectNetworkConnected(verifySSID: true, needDialog: false);
                if (isConnecting == false)
                {
                    if (rightNetwork == true && isDisconnected == false)
                    {
                        string code = await VerifyAP(200);
                        if (code == "OK")
                        {
                            return Status.Connected;
                        }
                    }
                    else
                    {
                        return Status.Disconnected;
                    }
                }
                else
                {
                    return Status.Connecting;
                }
                return Status.Disconnected;      
        } //Checks if the computer is connected to the ESP32 WiFi and call VerificarAP

        public async Task ConnectToWifi()
        {
            try
            {
                var result = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());
                if (result.Count >= 1)
                {
                    wifiAdapter = await WiFiAdapter.FromIdAsync(result[0].Id);
                    await wifiAdapter.ScanAsync();// escaneando redes
                    var network = wifiAdapter.NetworkReport.AvailableNetworks.Where(y => y.Ssid == SSID).FirstOrDefault(); //verificando a rede
                    var credential = new PasswordCredential //senha
                    {
                        Password = this.Password
                    };
                    WiFiReconnectionKind reconnectionKind = WiFiReconnectionKind.Automatic; //tipo de Connection
                    await wifiAdapter.ConnectAsync(network, reconnectionKind, credential); //a Connection
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("ConnectToWifi() Exception: " + ex.Message);
                throw;
            }
        } //Tries to connect to the esp32 WiFi AP

        public void DisconnectWifi()
        {
            wifiAdapter.Disconnect();
        } //Disconnect the Wifi
    }
}