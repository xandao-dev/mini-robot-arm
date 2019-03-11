using RobotArmAPP.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;

namespace RobotArmAPP.Classes
{
    partial class Controls
    {   //Sliders
        HTTPRequests httpRequests = new HTTPRequests();

        public async Task SendSlidersValues(bool liveBoxStatus, bool okToSend, bool playing, Movement movement)
        {
            try
            {
                if (liveBoxStatus == true && okToSend == true || playing == true)
                {
                    await httpRequests.SendMovementToRobot(movement);
                }
            }
            catch { }
        }
    }

    partial class Controls
    {   //Boxes
        public enum Control
        {
            FrameSpeedBox,
            DelayBox,
            RepeatTimesBox
        }

        public async Task WhenSliderBoxLoseFocus(bool liveBoxStatus, bool okToSend, Movement movement)
        {
            if (liveBoxStatus == true && okToSend == true)
            {
                await httpRequests.SendMovementToRobot(movement);
            }
        }

        public void VerifySliderBoxValue(int axis, TextBox Eixo1SliderBox, TextBox Eixo2SliderBox, TextBox Eixo3SliderBox, TextBox Eixo4SliderBox, TextBox GarraSliderBox, Slider Eixo1Slider, Slider Eixo2Slider, Slider Eixo3Slider, Slider Eixo4Slider, Slider GarraSlider)
        {
            try
            {
                switch (axis)
                {
                    case 1:
                        Eixo1SliderBox.UpdateLayout();
                        if (Convert.ToDouble(Eixo1SliderBox.Text) >= 180.0)
                            Eixo1SliderBox.Text = "180";
                        else if (Convert.ToDouble(Eixo1SliderBox.Text) <= 0.0)
                            Eixo1SliderBox.Text = "0";
                        break;
                    case 2:
                        Eixo2SliderBox.UpdateLayout();
                        if (Convert.ToDouble(Eixo2SliderBox.Text) >= 180.0)
                            Eixo2SliderBox.Text = "180";
                        else if (Convert.ToDouble(Eixo2SliderBox.Text) <= 0.0)
                            Eixo2SliderBox.Text = "0";
                        break;
                    case 3:
                        Eixo3SliderBox.UpdateLayout();
                        if (Convert.ToDouble(Eixo3SliderBox.Text) >= 180.0)
                            Eixo3SliderBox.Text = "180";
                        else if (Convert.ToDouble(Eixo3SliderBox.Text) <= 0.0)
                            Eixo3SliderBox.Text = "0";
                        break;
                    case 4:
                        Eixo4SliderBox.UpdateLayout();
                        if (Convert.ToDouble(Eixo4SliderBox.Text) >= 180.0)
                            Eixo4SliderBox.Text = "180";
                        else if (Convert.ToDouble(Eixo4SliderBox.Text) <= 0.0)
                            Eixo4SliderBox.Text = "0";
                        break;
                    case 5:
                        GarraSliderBox.UpdateLayout();
                        if (Convert.ToDouble(GarraSliderBox.Text) >= 180.0)
                            GarraSliderBox.Text = "180";
                        else if (Convert.ToDouble(GarraSliderBox.Text) <= 0.0)
                            GarraSliderBox.Text = "0";
                        break;
                }
            }
            catch { }
        }

        public async Task BoxesMaxNumberLimiter(Control control,TextBox RepeatTimesBox,TextBox FrameSpeedBox,TextBox DelayBox)
        {
            try
            {
                double dRepeats = Convert.ToDouble(RepeatTimesBox.Text);
                double dSpeed = Convert.ToDouble(FrameSpeedBox.Text);
                double dDelay = Convert.ToDouble(DelayBox.Text);
                int speed = Convert.ToInt16(FrameSpeedBox.Text);
                int minimum = 900 * 100 / speed;
                int minimum1degree = 5 * 100 / speed;

                switch (control)
                {
                    case Control.FrameSpeedBox:
                        /*Speed Box*/
                        FrameSpeedBox.UpdateLayout();
                        if (dSpeed > 100.0)
                            FrameSpeedBox.Text = "100";
                        else if (dSpeed < 0.0)
                            FrameSpeedBox.Text = "0";

                        if (dDelay < minimum)
                            DelayBox.Text = Convert.ToString(minimum);
                        break;
                    case Control.DelayBox:
                        /*Delay Box*/
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
                    case Control.RepeatTimesBox:
                        /*Repeat Times Box*/
                        RepeatTimesBox.UpdateLayout();
                        if (dRepeats > 10000.0)
                            RepeatTimesBox.Text = "10000";
                        else if (dRepeats < 0.0)
                            RepeatTimesBox.Text = "0";
                        break;
                }
            }
            catch { }
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
            {
                ControllerPage.Focus(FocusState.Programmatic);
            }
        }
    }

    partial class Controls
    {   //Block and Reset

