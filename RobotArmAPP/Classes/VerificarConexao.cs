using System;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Devices.WiFi;
using Windows.Networking.Connectivity;

namespace RobotArmAPP
{
    class VerificarConexao
    {
        private const string SSID = "robotarm";

        public byte Status = 2;

        WiFiAdapter wifiAdapter;

        private async Task<string> VerificarAP(int conexaoAplicativoOK)
        {
            HttpClient cliente = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            string resultado = await cliente.GetStringAsync("http://10.10.10.10/?param=" + conexaoAplicativoOK);
            string statusCode = response.StatusCode.ToString(); // Retorna "OK" quando da certo
            return statusCode;
        }

        public async void MudarTexto(ConnectionProfile redeAtual) //Essa função verifica se o WiFi está conectado e verifica se o servidor está recebendo dados
        {
            /******************************
              1: Significa Conectado
              2: Significa Desconectado
             *****************************/

            if (redeAtual != null && redeAtual.ProfileName == SSID)
            {
                try
                {
                    string code = await VerificarAP(200);
                    if (code == "OK")
                    {
                        Status = 1;
                    }
                }
                catch
                {
                    Status = 2;
                }
            }
            else
            {
                Status = 2;
            }
        }

        public async void WifiStatus()
        {
            try
            {
                var connectedProfile = await wifiAdapter.NetworkAdapter.GetConnectedProfileAsync();
                MudarTexto(connectedProfile);
            }
            catch { }
        }

        public int CallStatus()
        {
            return Status;
        }

        public async void FirstScan()
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

                    WifiStatus();
                }
            }
            catch { }
        }

    }
}