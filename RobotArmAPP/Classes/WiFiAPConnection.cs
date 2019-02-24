using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Devices.WiFi;
using Windows.Networking.Connectivity;
using Windows.Security.Credentials;
using Windows.UI.Popups;

namespace RobotArmAPP
{
    class WiFiAPConnection
    {
        public static string SSID { get; set; } = "robotarm";
        public static string PW { get; set; } = "0xcrossbots";

        public static int status = 0; //0 = notConnected, 1 = isConnected, 2 = isConnecting

        WiFiAdapter wifiAdapter;

        public async void RequestWifiAcess()
        {
            try
            {
                var access = await WiFiAdapter.RequestAccessAsync();
                if (access == WiFiAccessStatus.Allowed)
                {
                    var result = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());
                    if (result.Count >= 1)
                    {
                        wifiAdapter = await WiFiAdapter.FromIdAsync(result[0].Id);
                    }
                }
            }
            catch { }
        }  //Method that needs to be called in the Page_Loaded or OnNavigatedTo function to request access to the wifi controls

        private async Task<string> VerifyAP(int appConnectionOK)
        {
            HttpClient cliente = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            string resultado = await cliente.GetStringAsync("http://10.10.10.10/?param=" + appConnectionOK);
            string statusCode = response.StatusCode.ToString(); // Retorna "OK" quando da certo
            return statusCode;
        } //Checks if ESP32 and computer are exchanging data

        public async Task<int> WifiStatus(ConnectionProfile currentNetwork, bool isConnecting) //Checks if the computer is connected to the ESP32 WiFi and call VerificarAP
        {
            if (isConnecting == false)
            {
                if (currentNetwork != null && currentNetwork.ProfileName == SSID)
                {
                    try
                    {
                        string code = await VerifyAP(200);
                        if (code == "OK")
                        {
                            status = 1;
                        }
                    }
                    catch
                    {
                        status = 0;
                    }
                }
                else
                {
                    status = 0;
                }
                return status;
            }
            else
            {
                status = 2;
                return status;
            }
        }

        public async Task<bool> GetNetworkProfiles(bool needDialog)
        {
            try
            {
                var connectedProfile = await wifiAdapter.NetworkAdapter.GetConnectedProfileAsync();
                await WifiStatus(connectedProfile, false);
                return (connectedProfile != null && connectedProfile.ProfileName == SSID);
            }
            catch
            {
                if (needDialog == true)
                {
                    var dialog = new MessageDialog("Check your WiFi network adapter!", "Error");
                    await dialog.ShowAsync();
                    await WifiStatus(null, false);
                }
                return false;
            }
        } //Search the networks and call the wifistatus to see if it is the right network, if yes it returns true. 

        public async Task ConnectToWifi()
        {
            try
            {
                var result = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());
                if (result.Count >= 1)
                {
                    wifiAdapter = await WiFiAdapter.FromIdAsync(result[0].Id);
                    await wifiAdapter.ScanAsync();// escaneando redes

                    var nw = wifiAdapter.NetworkReport.AvailableNetworks.Where(y => y.Ssid == SSID).FirstOrDefault(); //verificando a rede
                    var credential = new PasswordCredential //senha
                    {
                        Password = PW
                    };

                    WiFiReconnectionKind reconnectionKind = WiFiReconnectionKind.Automatic; //tipo de conexao

                    await wifiAdapter.ConnectAsync(nw, reconnectionKind, credential); //a conexao
                }

            }
            catch
            {
                var dialog = new MessageDialog("Verifique seu adaptador de rede WiFi e o Access Point do Braço Robótico!", "Erro");
                await dialog.ShowAsync();
                MudarTexto(null, 2);
            }
        } //Tries to connect to the esp32 WiFi AP
    }
}