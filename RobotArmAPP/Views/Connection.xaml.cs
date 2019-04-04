using RobotArmAPP.Classes;
using System;
using System.Diagnostics;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace RobotArmAPP.Views
{

    public sealed partial class Connection : Page
    {
        #region OBJECTS
        private DispatcherTimer WifiCheckerTimer;
        WiFiAPConnection wiFiAPConnection = new WiFiAPConnection();
        WiFiAPConnection.Status status = new WiFiAPConnection.Status();
        UpdateTexts updateTexts = new UpdateTexts();
        #endregion

        #region INITIALIZATION
        public Connection()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                await wiFiAPConnection.RequestWifiAccess();
                await wiFiAPConnection.GetWifiAdaptors();
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.Message);
                await dialog.ShowAsync();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            WifiCheckerTimer = new DispatcherTimer();
            WifiCheckerTimer.Tick += WifiCheckerTimer_Tick;
            WifiCheckerTimer.Interval = TimeSpan.FromMilliseconds(250);
            WifiCheckerTimer.Start();
        }
        #endregion

        #region CONTROLS
        private async void BTN_Conectar_Click(object sender, RoutedEventArgs e)
        {

            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Wait, 1);
            WifiCheckerTimer.Stop();
            BTN_Conectar.IsEnabled = false;
            BTN_Desconectar.IsEnabled = false;

            try
            {
                bool isCorrectNetwork = await wiFiAPConnection.IsCorrectNetworkConnected(needVerifySSID: true, needErrorDialog: true);
                if (isCorrectNetwork != true)
                {
                    status = await wiFiAPConnection.WifiConnectionStatus(isDisconnected: false, isConnecting: true);
                    updateTexts.StatusTextAndColor(status, TXT_Status);
                    try
                    {
                        await wiFiAPConnection.ConnectToWifi();
                    }
                    catch
                    {
                        var dialog = new MessageDialog("Check your WiFi network adapter and the Robotic Arm Access Point!", "Error");
                        await dialog.ShowAsync();
                    }
                }

                status = await wiFiAPConnection.WifiConnectionStatus(isDisconnected: false, isConnecting: false);
                bool isConnected = await wiFiAPConnection.IsCorrectNetworkConnected(needVerifySSID: true, needErrorDialog: false);
                await wiFiAPConnection.WifiConnectionStatus(isDisconnected: !isConnected, isConnecting: true);
                updateTexts.StatusTextAndColor(status, TXT_Status);

                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
                WifiCheckerTimer.Start();
                BTN_Conectar.IsEnabled = true;
                BTN_Desconectar.IsEnabled = true;
            }
            catch
            {
                status = await wiFiAPConnection.WifiConnectionStatus(isDisconnected: true, isConnecting: false);
                updateTexts.StatusTextAndColor(status, TXT_Status);
                var dialog = new MessageDialog("Check your WiFi network adapter!", "Error");
                await dialog.ShowAsync();
            }

        }

        private async void BTN_Desconectar_Click(object sender, RoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Wait, 1);
            BTN_Conectar.IsEnabled = false;
            BTN_Desconectar.IsEnabled = false;
            bool networkNotNull = await wiFiAPConnection.IsCorrectNetworkConnected(needVerifySSID: false, needErrorDialog: false);
            if (networkNotNull == true)
            {
                wiFiAPConnection.DisconnectWifi();
                status = await wiFiAPConnection.WifiConnectionStatus(isDisconnected: true, isConnecting: false);
                updateTexts.StatusTextAndColor(status, TXT_Status);
            }
            else
            {
                status = await wiFiAPConnection.WifiConnectionStatus(isDisconnected: false, isConnecting: false);
                updateTexts.StatusTextAndColor(status, TXT_Status);
            }
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
            BTN_Conectar.IsEnabled = true;
            BTN_Desconectar.IsEnabled = true;
        }
        #endregion

        #region TIMERS
        private async void WifiCheckerTimer_Tick(object sender, object e)
        {
            status = await wiFiAPConnection.WifiConnectionStatus(isDisconnected: false, isConnecting: false);
            updateTexts.StatusTextAndColor(status, TXT_Status);
        }
        #endregion
    }
}
