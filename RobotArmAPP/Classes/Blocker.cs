using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace RobotArmAPP.Classes
{
    partial class Blocker
    {
        public void BlockControls(bool locked, Rectangle Blocker1, Rectangle Blocker2, Rectangle Blocker3)
        {
            switch (locked)
            {
                case true:
                    //Blocker1.Visibility = Visibility.Visible;
                    //Blocker2.Visibility = Visibility.Visible;
                    //Blocker3.Visibility = Visibility.Visible;
                    break;
                case false:
                    Blocker1.Visibility = Visibility.Collapsed;
                    Blocker2.Visibility = Visibility.Collapsed;
                    Blocker3.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        public void SetStopButtonZIndexToBlock(bool isButtonAhead, Rectangle Blocker2, Button StopPlayback)
        {
            if (isButtonAhead == true)
            {
                Canvas.SetZIndex(StopPlayback, 2);
                Canvas.SetZIndex(Blocker2, 1);
            }
            else
            {
                Canvas.SetZIndex(StopPlayback, 1);
                Canvas.SetZIndex(Blocker2, 2);
            }
        }
    }

    partial class Blocker
    {
        public void LoginControlsHidden(bool isLogged, Button LogoutBTN, PasswordBox passwordBox, TextBlock PasswordTXT)
        {
            if (isLogged == true)
            {
                LogoutBTN.Visibility = Visibility.Visible;
                passwordBox.Visibility = Visibility.Collapsed;
                PasswordTXT.Visibility = Visibility.Collapsed;
            }
            else
            {
                LogoutBTN.Visibility = Visibility.Collapsed;
                passwordBox.Visibility = Visibility.Visible;
                PasswordTXT.Visibility = Visibility.Visible;
            }
        }
    }
}