        public void LockControls(bool? waitArrow, bool locked, Rectangle Blocker1, Rectangle Blocker2, Rectangle Blocker3)
        {
            switch (waitArrow)
            {
                case true:
                    Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Wait, 1);
                    break;
                case false:
                    Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
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
            if(isButtonAhead == true)
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

        public void ResetControls(Slider Eixo1Slider, Slider Eixo2Slider, Slider Eixo3Slider, Slider Eixo4Slider, Slider GarraSlider, TextBox RepeatTimesBox, TextBox FrameSpeedBox, TextBox DelayBox, ListView FramesListView, List<int[]> framesList, Movement defaultMovement)
        {
            Eixo1Slider.Value = defaultMovement.axis1; 
            Eixo2Slider.Value = defaultMovement.axis2;
            Eixo3Slider.Value = defaultMovement.axis3;
            Eixo4Slider.Value = defaultMovement.axis4;
            GarraSlider.Value = defaultMovement.garra;
            FrameSpeedBox.Text = Convert.ToString(defaultMovement.speed); 
            DelayBox.Text = Convert.ToString(defaultMovement.delay); 
            RepeatTimesBox.Text = Convert.ToString(defaultMovement.repeatTimes);
            FramesListView.Items.Clear();
            framesList.Clear();
        }
    }

    partial class Controls
    {   //Minimize Delay
        public enum Delay
        {
            all,
            one
        }

        public string SelectedFrameToString(int index, int delay, Movement movement)
        {
            int[] selectedArray = Controller.framesList[index];
            movement.garra = selectedArray[0];
            movement.axis4 = selectedArray[1];
            movement.axis3 = selectedArray[2];
            movement.axis2 = selectedArray[3];
            movement.axis1 = selectedArray[4];
            movement.speed = selectedArray[5];
            movement.delay = delay;
            return movement.MovesToString(Movement.StringType.allWithInfo);
        }

        public void SwitchMinimizeDelay(Delay delay, TextBox FrameSpeedBox, ListView FramesListView, List<int[]> framesList, Movement movement)
        {
            if (framesList.Count <= 0)
            {
                return;
            }

            switch (delay)
            {
                case Delay.all:
                    for (int selected = 0; selected < FramesListView.Items.Count; selected++)
                    {
                        MinimizeDelayCalculus(selected,FrameSpeedBox,FramesListView,framesList,movement);
                    }
                    FramesListView.SelectedIndex = FramesListView.Items.Count - 1;
                    break;
                case Delay.one:
                    int index = FramesListView.SelectedIndex;
                    MinimizeDelayCalculus(index,FrameSpeedBox,FramesListView,framesList,movement);
                    FramesListView.SelectedIndex = index;
                    break;
            }
        }

        private void MinimizeDelayCalculus(int selected,TextBox FrameSpeedBox ,ListView FramesListView, List<int[]> framesList, Movement movement)
        {
            if (framesList.Count <= 0)
            {
                return;
            }

            FramesListView.SelectedIndex = selected;
            int index = FramesListView.SelectedIndex;

            if (selected == 0)
            {

                int[] selectedArray = framesList[selected];
                int[] lastArray = framesList[FramesListView.Items.Count - 1];
                int speed = selectedArray[5];

                if (selectedArray == lastArray)
                {
                    int minimum = 900 * 100 / speed;

                    framesList[selected] = new int[] { selectedArray[0], selectedArray[1], selectedArray[2], selectedArray[3], selectedArray[4], selectedArray[5], minimum };
                    FramesListView.Items.Insert(selected, SelectedFrameToString(index, minimum, movement));
                    FramesListView.Items.RemoveAt(selected + 1);
                    FramesListView.SelectedIndex = selected;
                }
                else
                {
                    int eixo1difference = Math.Abs(selectedArray[0] - lastArray[0]);
                    int eixo2difference = Math.Abs(selectedArray[1] - lastArray[1]);
                    int eixo3difference = Math.Abs(selectedArray[2] - lastArray[2]);
                    int eixo4difference = Math.Abs(selectedArray[3] - lastArray[3]);
                    int garradifference = Math.Abs(selectedArray[4] - lastArray[4]);

                    int biggest = Math.Max(Math.Max(Math.Max(eixo1difference, eixo2difference), eixo3difference), Math.Max(eixo4difference, garradifference));
                    int delayMin = (biggest * 5) * 100 / speed; //Valor calculado manualmente, 5ms por grau no MG995

                    framesList[selected] = new int[] { selectedArray[0], selectedArray[1], selectedArray[2], selectedArray[3], selectedArray[4], selectedArray[5], delayMin };
                    FramesListView.Items.Insert(selected, SelectedFrameToString(index, delayMin, movement));
                    FramesListView.Items.RemoveAt(selected + 1);
                    FramesListView.SelectedIndex = selected;
                }
            }
            else
            {
                int[] selectedArray = framesList[selected];
                int[] previousArray = framesList[selected - 1];

                int eixo1difference = Math.Abs(selectedArray[0] - previousArray[0]);
                int eixo2difference = Math.Abs(selectedArray[1] - previousArray[1]);
                int eixo3difference = Math.Abs(selectedArray[2] - previousArray[2]);
                int eixo4difference = Math.Abs(selectedArray[3] - previousArray[3]);
                int garradifference = Math.Abs(selectedArray[4] - previousArray[4]);

                int speed = Convert.ToInt16(FrameSpeedBox.Text);

                int biggest = Math.Max(Math.Max(Math.Max(eixo1difference, eixo2difference), eixo3difference), Math.Max(eixo4difference, garradifference));
                int delayMin = (biggest * 5) * 100 / speed; //Valor calculado manualmente, 5ms por grau no MG995

                framesList[selected] = new int[] { selectedArray[0], selectedArray[1], selectedArray[2], selectedArray[3], selectedArray[4], selectedArray[5], delayMin };
                FramesListView.Items.Insert(selected, SelectedFrameToString(index, delayMin,movement));
                FramesListView.Items.RemoveAt(selected + 1);
                FramesListView.SelectedIndex = selected;
            }
        }
    }
}
