using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace RobotArmAPP.Views
{

    public sealed partial class Connection : Page
    {
        private DispatcherTimer WifiCheckerTimer;
        WiFiAPConnection wiFiAPConnection = new WiFiAPConnection();

        public Connection()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            wiFiAPConnection.RequestWifiAcess();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            WifiCheckerTimer = new DispatcherTimer();
            WifiCheckerTimer.Tick += WifiCheckerTimer_Tick;
            WifiCheckerTimer.Interval = TimeSpan.FromMilliseconds(250);
            WifiCheckerTimer.Start();
        }

        private async void BTN_Conectar_Click(object sender, RoutedEventArgs e)
        {

            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Wait, 1);
            WifiCheckerTimer.Stop();
            BTN_Conectar.IsEnabled = false;
            BTN_Desconectar.IsEnabled = false;

            if (await wiFiAPConnection.GetNetworkProfiles(true, true) == false)
            {
                int status = await wiFiAPConnection.WifiStatus(false, true);
                TextAndColor(status);
                await wiFiAPConnection.ConnectToWifi();
            }

            try
            {
                int status = await wiFiAPConnection.WifiStatus(false, false);
                bool connected = await wiFiAPConnection.GetNetworkProfiles(true, false);
                await wiFiAPConnection.WifiStatus(connected, true);
                TextAndColor(status);
            }
            catch
            {
                int status = await wiFiAPConnection.WifiStatus(true, false);
                TextAndColor(status);
            }

            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
            WifiCheckerTimer.Start();
            BTN_Conectar.IsEnabled = true;
            BTN_Desconectar.IsEnabled = true;
        } //chama ConectarWifi, MudarTexto

        private async void BTN_Desconectar_Click(object sender, RoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Wait, 1);
            BTN_Conectar.IsEnabled = false;
            BTN_Desconectar.IsEnabled = false;
            try
            {
                bool networkNotNull = await wiFiAPConnection.GetNetworkProfiles(false, false);
                if (networkNotNull == true)
                {
                    wiFiAPConnection.DisconnectWifi();
                    int status = await wiFiAPConnection.WifiStatus(true, false);
                    TextAndColor(status);
                }
                else
                {
                    int status = await wiFiAPConnection.WifiStatus(false, false);
                    TextAndColor(status);
                }
            }
            catch { }
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
            BTN_Conectar.IsEnabled = true;
            BTN_Desconectar.IsEnabled = true;
        } //chama MudarTexto

        private void TextAndColor(int status)
        {
            if (status == 0)
            {
                TXT_Status.Text = "Disconnected";
                TXT_Status.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
            }
            else if (status == 1)
            {
                TXT_Status.Text = "Connected";
                TXT_Status.Foreground = new SolidColorBrush(Windows.UI.Colors.Green);
            }
            else if (status == 2)
            {
                TXT_Status.Text = "Connecting";
                TXT_Status.Foreground = new SolidColorBrush(Windows.UI.Colors.DarkOrange);
            }
        }

        private async void WifiCheckerTimer_Tick(object sender, object e) //Metodo do Timer para atualizar o Status do Wifi
        {
            int status = await wiFiAPConnection.WifiStatus(false, false);
            TextAndColor(status);
        }
    }
}
