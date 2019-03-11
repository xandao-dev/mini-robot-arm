using RobotArmAPP.Classes;
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
        WiFiAPConnection.Status status = new WiFiAPConnection.Status();
        UpdateTexts updateTexts = new UpdateTexts();

        public Connection()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            wiFiAPConnection.RequestWifiAccess();
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
                status = await wiFiAPConnection.WifiStatus(false, true);
                updateTexts.StatusTextAndColor(status,TXT_Status);
                await wiFiAPConnection.ConnectToWifi();
            }

            try
            {
                status = await wiFiAPConnection.WifiStatus(false, false);
                bool connected = await wiFiAPConnection.GetNetworkProfiles(true, false);
                await wiFiAPConnection.WifiStatus(connected, true);
                updateTexts.StatusTextAndColor(status, TXT_Status);
            }
            catch
            {
                status = await wiFiAPConnection.WifiStatus(true, false);
                updateTexts.StatusTextAndColor(status, TXT_Status);
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
                    status = await wiFiAPConnection.WifiStatus(true, false);
                    updateTexts.StatusTextAndColor(status, TXT_Status);
                }
                else
                {
                    status = await wiFiAPConnection.WifiStatus(false, false);
                    updateTexts.StatusTextAndColor(status, TXT_Status);
                }
            }
            catch { }
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
            BTN_Conectar.IsEnabled = true;
            BTN_Desconectar.IsEnabled = true;
        } //chama MudarTexto

        private async void WifiCheckerTimer_Tick(object sender, object e) //Metodo do Timer para atualizar o Status do Wifi
        {
            status = await wiFiAPConnection.WifiStatus(false, false);
            updateTexts.StatusTextAndColor(status, TXT_Status);
        }
    }
}
