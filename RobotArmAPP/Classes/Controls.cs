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

        public async Task SendSlidersValues(bool liveBoxStatus, bool okToSend, bool playing, int axis, TextBox Eixo1SliderBox, TextBox Eixo2SliderBox, TextBox Eixo3SliderBox, TextBox Eixo4SliderBox, TextBox GarraSliderBox, Slider Eixo1Slider, Slider Eixo2Slider, Slider Eixo3Slider, Slider Eixo4Slider, Slider GarraSlider)
        {
            try
            {
                if (liveBoxStatus == true && okToSend == true)
                {
                    SwitchAxisBoxToSlider(axis,Eixo1SliderBox,Eixo2SliderBox,Eixo2SliderBox,Eixo4SliderBox,GarraSliderBox,Eixo1Slider,Eixo2Slider,Eixo3Slider,Eixo4Slider,GarraSlider);
                    await httpRequests.GetRequest(Convert.ToString(Eixo1Slider.Value), Convert.ToString(Eixo2Slider.Value), Convert.ToString(Eixo3Slider.Value), Convert.ToString(Eixo4Slider.Value), Convert.ToString(GarraSlider.Value));

                }
                else
                {
                    SwitchAxisBoxToSlider(axis, Eixo1SliderBox, Eixo2SliderBox, Eixo2SliderBox, Eixo4SliderBox, GarraSliderBox, Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider);
                }

                if (playing == true)
                {
                    await httpRequests.GetRequest(Convert.ToString(Eixo1Slider.Value), Convert.ToString(Eixo2Slider.Value), Convert.ToString(Eixo3Slider.Value), Convert.ToString(Eixo4Slider.Value), Convert.ToString(GarraSlider.Value));

                }
            }
            catch { }
        }

        private void SwitchAxisSliderToBox(int axis, TextBox Eixo1SliderBox, TextBox Eixo2SliderBox, TextBox Eixo3SliderBox, TextBox Eixo4SliderBox, TextBox GarraSliderBox, Slider Eixo1Slider, Slider Eixo2Slider, Slider Eixo3Slider, Slider Eixo4Slider, Slider GarraSlider)
        {
            switch (axis)
            {
                case 1:
                    Eixo1Slider.Value = Convert.ToDouble(Eixo1SliderBox.Text);
                    break;
                case 2:
                    Eixo2Slider.Value = Convert.ToDouble(Eixo2SliderBox.Text);
                    break;
                case 3:
                    Eixo3Slider.Value = Convert.ToDouble(Eixo3SliderBox.Text);
                    break;
                case 4:
                    Eixo4Slider.Value = Convert.ToDouble(Eixo4SliderBox.Text);
                    break;
                case 5:
                    GarraSlider.Value = Convert.ToDouble(GarraSliderBox.Text);
                    break;
            }
        }
    }

    partial class Controls
    {   //Boxes

        public async Task WhenSliderBoxLostFocus(bool liveBoxStatus, bool okToSend, int axis, TextBox Eixo1SliderBox, TextBox Eixo2SliderBox, TextBox Eixo3SliderBox, TextBox Eixo4SliderBox, TextBox GarraSliderBox, Slider Eixo1Slider, Slider Eixo2Slider, Slider Eixo3Slider, Slider Eixo4Slider, Slider GarraSlider)
        {
            if (liveBoxStatus == true && okToSend == true)
            {
                SwitchAxisSliderToBox(axis, Eixo1SliderBox, Eixo2SliderBox, Eixo2SliderBox, Eixo4SliderBox, GarraSliderBox, Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider);
                await httpRequests.GetRequest(Convert.ToString(Eixo1Slider.Value), Convert.ToString(Eixo2Slider.Value), Convert.ToString(Eixo3Slider.Value), Convert.ToString(Eixo4Slider.Value), Convert.ToString(GarraSlider.Value));
            }
            else
            {
                SwitchAxisSliderToBox(axis, Eixo1SliderBox, Eixo2SliderBox, Eixo2SliderBox, Eixo4SliderBox, GarraSliderBox, Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider);
            }

            try
            {
                VerifySliderBoxValue(axis);
            }
            catch { }
        }

        private void SwitchAxisBoxToSlider(int axis, TextBox Eixo1SliderBox, TextBox Eixo2SliderBox, TextBox Eixo3SliderBox, TextBox Eixo4SliderBox, TextBox GarraSliderBox, Slider Eixo1Slider, Slider Eixo2Slider, Slider Eixo3Slider, Slider Eixo4Slider, Slider GarraSlider)
        {
            switch (axis)
            {
                case 1:
                    Eixo1SliderBox.Text = Eixo1Slider.Value.ToString();
                    break;
                case 2:
                    Eixo2SliderBox.Text = Eixo2Slider.Value.ToString();
                    break;
                case 3:
                    Eixo3SliderBox.Text = Eixo3Slider.Value.ToString();
                    break;
                case 4:
                    Eixo4SliderBox.Text = Eixo4Slider.Value.ToString();
                    break;
                case 5:
                    GarraSliderBox.Text = GarraSlider.Value.ToString();
                    break;
            }
        }

        private void VerifySliderBoxValue(int axis, TextBox Eixo1SliderBox, TextBox Eixo2SliderBox, TextBox Eixo3SliderBox, TextBox Eixo4SliderBox, TextBox GarraSliderBox, Slider Eixo1Slider, Slider Eixo2Slider, Slider Eixo3Slider, Slider Eixo4Slider, Slider GarraSlider)
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

        public async Task BoxesMaxNumberLimiter(int control,TextBox RepeatTimesBox,TextBox FrameSpeedBox,TextBox DelayBox)
        {
            /*
             *   1:FrameSpeedBox
             *   2:DelayBox
             *   3:RepeatTimesBox
             */
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
                    case 1:
                        /*Speed Box*/
                        FrameSpeedBox.UpdateLayout();
                        if (dSpeed > 100.0)
                            FrameSpeedBox.Text = "100";
                        else if (dSpeed < 0.0)
                            FrameSpeedBox.Text = "0";

                        if (dDelay < minimum)
                            DelayBox.Text = Convert.ToString(minimum);
                        break;
                    case 2:
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
                    case 3:
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

        public void ResetControls(Slider Eixo1Slider, Slider Eixo2Slider, Slider Eixo3Slider, Slider Eixo4Slider, Slider GarraSlider, TextBox RepeatTimesBox, TextBox FrameSpeedBox, TextBox DelayBox, ListView FramesListView, List<int[]> framesList)
        {
            Eixo1Slider.Value = 90;
            Eixo2Slider.Value = 90;
            Eixo3Slider.Value = 90;
            Eixo4Slider.Value = 90;
            GarraSlider.Value = 90;
            FrameSpeedBox.Text = "100";
            DelayBox.Text = "1000";
            RepeatTimesBox.Text = "0";
            FramesListView.Items.Clear();
            framesList.Clear();
        }
    }

    partial class Controls
    {   //Minimize Delay

        ConvertToString convertToString = new ConvertToString();

        public void SwitchMinimizeDelay(byte type, ListView FramesListView, List<int[]> framesList)
        {
            /*
             * 0: Min Delay All Items
             * 1: Min Delay One Item
             */
            if (framesList.Count <= 0)
            {
                return;
            }

            switch (type)
            {
                case 0:
                    for (int selected = 0; selected < FramesListView.Items.Count; selected++)
                    {
                        MinimizeDelayCalculus(selected);
                    }
                    FramesListView.SelectedIndex = FramesListView.Items.Count - 1;
                    break;
                case 1:
                    int index = FramesListView.SelectedIndex;
                    MinimizeDelayCalculus(index);
                    FramesListView.SelectedIndex = index;
                    break;
            }
        }

        private void MinimizeDelayCalculus(int selected,TextBox FrameSpeedBox ,ListView FramesListView, List<int[]> framesList)
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
                    FramesListView.Items.Insert(selected, convertToString.SelectedItemToString(framesList, index, minimum));
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
                    FramesListView.Items.Insert(selected, convertToString.SelectedItemToString(framesList, index, delayMin));
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
                FramesListView.Items.Insert(selected, convertToString.SelectedItemToString(framesList, index, delayMin));
                FramesListView.Items.RemoveAt(selected + 1);
                FramesListView.SelectedIndex = selected;
            }
        }
    }
}
