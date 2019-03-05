using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotArmAPP.Classes
{
    class Playback
    {
        public void PlaybackFunction()
        {
            FramesListView.SelectedIndex = currentFrame;
            currentFrameArray = framesList[currentFrame];
            GarraSlider.Value = currentFrameArray[0];
            Eixo4Slider.Value = currentFrameArray[1];
            Eixo3Slider.Value = currentFrameArray[2];
            Eixo2Slider.Value = currentFrameArray[3];
            Eixo1Slider.Value = currentFrameArray[4];
            FrameSpeedBox.Text = Convert.ToString(currentFrameArray[5]);
            DelayBox.Text = Convert.ToString(currentFrameArray[6]);
            playbackTimer.Interval = TimeSpan.FromMilliseconds(currentFrameArray[6]);

            if (framesList.Count > currentFrame + 1)
            {
                currentFrame++;
            }
            else
            {
                if (Convert.ToInt32(RepeatTimesBox.Text) == 0 && repeatTimes == 0)
                {
                    currentFrame = 0;
                }
                else if (Convert.ToInt32(RepeatTimesBox.Text) > 1)
                {
                    currentFrame = 0;
                    RepeatTimesBox.Text = Convert.ToString(Convert.ToInt32(RepeatTimesBox.Text) - 1);
                }
                else
                {
                    RepeatTimesBox.Text = Convert.ToString(repeatTimes);
                    IsPlaying(false);
                }
            }
        }

        public void IsPlaying(bool onOff)
        {
            if (framesList.Count <= 0)
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

            playing = onOff;
            onOff = !onOff;
            okToSend = onOff;
            MainPage.MenuBlocker.IsEnabled = onOff;
        }
    }
}
