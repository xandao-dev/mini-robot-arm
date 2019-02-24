using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace RobotArmAPP.Views
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>

    public sealed partial class Home : Page
    {
        private static bool access = false;
        public static string HomePassword { get; set; } = "crossbots";

        public Home()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (access == false)
            {
                PasswordTXT.Visibility = Visibility.Visible;
                MainPage.MenuBlocker.IsEnabled = false;
            }
            else
            {
                PasswordTXT.Visibility = Visibility.Collapsed;
                MainPage.MenuBlocker.IsEnabled = true;
                LogoutBTN.Visibility = Visibility.Visible;
                passwordBox.Visibility = Visibility.Collapsed;
            }
        }

        private void PasswordBox_Loaded(object sender, RoutedEventArgs e)
        {
            passwordBox.Focus(FocusState.Programmatic);
        }

        private void PasswordBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                if (passwordBox.Password == HomePassword)
                {
                    MainPage.MenuBlocker.IsEnabled = true;
                    access = true;
                    LogoutBTN.Visibility = Visibility.Visible;
                    passwordBox.Visibility = Visibility.Collapsed;
                    PasswordTXT.Visibility = Visibility.Collapsed;
                    passwordBox.Password = "";
                }
            }
        }

        private void LogoutBTN_Click(object sender, RoutedEventArgs e)
        {
            MainPage.MenuBlocker.IsEnabled = false;
            access = false;
            LogoutBTN.Visibility = Visibility.Collapsed;
            passwordBox.Visibility = Visibility.Visible;
            PasswordTXT.Visibility = Visibility.Visible;
            passwordBox.PlaceholderText = "Enter password";
        }
    }
}
