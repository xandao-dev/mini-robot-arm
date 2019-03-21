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
    class Blocker
    {
        public enum MouseCursor
        {
            Wait,
            Arrow,
            Null
        }

        public void BlockControls(MouseCursor mouseCursor, bool locked, Rectangle Blocker1, Rectangle Blocker2, Rectangle Blocker3)
        {
            switch (mouseCursor)
            {
                case MouseCursor.Wait:
                    Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Wait, 1);
                    break;
                case MouseCursor.Arrow:
                    Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
                    break;
                default:
                    break;
            }

            switch (locked)
            {
                case true:
                    Blocker1.Visibility = Visibility.Visible;
                    Blocker2.Visibility = Visibility.Visible;
                    Blocker3.Visibility = Visibility.Visible;
                    break;
                case false:
                    Blocker1.Visibility = Visibility.Collapsed;
                    Blocker2.Visibility = Visibility.Collapsed;
                    Blocker3.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        public void SetStopButtonZIndex(bool isButtonAhead, Rectangle Blocker2, Button StopPlayback)
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
}
