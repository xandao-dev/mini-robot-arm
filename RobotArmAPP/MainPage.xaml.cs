using RobotArmAPP.Classes;
using RobotArmAPP.Models;
using System;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;


namespace RobotArmAPP
{
    public sealed partial class MainPage : Page
    {
        private DispatcherTimer WifiCheckerTimer;
        public static ListView LeftMenuAccess { get; set; }

        WiFiAPConnection wiFiAPConnection = new WiFiAPConnection();
        WiFiAPConnection.Status status = new WiFiAPConnection.Status();
        UpdateTexts updateTexts = new UpdateTexts();

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
            LeftMenuAccess = LeftMenu;
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e) //Define o tamanho minimo do aplicativo -> Page_SizeChanged está definido no xaml
        {

            if (e.NewSize.Width < 1000 || e.NewSize.Height < 600)
            {
                ApplicationView.GetForCurrentView().TryResizeView(new Size(Math.Max(1000, e.NewSize.Width), Math.Max(600, e.NewSize.Height)));
            }
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            MenuGrid_Tapped(null, null);

            try
            {
                wiFiAPConnection.RequestWifiAccess();
            }
            catch(Exception ex)
            {
                var dialog = new MessageDialog(ex.ToString());
                await dialog.ShowAsync();
            }

            WifiCheckerTimer = new DispatcherTimer();
            WifiCheckerTimer.Tick += WifiCheckerTimer_Tick;
            WifiCheckerTimer.Interval = TimeSpan.FromMilliseconds(500.0);
            WifiCheckerTimer.Start();
        }

        private async void MenuGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (NavigationPane.IsPaneOpen)
                NavigationPane.IsPaneOpen = !NavigationPane.IsPaneOpen;

            if (LeftMenu.SelectedItem is MenuItem menu)
            {
                if (menu.NavigateTo != null)
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { FrameContent.Navigate(menu.NavigateTo); });
            }
        }

        private async void WifiCheckerTimer_Tick(object sender, object e) //Metodo do Timer para atualizar o Status do Wifi
        {
            status = await wiFiAPConnection.WifiStatus(false, false);
            updateTexts.StatusTextAndColor(status,TXT_StatusGlobal);
        }
    }
}
