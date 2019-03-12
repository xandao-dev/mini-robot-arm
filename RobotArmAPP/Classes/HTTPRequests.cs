using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Popups;
using System;


namespace RobotArmAPP.Classes
{
    class HTTPRequests
    {
        HttpClient cliente = new HttpClient();

        public static string baseUrl { get; set; } = "http://10.10.10.10";
        public static string portForSendFrames { get; set; } = ":18";
        public static string portForVerifyConnection { get; set; } = null;

        public async Task SendMovementToRobot(Movement movement)
        {
            try
            {
                await cliente.GetStringAsync(baseUrl + portForSendFrames + "/?param1=" + movement.axis1 + "&param2=" + movement.axis2 + "&param3=" + movement.axis3 + "&param4=" + movement.axis4 + "&param5=" + movement.garra);
            }
            catch { }
        }

        public async Task<string> ReadyToSend(int readyToSend)
        {
            try
            {
                HttpClient cliente = new HttpClient();
                HttpResponseMessage response = new HttpResponseMessage();//"http://10.10.10.10/readytosend/?param="
                string resultado = await cliente.GetStringAsync(baseUrl + portForVerifyConnection + "/readytosend/?param=" + readyToSend);
                return resultado;
            }
            catch
            {
                return null;
            }
        }
    }
}
