using System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace RobotArmAPP.Views
{

    public sealed partial class Conexao : Page
    {
        WiFiAPConnection wiFiAPConnection = new WiFiAPConnection();

        public Conexao()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            wiFiAPConnection.RequestWifiAcess();
            //await wiFiAPConnection.GetNetworkProfiles(true);
        }

        /*private async Task<string> VerificarAP(int conexaoAplicativoOK)
        {
            HttpClient cliente = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            string resultado = await cliente.GetStringAsync("http://10.10.10.10/?param=" + conexaoAplicativoOK);
            string statusCode = response.StatusCode.ToString(); // Retorna "OK" quando da certo
            return statusCode;
        }*/

        /*private async Task<bool> ConectadoRedeCerta()
        {
            try
            {
                var connectedProfile = await wifiAdapter.NetworkAdapter.GetConnectedProfileAsync();
                return (connectedProfile != null && connectedProfile.ProfileName == SSID);
            }
            catch
            {
                var dialog = new MessageDialog("Check your WiFi network adapter!", "Error");
                await dialog.ShowAsync();
                MudarTexto(null, 2);
                return false;
            }
        }*/

        /*private async Task<int> ConectarWifi()
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
                        Password = Senha
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
            return 1;
        }*/

        /*public async void MudarTexto(ConnectionProfile redeAtual, byte tipo)
        {
             
             //* 0: conectado ou desconectado
             //* 1: conectando      -> parametro 1 null
             //* 2 ou outro: desconectado   -> parametro 1 null

            TXT_Status.Visibility = Visibility.Visible;
            if (tipo == 0)
            {
                if (redeAtual != null && redeAtual.ProfileName == SSID)
                {
                    try
                    {
                        string code = await VerificarAP(200);
                        if (code == "OK")
                        {
                            TXT_Status.Text = "Connected";
                            TXT_Status.Foreground = new SolidColorBrush(Windows.UI.Colors.Green);
                        }
                    }
                    catch
                    {
                        TXT_Status.Text = "Disconnected";
                        TXT_Status.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
                    }
                }
                else
                {
                    TXT_Status.Text = "Disconnected";
                    TXT_Status.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
                }
            }
            else if (tipo == 1)
            {
                TXT_Status.Text = "Connecting";
                TXT_Status.Foreground = new SolidColorBrush(Windows.UI.Colors.DarkOrange);
            }
            else
            {
                TXT_Status.Text = "Disconnected";
                TXT_Status.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
            }
        }*/

        private async void BTN_Conectar_Click(object sender, RoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Wait, 1);
            BTN_Conectar.IsEnabled = false;
            BTN_Desconectar.IsEnabled = false;

            if (await wiFiAPConnection.GetNetworkProfiles(true) == false)
            {
                //wifiAdapter.Disconnect();
                //MudarTexto(null, 1);
                Thread.Sleep(250);
                await wiFiAPConnection.ConnectToWifi();
            }

            try
            {
                //var connectedProfile = await wifiAdapter.NetworkAdapter.GetConnectedProfileAsync();
                //MudarTexto(connectedProfile, 0);
            }
            catch
            {
                //MudarTexto(null, 2);
            }

            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
            BTN_Conectar.IsEnabled = true;
            BTN_Desconectar.IsEnabled = true;
        }

        private async void BTN_Desconectar_Click(object sender, RoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Wait, 1);
            BTN_Conectar.IsEnabled = false;
            BTN_Desconectar.IsEnabled = false;
            try
            {
                /*var connectedProfile = await wifiAdapter.NetworkAdapter.GetConnectedProfileAsync();//verifica se tem wifi conectado e qual
                if (connectedProfile != null)
                {
                    wifiAdapter.Disconnect(); //desconecta o wifi
                    MudarTexto(null, 2); //altera o status
                }
                else
                {
                    MudarTexto(connectedProfile, 0); //altera o status
                }*/
            }
            catch
            {
                //MudarTexto(null, 2);
            }
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
            BTN_Conectar.IsEnabled = true;
            BTN_Desconectar.IsEnabled = true;
        }
    }
}
