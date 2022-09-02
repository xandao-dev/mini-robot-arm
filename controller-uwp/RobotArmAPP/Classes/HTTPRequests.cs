using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Popups;
using System;
using System.Diagnostics;
using System.Threading;

namespace RobotArmAPP.Classes
{
    class HTTPRequests
    {
        HttpClient client = new HttpClient();
        HttpClient client2 = new HttpClient();

        private readonly string statusCodeOK = "200";
        private readonly int timeoutSeconds = 30;
        private readonly int timeoutMilliseconds = 0;

        public string BaseUrl { get; set; } = "http://10.10.10.10";
        public string PortForSendFrames { get; set; } = ":18";
        public string PortForVerifyConnection { get; set; } = null;

        public async Task SendMovementToRobot(Movement movement)
        {
            try
            {
                var cancellation = new CancellationTokenSource(new TimeSpan(0, 0, 0, timeoutSeconds, timeoutMilliseconds));
                await client.GetAsync(BaseUrl + PortForSendFrames + "/?param1=" + movement.Axis1 + "&param2=" + movement.Axis2 + "&param3=" + movement.Axis3 + "&param4=" + movement.Axis4 + "&param5=" + movement.Garra, cancellation.Token);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine("SendMovementToRobot() Exception: " + ex.Message);
            }
        }

        public async Task<string> ReadyToSend(int readyToSend)
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                string resultado = await client2.GetStringAsync(BaseUrl + PortForVerifyConnection + "/readytosend/?param=" + readyToSend);
                return resultado;
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine("SendMovementToRobot() Exception: " + ex.Message);
                return null;
            }
        }

        public async Task<string> VerifyWifiAPConnection()
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                await client2.GetStringAsync(BaseUrl + PortForVerifyConnection + "/?param=" + statusCodeOK);
                string statusCode = response.StatusCode.ToString();
                return statusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("VerifyAP() Exception: " + ex.Message);
                return null;
            }
        }
    }
}
