using RobotArmAPP.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        Blocker blocker = new Blocker();
        HTTPRequests httpRequests = new HTTPRequests();
        //Movement movement = new Movement(90, 90, 90, 90, 90, 100, 1000, 0);

        public void FramePlayback(DispatcherTimer playbackTimer,
                                  Rectangle Blocker1,
                                  Rectangle Blocker2,
                                  Rectangle Blocker3,
                                  TextBox RepeatTimesBox,
                                  TextBox FrameSpeedBox,
                                  TextBox DelayBox,
                                  ListView FramesListView,
                                  Slider Eixo1Slider,
                                  Slider Eixo2Slider,
                                  Slider Eixo3Slider,
                                  Slider Eixo4Slider,
                                  Slider GarraSlider,
                                  Button StopPlayback)
        {
            try
            {
                FramesListView.SelectedIndex = Controller.currentFramePosition;
                playbackTimer.Interval = TimeSpan.FromMilliseconds(Controller.framesList[Controller.currentFramePosition][6]);
                DelayBox.Text = Convert.ToString(Controller.framesList[Controller.currentFramePosition][6]);
                FrameSpeedBox.Text = Convert.ToString(Controller.framesList[Controller.currentFramePosition][5]);
                GarraSlider.Value = Controller.framesList[Controller.currentFramePosition][0];
                Eixo4Slider.Value = Controller.framesList[Controller.currentFramePosition][1];
                Eixo3Slider.Value = Controller.framesList[Controller.currentFramePosition][2];
                Eixo2Slider.Value = Controller.framesList[Controller.currentFramePosition][3];
                Eixo1Slider.Value = Controller.framesList[Controller.currentFramePosition][4];

                //await await httpRequests.SendMovementToRobot(movement); mudar isPlaying == false nos controles quando ativar

                if (Controller.framesList.Count > Controller.currentFramePosition + 1)
                    Controller.currentFramePosition++;
                else
                {
                    if (Convert.ToInt32(RepeatTimesBox.Text) == 0 && Controller.repeatTimes == 0)
                        Controller.currentFramePosition = 0;
                    else if (Convert.ToInt32(RepeatTimesBox.Text) > 1)
                    {
                        Controller.currentFramePosition = 0;
                        RepeatTimesBox.Text = Convert.ToString(Convert.ToInt32(RepeatTimesBox.Text) - 1);
                    }
                    else
                    {
                        RepeatTimesBox.Text = Convert.ToString(Controller.repeatTimes);
                        SetPlayingStatus(isON: false, playbackTimer, Blocker1, Blocker2, Blocker3, StopPlayback);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("FramePlayback() Exception: " + ex.Message);
            }
        }

        public void SetPlayingStatus(bool isON,
                                     DispatcherTimer playbackTimer,
                                     Rectangle Blocker1,
                                     Rectangle Blocker2,
                                     Rectangle Blocker3,
                                     Button StopPlayback)
        {
            if (Controller.framesList.Count <= 1)
                return;

            if (isON == true)
            {
                playbackTimer.Start();
                blocker.BlockControls(locked: true, Blocker1, Blocker2, Blocker3);
                blocker.SetStopButtonZIndexToBlock(true, Blocker2, StopPlayback);
            }
            else
            {
                blocker.BlockControls(locked: false, Blocker1, Blocker2, Blocker3);
                playbackTimer.Stop();
            }
            Controller.isPlaying = isON;
            MainPage.LeftMenuAccess.IsEnabled = !isON;
        }

        /*~Playback() //Destructor como tentativa de zerar a posição do frame (não funcionou)
        {
            Controller.currentFramePosition = 0;
        }*/
    }
}
