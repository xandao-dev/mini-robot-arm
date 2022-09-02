using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace RobotArmAPP.Classes
{
    class Controls
    {
        HTTPRequests httpRequests = new HTTPRequests();

        public enum ControlsIndex
        {
            Gripper,
            Axis4,
            Axis3,
            Axis2,
            Axis1,
            FrameSpeedBox,
            DelayBox,
            RepeatTimesBox
        }

        public async Task SendSlidersValues(bool liveBoxStatus, bool isOkToSendMoviments, bool isPlaying, Movement movement)
        {
            try
            {
                if (liveBoxStatus == true && isOkToSendMoviments == true || isPlaying == true)
                    await httpRequests.SendMovementToRobot(movement);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SendSlidersValues() Exception: " + ex.Message);
            }
        }

        public async Task WhenSliderBoxLoseFocus(bool liveBoxStatus, bool isOkToSendMoviments, Movement movement)
        {
            if (liveBoxStatus == true && isOkToSendMoviments == true)
                await httpRequests.SendMovementToRobot(movement);
        }

        public void VerifySliderBoxValue(TextBox SliderBox)
        {
            try
            {
                SliderBox.UpdateLayout();
                if (Convert.ToDouble(SliderBox.Text) >= 180.0)
                    SliderBox.Text = "180";
                else if (Convert.ToDouble(SliderBox.Text) <= 0.0)
                    SliderBox.Text = "0";
            }
            catch (Exception ex)
            {
                Debug.WriteLine("VerifySliderBoxValue() Exception: " + ex.Message);
            }
        }

        public async Task BoxesMaxNumberLimiter(ControlsIndex controlsIndex,
                                                TextBox RepeatTimesBox,
                                                TextBox FrameSpeedBox,
                                                TextBox DelayBox)
        {
            try
            {
                double dRepeats = Convert.ToDouble(RepeatTimesBox.Text);
                double dSpeed = Convert.ToDouble(FrameSpeedBox.Text);
                double dDelay = Convert.ToDouble(DelayBox.Text);
                int speed = Convert.ToInt16(FrameSpeedBox.Text);
                int minimum = 900 * 100 / speed;
                int minimum1degree = 5 * 100 / speed;

                switch (controlsIndex)
                {
                    case ControlsIndex.FrameSpeedBox:
                        FrameSpeedBox.UpdateLayout();
                        if (dSpeed > 100.0)
                            FrameSpeedBox.Text = "100";
                        else if (dSpeed < 0.0)
                            FrameSpeedBox.Text = "0";

                        if (dDelay < minimum)
                            DelayBox.Text = Convert.ToString(minimum);
                        break;
                    case ControlsIndex.DelayBox:
                        DelayBox.UpdateLayout();
                        if (dDelay > 300000.0)
                            DelayBox.Text = "300000";
                        else if (dDelay < 0.0)
                            DelayBox.Text = "0";

                        if (Convert.ToInt64(DelayBox.Text) < minimum)
                        {
                            var dialog = new MessageDialog("Minimum Recommended at " + speed + "% speed for MG995: " + minimum + "ms\nTimes greater than " + minimum + "ms can make movement incomplete.\n" + minimum + "ms is the minimum required for the MG995 to rotate 180º at " + speed + "% speed.\nBut if you want to continue, use at least " + minimum1degree + "ms per degree changed.\n\nFor automatic minimum delay, choose the frame item and click at \"Min Delay\"", "Alert!");
                            await dialog.ShowAsync();
                        }
                        break;
                    case ControlsIndex.RepeatTimesBox:
                        RepeatTimesBox.UpdateLayout();
                        if (dRepeats > 10000.0)
                            RepeatTimesBox.Text = "10000";
                        else if (dRepeats < 0.0)
                            RepeatTimesBox.Text = "0";
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("BoxesMaxNumberLimiter() Exception: " + ex.Message);
            }
        }

        public void CheckBoxOnlyNumber(KeyRoutedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(e.Key.ToString(), "[0-9]"))
                e.Handled = false;
            else e.Handled = true;
        }

        public void BoxFocusOut(KeyRoutedEventArgs e, Page ControllerPage)
        {
            if (e.Key == VirtualKey.Enter)
                ControllerPage.Focus(FocusState.Programmatic);
        }
    }
}
