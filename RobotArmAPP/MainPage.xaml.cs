using RobotArmAPP.Models;
using System;
using System.Threading;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace RobotArmAPP
{
    public sealed partial class MainPage : Page
    {
        private DispatcherTimer timer;
        public static ListView MenuBlocker { get; set; }

        VerificarConexao verificarConexao = new VerificarConexao();

        public MainPage()
        {
            this.InitializeComponent();
            MenuBlocker = LeftMenu;
            timer = new DispatcherTimer();
            this.Loaded += MainPage_Loaded;
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
            verificarConexao.FirstScan();
            Thread.Sleep(250);
            MudarTexto();

            timer.Interval = TimeSpan.FromSeconds(1.0); //começa o timer
            timer.Tick += Timer_Tick;
            timer.Start();
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

        public void MudarTexto() //esse metodo pega informações da classe VerificarConexão e muda o Status Global do Wifi
        {
            if (verificarConexao.Status == 1)
            {
                TXT_StatusGlobal.Text = "Connected";
                TXT_StatusGlobal.Foreground = new SolidColorBrush(Windows.UI.Colors.Green);
            }
            else
            {
                TXT_StatusGlobal.Text = "Disconnected";
                TXT_StatusGlobal.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
            }
            TXT_StatusGlobal.Visibility = Visibility.Visible;
        }

        private void Timer_Tick(object sender, object e) //Metodo do Timer para atualizar o Status do Wifi
        {
            verificarConexao.WifiStatus();
            //Thread.Sleep(10);
            MudarTexto();
        }
    }
}
