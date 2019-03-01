using RobotArmAPP.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace RobotArmAPP.Views
{
    public sealed partial class Controle : Page
    {
        #region VARIABLES
        private bool okToSend = true;
        private bool playing = false; //usado para funções de controle programaveis( IsPlaying(),PlayAtTop,PlayFromSelected ... )
        private bool changingControls = false;
        private bool liveBoxStatus = true; //Váriavel de leitura, mudar ela não altera a liveBoxStatus
        private int ConexaoStatus = 2; //1 = conectado  e  2 = desconectado
        private int repeatTimes = 0; // numero de repetiçoes das sequencias, essa variavel serve pra armazenar o valor original
        private int currentFrame = 0; //usado para funções de controle programaveis( IsPlaying(),PlayAtTop,PlayFromSelected ... )
        private int[] currentFrameArray; //usado para funções de controle programaveis( IsPlaying(),PlayAtTop,PlayFromSelected ... )
        #endregion

        #region OBJECTS
        private List<int[]> framesList = new List<int[]>(); //Lista é um objeto que armazena variáveis
        #endregion

        #region INSTANCE FIELDS
        private DispatcherTimer WifiCheckerTimer;
        private DispatcherTimer playbackTimer;

        WiFiAPConnection wiFiAPConnection = new WiFiAPConnection();
        HTTPRequests httpRequests = new HTTPRequests();
        JsonSaverAndLoader jsonSaverAndLoader = new JsonSaverAndLoader();
        ConvertToString convertToString = new ConvertToString();
        #endregion

        #region INITIALIZATION
        public Controle()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            wiFiAPConnection.RequestWifiAcess();
            await jsonSaverAndLoader.JsonAutoLoader(framesList, FramesListView, RepeatTimesBox);
            try
            {
                await httpRequests.ReadyToSend(200);
            }
            catch { }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            playbackTimer = new DispatcherTimer();
            playbackTimer.Tick += PlaybackTimer_Tick;

            WifiCheckerTimer = new DispatcherTimer();
            WifiCheckerTimer.Tick += WifiCheckerTimer_Tick;
            WifiCheckerTimer.Interval = TimeSpan.FromMilliseconds(250.0);
            WifiCheckerTimer.Start();
        }
        #endregion

        #region SLIDERS & SLIDERS BOXES
        private async void Eixo1Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            await SendSlidersValues(liveBoxStatus, okToSend, playing, 1);
        }

        private void Eixo1SliderBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            CheckBoxOnlyNumber(e);
        }

        private void Eixo1SliderBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            BoxFocusOut(e);
        }

        private async void Eixo1SliderBox_LostFocus(object sender, RoutedEventArgs e)
        {
            await WhenSliderBoxLostFocus(liveBoxStatus, okToSend, 1);
        }


        private async void Eixo2Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            await SendSlidersValues(liveBoxStatus, okToSend, playing, 2);
        }

        private void Eixo2SliderBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            CheckBoxOnlyNumber(e);
        }

        private void Eixo2SliderBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            BoxFocusOut(e);
        }

        private async void Eixo2SliderBox_LostFocus(object sender, RoutedEventArgs e)
        {
            await WhenSliderBoxLostFocus(liveBoxStatus, okToSend, 2);
        }


        private async void Eixo3Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            await SendSlidersValues(liveBoxStatus, okToSend, playing, 3);
        }

        private void Eixo3SliderBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            CheckBoxOnlyNumber(e);
        }

        private void Eixo3SliderBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            BoxFocusOut(e);
        }

        private async void Eixo3SliderBox_LostFocus(object sender, RoutedEventArgs e)
        {
            await WhenSliderBoxLostFocus(liveBoxStatus, okToSend, 3);
        }


        private async void Eixo4Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            await SendSlidersValues(liveBoxStatus, okToSend, playing, 4);
        }

        private void Eixo4SliderBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            CheckBoxOnlyNumber(e);
        }

        private void Eixo4SliderBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            BoxFocusOut(e);
        }

        private async void Eixo4SliderBox_LostFocus(object sender, RoutedEventArgs e)
        {
            await WhenSliderBoxLostFocus(liveBoxStatus, okToSend, 4);
        }


        private async void GarraSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            await SendSlidersValues(liveBoxStatus, okToSend, playing, 5);
        }

        private void GarraSliderBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            CheckBoxOnlyNumber(e);
        }

        private void GarraSliderBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            BoxFocusOut(e);
        }

        private async void GarraSliderBox_LostFocus(object sender, RoutedEventArgs e)
        {
            await WhenSliderBoxLostFocus(liveBoxStatus, okToSend, 5);
        }
        #endregion

        #region SPEED/REPETITIONS/DELAY
        private void FrameSpeedBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            CheckBoxOnlyNumber(e);
        }

        private void FrameSpeedBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            BoxFocusOut(e);
        }

        private async void FrameSpeedBox_LostFocus(object sender, RoutedEventArgs e)
        {
            await BoxesMaxNumberLimiter(1);
        }


        private void DelayBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            CheckBoxOnlyNumber(e);
        }

        private void DelayBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            BoxFocusOut(e);
        }

        private async void DelayBox_LostFocus(object sender, RoutedEventArgs e)
        {
            await BoxesMaxNumberLimiter(2);
        }

        private async void MinDelay_Click(object sender, RoutedEventArgs e)//CALCULADO APENAS PARA O MG995
        {
            SwitchMinimizeDelay(1);
            await jsonSaverAndLoader.JsonAutoSaver(framesList, RepeatTimesBox.Text);
        }

        private async void MinDelayAll_Click(object sender, RoutedEventArgs e)
        {
            changingControls = true;
            LockControls(true, true);
            Canvas.SetZIndex(StopPlayback, 1);
            Canvas.SetZIndex(Blocker2, 2);

            SwitchMinimizeDelay(0);

            changingControls = false;
            LockControls(false, false);

            await jsonSaverAndLoader.JsonAutoSaver(framesList, RepeatTimesBox.Text);
        }


        private void RepeatTimesBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            CheckBoxOnlyNumber(e);
        }

        private void RepeatTimesBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            BoxFocusOut(e);
        }

        private async void RepeatTimesBox_LostFocus(object sender, RoutedEventArgs e)
        {
            await BoxesMaxNumberLimiter(3);
            await jsonSaverAndLoader.JsonAutoSaver(framesList, RepeatTimesBox.Text);
        }
        #endregion

        #region CONTROLS
        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            await SendAxisValuesToEsp();
            await jsonSaverAndLoader.JsonAutoSaver(framesList, RepeatTimesBox.Text);
        }

        private async void ResetControlsButton_Click(object sender, RoutedEventArgs e)
        {
            ResetControls();
            await jsonSaverAndLoader.JsonAutoDeleter();
        }

        private async void InsertFrame_Click(object sender, RoutedEventArgs e)
        {
            InsertFrameFunction();
            await jsonSaverAndLoader.JsonAutoSaver(framesList, RepeatTimesBox.Text);
        }

        private async void OverwriteFrame_Click(object sender, RoutedEventArgs e)
        {
            OverwriteFrameFunction();
            await jsonSaverAndLoader.JsonAutoSaver(framesList, RepeatTimesBox.Text);
        }

        private async void DeleteFrame_Click(object sender, RoutedEventArgs e)
        {
            if (FramesListView.SelectedItems.Count == 0)
            {
                return;
            }

            DeleteFrameFunction();
            await jsonSaverAndLoader.JsonAutoSaver(framesList, RepeatTimesBox.Text);
        }

        private void FramesListView_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            SendItemsToSlidersWhenDoubleTapped();
        }

        private async void FramesListView_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Delete)
            {
                if (FramesListView.SelectedItems.Count == 0)
                {
                    return;
                }

                DeleteFrameFunction();
                await jsonSaverAndLoader.JsonAutoSaver(framesList, RepeatTimesBox.Text);
            }
        }
        #endregion

        #region PLAYBACK
        private async void PlayTop_Click(object sender, RoutedEventArgs e)
        {
            await jsonSaverAndLoader.JsonAutoSaver(framesList, RepeatTimesBox.Text);
            repeatTimes = Convert.ToInt32(RepeatTimesBox.Text);
            currentFrame = 0;
            IsPlaying(true);
        }

        private async void PlaySelected_Click(object sender, RoutedEventArgs e)
        {
            await jsonSaverAndLoader.JsonAutoSaver(framesList, RepeatTimesBox.Text);
            repeatTimes = Convert.ToInt32(RepeatTimesBox.Text);
            currentFrame = FramesListView.SelectedIndex;
            IsPlaying(true);
        }

        private async void StopPlayback_Click(object sender, RoutedEventArgs e)
        {
            await jsonSaverAndLoader.JsonAutoSaver(framesList, RepeatTimesBox.Text);
            IsPlaying(false);
        }

        private void IsPlaying(bool onOff)
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
        #endregion

        #region TIMERS
        private async void WifiCheckerTimer_Tick(object sender, object e)
        {
            int status = await wiFiAPConnection.WifiStatus(false, false);
            if (status == 1 && playing != true && changingControls != true)
            {
                LockControls(null, false);
            }
            else if (status != 1)
            {
                Canvas.SetZIndex(StopPlayback, 1);
                Canvas.SetZIndex(Blocker2, 2);
            }
            else
            {
                LockControls(null, true);
            }
            QuantidadeItens.Text = Convert.ToString(framesList.Count);
        }  /*ATIVAR*/ //Verifica o Wifi Para bloquear os Controles

        private void PlaybackTimer_Tick(object sender, object e)
        {
            PlaybackFunction();
        } // Timer que envia as sequencias no Play
        #endregion

        #region JSON - SAVE/OPEN
        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            await jsonSaverAndLoader.SaveJsonWithFilePicker(framesList, RepeatTimesBox.Text);
        }

        private async void Open_Click(object sender, RoutedEventArgs e)
        {
            okToSend = false;
            double garraCurrent = GarraSlider.Value;
            double axis4Current = Eixo4Slider.Value;
            double axis3Current = Eixo3Slider.Value;
            double axis2Current = Eixo2Slider.Value;
            double axis1Current = Eixo1Slider.Value;
            string speedCurrent = FrameSpeedBox.Text;
            string delayCurrent = DelayBox.Text;

            await jsonSaverAndLoader.OpenJsonWithFilePicker(framesList, FramesListView, RepeatTimesBox);

            GarraSlider.Value = garraCurrent;
            Eixo4Slider.Value = axis4Current;
            Eixo3Slider.Value = axis3Current;
            Eixo2Slider.Value = axis2Current;
            Eixo1Slider.Value = axis1Current;
            FrameSpeedBox.Text = speedCurrent;
            DelayBox.Text = delayCurrent;
            okToSend = true;
        }
        #endregion

        #region FUNCOES
        public async Task SendSlidersValues(bool liveBoxStatus, bool okToSend, bool playing, int axis)
        {
            try
            {
                if (liveBoxStatus == true && okToSend == true)
                {
                    SwitchAxisBoxToSlider(axis);
                    await SendAxisValuesToEsp();
                }
                else
                {
                    SwitchAxisBoxToSlider(axis);
                }

                if (playing == true)
                {
                    await SendAxisValuesToEsp();
                }
            }
            catch { }
        }

        public async Task SendAxisValuesToEsp()
        {
            await httpRequests.GetRequest(Convert.ToString(Eixo1Slider.Value), Convert.ToString(Eixo2Slider.Value), Convert.ToString(Eixo3Slider.Value), Convert.ToString(Eixo4Slider.Value), Convert.ToString(GarraSlider.Value));
        }

        private void SwitchAxisBoxToSlider(int axis)
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



        public void CheckBoxOnlyNumber(KeyRoutedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(e.Key.ToString(), "[0-9]"))
                e.Handled = false;
            else e.Handled = true;
        }

        public void BoxFocusOut(KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                this.Focus(FocusState.Programmatic);
            }
        }

        public void LockControls(bool? waitArrow, bool locked)
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

        public void ResetControls()
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



        private async Task WhenSliderBoxLostFocus(bool liveBoxStatus, bool okToSend, int axis)
        {
            if (liveBoxStatus == true && okToSend == true)
            {
                SwitchAxisSliderToBox(axis);
                await httpRequests.GetRequest(Convert.ToString(Eixo1Slider.Value), Convert.ToString(Eixo2Slider.Value), Convert.ToString(Eixo3Slider.Value), Convert.ToString(Eixo4Slider.Value), Convert.ToString(GarraSlider.Value));
            }
            else
            {
                SwitchAxisSliderToBox(axis);
            }

            try
            {
                VerifySliderBoxValue(axis);
            }
            catch { }
        }

        private void SwitchAxisSliderToBox(int axis)
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

        private void VerifySliderBoxValue(int axis)
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



        private async Task BoxesMaxNumberLimiter(int control)
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

        private void SwitchMinimizeDelay(byte type)
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

        public void MinimizeDelayCalculus(int selected)
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





        public void InsertFrameFunction()
        {
            try
            {
                int frameSpeed = Convert.ToInt16(FrameSpeedBox.Text);
                int frameDelay = Convert.ToInt32(DelayBox.Text);
                string add = convertToString.ConvertItemToString((int)GarraSlider.Value, (int)Eixo4Slider.Value, (int)Eixo3Slider.Value, (int)Eixo2Slider.Value, (int)Eixo1Slider.Value, frameSpeed, frameDelay);

                if (FramesListView.SelectedItems.Count > 0)
                {
                    framesList.Insert(FramesListView.SelectedIndex + 1, new int[] { (int)GarraSlider.Value, (int)Eixo4Slider.Value, (int)Eixo3Slider.Value, (int)Eixo2Slider.Value, (int)Eixo1Slider.Value, frameSpeed, frameDelay });
                    FramesListView.Items.Insert(FramesListView.SelectedIndex + 1, add);
                    FramesListView.SelectedIndex = FramesListView.SelectedIndex + 1;
                }
                else
                {
                    framesList.Add(new int[] { (int)GarraSlider.Value, (int)Eixo4Slider.Value, (int)Eixo3Slider.Value, (int)Eixo2Slider.Value, (int)Eixo1Slider.Value, frameSpeed, frameDelay });
                    FramesListView.Items.Add(add);
                    FramesListView.SelectedIndex = framesList.Count() - 1;
                }
            }
            catch { }
        }

        public void OverwriteFrameFunction()
        {
            try
            {
                int selectedIndex = FramesListView.SelectedIndex;
                framesList[selectedIndex] = new int[] { (int)GarraSlider.Value, (int)Eixo4Slider.Value, (int)Eixo3Slider.Value, (int)Eixo2Slider.Value, (int)Eixo1Slider.Value, Convert.ToInt16(FrameSpeedBox.Text), Convert.ToInt32(DelayBox.Text) };
                FramesListView.Items.Insert(selectedIndex, convertToString.ConvertItemToString((int)GarraSlider.Value, (int)Eixo4Slider.Value, (int)Eixo3Slider.Value, (int)Eixo2Slider.Value, (int)Eixo1Slider.Value, Convert.ToInt16(FrameSpeedBox.Text), Convert.ToInt32(DelayBox.Text)));
                FramesListView.Items.RemoveAt(selectedIndex + 1);
                FramesListView.SelectedIndex = selectedIndex;
            }
            catch { }
        }

        public void DeleteFrameFunction()
        {
            int selected = FramesListView.SelectedIndex;
            framesList.RemoveAt(selected);
            FramesListView.Items.RemoveAt(selected);

            try
            {
                FramesListView.SelectedIndex = selected;
            }
            catch
            {
                try
                {
                    FramesListView.SelectedIndex = selected - 1;
                }
                catch { }
            }
        }

        public void SendItemsToSlidersWhenDoubleTapped()
        {
            try
            {
                int selected = FramesListView.SelectedIndex;
                int[] selectedArray = framesList[selected];
                GarraSlider.Value = selectedArray[0];
                Eixo4Slider.Value = selectedArray[1];
                Eixo3Slider.Value = selectedArray[2];
                Eixo2Slider.Value = selectedArray[3];
                Eixo1Slider.Value = selectedArray[4];
                FrameSpeedBox.Text = Convert.ToString(selectedArray[5]);
                DelayBox.Text = Convert.ToString(selectedArray[6]);
            }
            catch { }
        }


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
        #endregion
    }
}

/*
 *NOTAS: 
 * Chamar um botão: button_Click(this, new RoutedEventArgs());
 * Delays: Thread.Sleep(1000); -> Trava o UI         Thread.Sleep(1000);  -> Não trava o UI, sai e depois volta pra função
 */

