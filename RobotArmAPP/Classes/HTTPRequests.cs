using System.Net.Http;
using System.Threading.Tasks;

namespace RobotArmAPP.Classes
{
    class HTTPRequests
    {
        public async Task<string> GetRequest(string eixo1Valor, string eixo2Valor, string eixo3Valor, string eixo4Valor, string garraValor)
        {
            HttpClient cliente = new HttpClient();
            string resultado = await cliente.GetStringAsync("http://10.10.10.10:18/?param1=" + eixo1Valor + "&param2=" + eixo2Valor + "&param3=" + eixo3Valor + "&param4=" + eixo4Valor + "&param5=" + garraValor);
            return resultado;
        }

        public async Task<string> GetRequestPlayer(string eixo1Valor, string eixo2Valor, string eixo3Valor, string eixo4Valor, string garraValor, string speed)
        {
            HttpClient cliente = new HttpClient();
            string resultado = await cliente.GetStringAsync("http://10.10.10.10:18/?param1=" + eixo1Valor + "&param2=" + eixo2Valor + "&param3=" + eixo3Valor + "&param4=" + eixo4Valor + "&param5=" + garraValor + "&param6=" + speed);
            return resultado;
        }

        public async Task<string> ReadyToSend(int readyToSend)
        {
            HttpClient cliente = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            string resultado = await cliente.GetStringAsync("http://10.10.10.10/readytosend/?param=" + readyToSend);
            return resultado;
        }
    }
}
