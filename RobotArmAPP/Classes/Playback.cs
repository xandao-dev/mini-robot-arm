using RobotArmAPP.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace RobotArmAPP.Classes
{
    class Playback
    {
        public void FramePlayback(DispatcherTimer playbackTimer, Rectangle Blocker1, Rectangle Blocker2, Rectangle Blocker3, TextBox RepeatTimesBox, TextBox FrameSpeedBox, TextBox DelayBox, ListView FramesListView, Slider Eixo1Slider, Slider Eixo2Slider, Slider Eixo3Slider, Slider Eixo4Slider, Slider GarraSlider, Button StopPlayback)
        {
            FramesListView.SelectedIndex = Controller.currentFrame;
            Controller.currentFrameArray = Controller.framesList[Controller.currentFrame];
            GarraSlider.Value = Controller.currentFrameArray[0];
            Eixo4Slider.Value = Controller.currentFrameArray[1];
            Eixo3Slider.Value = Controller.currentFrameArray[2];
            Eixo2Slider.Value = Controller.currentFrameArray[3];
            Eixo1Slider.Value = Controller.currentFrameArray[4];
            FrameSpeedBox.Text = Convert.ToString(Controller.currentFrameArray[5]);
            DelayBox.Text = Convert.ToString(Controller.currentFrameArray[6]);
            playbackTimer.Interval = TimeSpan.FromMilliseconds(Controller.currentFrameArray[6]);

            if (Controller.framesList.Count > Controller.currentFrame + 1)
            {
                Controller.currentFrame++;
            }
            else
            {
                if (Convert.ToInt32(RepeatTimesBox.Text) == 0 && Controller.repeatTimes == 0)
                {
                    Controller.currentFrame = 0;
                }
                else if (Convert.ToInt32(RepeatTimesBox.Text) > 1)
                {
                    Controller.currentFrame = 0;
                    RepeatTimesBox.Text = Convert.ToString(Convert.ToInt32(RepeatTimesBox.Text) - 1);
                }
                else
                {
                    RepeatTimesBox.Text = Convert.ToString(Controller.repeatTimes);
                    SetPlayingStatus(false, playbackTimer, Blocker1, Blocker2, Blocker3, StopPlayback);
                }
            }
        }

        public void SetPlayingStatus(bool onOff, DispatcherTimer playbackTimer, Rectangle Blocker1, Rectangle Blocker2, Rectangle Blocker3, Button StopPlayback)
        {
            if (Controller.framesList.Count <= 0)
            {
                return;
            }

            if (onOff == true)
            {
                playbackTimer.Start();
            }
            else
            {
                playbackTimer.Stop();
            }

            if (playbackTimer.IsEnabled)
            {
                playbackTimer.Interval = TimeSpan.FromMilliseconds(20.0);
            }

            if (onOff == true)
            {
                Blocker1.Visibility = Visibility.Visible;
                Blocker2.Visibility = Visibility.Visible;
                Blocker3.Visibility = Visibility.Visible;
                Canvas.SetZIndex(StopPlayback, 2);
                Canvas.SetZIndex(Blocker2, 1);
            }
            else
            {
                Blocker1.Visibility = Visibility.Collapsed;
                Blocker2.Visibility = Visibility.Collapsed;
                Blocker3.Visibility = Visibility.Collapsed;
            }

            Controller.playing = onOff;
            onOff = !onOff;
            Controller.okToSend = onOff;
            MainPage.MenuBlocker.IsEnabled = onOff;
        }
    }
}
