using System.Net.Http;
using System.Threading.Tasks;

namespace RobotArmAPP.Classes
{
    class HTTPRequests
    {
        public async Task<string> GetRequest(string eixo1Valor, string eixo2Valor, string eixo3Valor, string eixo4Valor, string garraValor)
        {
            try
            {
                HttpClient cliente = new HttpClient();
                string resultado = await cliente.GetStringAsync("http://10.10.10.10:18/?param1=" + eixo1Valor + "&param2=" + eixo2Valor + "&param3=" + eixo3Valor + "&param4=" + eixo4Valor + "&param5=" + garraValor);
                return resultado;
            }
            catch
            {
                return null;
            }
        }

        public async Task<string> GetRequestPlayer(string eixo1Valor, string eixo2Valor, string eixo3Valor, string eixo4Valor, string garraValor, string speed)
        {
            try
            {
                HttpClient cliente = new HttpClient();
                string resultado = await cliente.GetStringAsync("http://10.10.10.10:18/?param1=" + eixo1Valor + "&param2=" + eixo2Valor + "&param3=" + eixo3Valor + "&param4=" + eixo4Valor + "&param5=" + garraValor + "&param6=" + speed);
                return resultado;
            }
            catch
            {
                return null;
            }
        }

        public async Task<string> ReadyToSend(int readyToSend)
        {
            try
            {
                HttpClient cliente = new HttpClient();
                HttpResponseMessage response = new HttpResponseMessage();
                string resultado = await cliente.GetStringAsync("http://10.10.10.10/readytosend/?param=" + readyToSend);
                return resultado;
            }
            catch
            {
                return null;
            }
        }
    }
}
