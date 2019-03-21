using RobotArmAPP.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace RobotArmAPP.Classes
{
    class Playback
    {
        private int currentFrame=0;
        Blocker blocker = new Blocker();

        public void FramePlayback(ref int CurrentFrame, DispatcherTimer playbackTimer, Rectangle Blocker1, Rectangle Blocker2, Rectangle Blocker3, TextBox RepeatTimesBox, TextBox FrameSpeedBox, TextBox DelayBox, ListView FramesListView, Slider[] SliderArray, Button StopPlayback)
        {
            currentFrame = CurrentFrame;
            FramesListView.SelectedIndex = currentFrame;
            Controller.currentFrameArray = Controller.framesList[CurrentFrame];
            SliderArray[(int)Controls.ControlsIndex.Gripper].Value = Controller.currentFrameArray[(int)Controls.ControlsIndex.Gripper];
            SliderArray[(int)Controls.ControlsIndex.Axis4].Value = Controller.currentFrameArray[(int)Controls.ControlsIndex.Axis4];
            SliderArray[(int)Controls.ControlsIndex.Axis3].Value = Controller.currentFrameArray[(int)Controls.ControlsIndex.Axis3];
            SliderArray[(int)Controls.ControlsIndex.Axis2].Value = Controller.currentFrameArray[(int)Controls.ControlsIndex.Axis2];
            SliderArray[(int)Controls.ControlsIndex.Axis1].Value = Controller.currentFrameArray[(int)Controls.ControlsIndex.Axis1];
            FrameSpeedBox.Text = Convert.ToString(Controller.currentFrameArray[(int)Controls.ControlsIndex.FrameSpeedBox]);
            DelayBox.Text = Convert.ToString(Controller.currentFrameArray[(int)Controls.ControlsIndex.DelayBox]);
            playbackTimer.Interval = TimeSpan.FromMilliseconds(Controller.currentFrameArray[(int)Controls.ControlsIndex.DelayBox]);

            if (Controller.framesList.Count > currentFrame + 1)
            {
                currentFrame++;
            }
            else
            {
                if (Convert.ToInt32(RepeatTimesBox.Text) == 0 && Controller.repeatTimes == 0)
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
                    RepeatTimesBox.Text = Convert.ToString(Controller.repeatTimes);
                    SetPlayingStatus(onOff: false ,playbackTimer, Blocker1, Blocker2, Blocker3, StopPlayback);
                }
            }     
        }

        public void SetPlayingStatus(bool onOff,DispatcherTimer playbackTimer, Rectangle Blocker1, Rectangle Blocker2, Rectangle Blocker3, Button StopPlayback)
        {
            if (Controller.framesList.Count <= 1)
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

            if (onOff == true)
            {
                blocker.BlockControls(Blocker.MouseCursor.Null, locked: true, Blocker1, Blocker2, Blocker3);
                blocker.SetStopButtonZIndex(true, Blocker2, StopPlayback);
            }
            else
            {
                blocker.BlockControls(Blocker.MouseCursor.Null, locked: false, Blocker1, Blocker2, Blocker3);
            }

            Controller.playing = onOff;
            Controller.okToSend = !onOff;
            MainPage.LeftMenuAccess.IsEnabled = !onOff;
        }
    }
}
