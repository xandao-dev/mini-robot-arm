using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace RobotArmAPP.Classes
{
    class UpdateTexts
    {
        public void StatusTextAndColor(WiFiAPConnection.Status status, TextBlock statusText)
        {
            if (status == WiFiAPConnection.Status.Disconnected)
            {
                statusText.Text = "Disconnected";
                statusText.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
            }
            else if (status == WiFiAPConnection.Status.Connected)
            {
                statusText.Text = "Connected";
                statusText.Foreground = new SolidColorBrush(Windows.UI.Colors.Green);
            }
            else if (status == WiFiAPConnection.Status.Connecting)
            {
                statusText.Text = "Connecting";
                statusText.Foreground = new SolidColorBrush(Windows.UI.Colors.DarkOrange);
            }
        }
    }
}
