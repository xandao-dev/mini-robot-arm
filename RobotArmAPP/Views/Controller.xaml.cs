using RobotArmAPP.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public sealed partial class Controller : Page
    {
        #region VARIABLES
        public const int numberOfAxis = 5;

        public static bool isOkToSendMoviments = true;
        public static bool isPlaying = false;
        public static bool isChangingControls = false;
        public bool liveBoxStatus = true;

        public static int repeatTimes = 0;
        public static int currentFramePosition = 0;
        #endregion

        #region OBJECTS
        public Slider[] SliderArray { get; set; }
        public static List<int[]> framesList = new List<int[]>();

        private DispatcherTimer WifiCheckerTimer;
        private DispatcherTimer playbackTimer;

        WiFiAPConnection wiFiAPConnection = new WiFiAPConnection();
        WiFiAPConnection.Status status = new WiFiAPConnection.Status();
        HTTPRequests httpRequests = new HTTPRequests();
        JsonSaverAndLoader jsonSaverAndLoader = new JsonSaverAndLoader();
        Controls controls = new Controls();
        Blocker blocker = new Blocker();
        Reset reset = new Reset();
        DelayControl delayControl = new DelayControl();
        Frames frames = new Frames();
        Playback playback = new Playback();
        Movement movement = new Movement(90, 90, 90, 90, 90, 100, 1000, 0);
        Movement defaultMovement = new Movement(90, 90, 90, 90, 90, 100, 1000, 0);
        #endregion

        #region INITIALIZATION
        public Controller()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                await wiFiAPConnection.RequestWifiAccess();
                await wiFiAPConnection.GetWifiAdaptors();
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.Message);
                await dialog.ShowAsync();
            }

            await jsonSaverAndLoader.JsonAutoLoader(FramesListView, RepeatTimesBox, movement, defaultMovement);
            AssignMovementValues();
            await httpRequests.ReadyToSend(200);

            if (FramesListView.Items.Count > 0)
                FramesListView.SelectedIndex = 0;
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

        #region CONTROLS

        #region SLIDERS & SLIDERS BOXES
        private async void Eixo1Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            AssignMovementValues();
            await controls.SendSlidersValues(liveBoxStatus, isOkToSendMoviments, isPlaying, movement);
        }

        private void Eixo1SliderBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            controls.CheckBoxOnlyNumber(e);
        }

        private void Eixo1SliderBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            controls.BoxFocusOut(e, ControllerPage);
        }

        private async void Eixo1SliderBox_LostFocus(object sender, RoutedEventArgs e)
        {
            AssignMovementValues();
            controls.VerifySliderBoxValue(Eixo1SliderBox,
                                          Eixo2SliderBox,
                                          Eixo3SliderBox,
                                          Eixo4SliderBox,
                                          GarraSliderBox,
                                          axis: 1);
            await controls.WhenSliderBoxLoseFocus(liveBoxStatus, isOkToSendMoviments, movement);
        }


        private async void Eixo2Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            AssignMovementValues();
            await controls.SendSlidersValues(liveBoxStatus, isOkToSendMoviments, isPlaying, movement);
        }

        private void Eixo2SliderBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            controls.CheckBoxOnlyNumber(e);
        }

        private void Eixo2SliderBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            controls.BoxFocusOut(e, ControllerPage);
        }

        private async void Eixo2SliderBox_LostFocus(object sender, RoutedEventArgs e)
        {
            AssignMovementValues();
            controls.VerifySliderBoxValue(Eixo1SliderBox,
                                          Eixo2SliderBox,
                                          Eixo3SliderBox,
                                          Eixo4SliderBox,
                                          GarraSliderBox,
                                          axis: 2);
            await controls.WhenSliderBoxLoseFocus(liveBoxStatus, isOkToSendMoviments, movement);
        }


        private async void Eixo3Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            AssignMovementValues();
            await controls.SendSlidersValues(liveBoxStatus, isOkToSendMoviments, isPlaying, movement);
        }

        private void Eixo3SliderBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            controls.CheckBoxOnlyNumber(e);
        }

        private void Eixo3SliderBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            controls.BoxFocusOut(e, ControllerPage);
        }

        private async void Eixo3SliderBox_LostFocus(object sender, RoutedEventArgs e)
        {
            AssignMovementValues();
            controls.VerifySliderBoxValue(Eixo1SliderBox,
                                          Eixo2SliderBox,
                                          Eixo3SliderBox,
                                          Eixo4SliderBox,
                                          GarraSliderBox,
                                          axis: 3);
            await controls.WhenSliderBoxLoseFocus(liveBoxStatus, isOkToSendMoviments, movement);
        }


        private async void Eixo4Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            AssignMovementValues();
            await controls.SendSlidersValues(liveBoxStatus, isOkToSendMoviments, isPlaying, movement);
        }

        private void Eixo4SliderBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            controls.CheckBoxOnlyNumber(e);
        }

        private void Eixo4SliderBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            controls.BoxFocusOut(e, ControllerPage);
        }

        private async void Eixo4SliderBox_LostFocus(object sender, RoutedEventArgs e)
        {
            AssignMovementValues();
            controls.VerifySliderBoxValue(Eixo1SliderBox,
                                          Eixo2SliderBox,
                                          Eixo3SliderBox,
                                          Eixo4SliderBox,
                                          GarraSliderBox,
                                          axis: 4);
            await controls.WhenSliderBoxLoseFocus(liveBoxStatus, isOkToSendMoviments, movement);
        }


        private async void GarraSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            AssignMovementValues();
            await controls.SendSlidersValues(liveBoxStatus, isOkToSendMoviments, isPlaying, movement);
        }

        private void GarraSliderBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            controls.CheckBoxOnlyNumber(e);
        }

        private void GarraSliderBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            controls.BoxFocusOut(e, ControllerPage);
        }

        private async void GarraSliderBox_LostFocus(object sender, RoutedEventArgs e)
        {
            AssignMovementValues();
            controls.VerifySliderBoxValue(Eixo1SliderBox,
                                          Eixo2SliderBox,
                                          Eixo3SliderBox,
                                          Eixo4SliderBox,
                                          GarraSliderBox,
                                          gripper: 1);
            await controls.WhenSliderBoxLoseFocus(liveBoxStatus, isOkToSendMoviments, movement);
        }
        #endregion

        #region SPEED/REPETITIONS/DELAY
        private void FrameSpeedBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            controls.CheckBoxOnlyNumber(e);
        }

        private void FrameSpeedBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            controls.BoxFocusOut(e, ControllerPage);
        }

        private async void FrameSpeedBox_LostFocus(object sender, RoutedEventArgs e)
        {
            AssignMovementValues();
            await controls.BoxesMaxNumberLimiter(Controls.ControlsIndex.FrameSpeedBox,
                                                 RepeatTimesBox,
                                                 FrameSpeedBox,
                                                 DelayBox);
        }


        private void DelayBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            controls.CheckBoxOnlyNumber(e);
        }

        private void DelayBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            controls.BoxFocusOut(e, ControllerPage);
        }

        private async void DelayBox_LostFocus(object sender, RoutedEventArgs e)
        {
            AssignMovementValues();
            await controls.BoxesMaxNumberLimiter(Controls.ControlsIndex.DelayBox, RepeatTimesBox, FrameSpeedBox, DelayBox);
        }


        private async void MinDelay_Click(object sender, RoutedEventArgs e)//CALCULADO APENAS PARA O MG995
        {
            delayControl.SwitchMinimizeDelay(DelayControl.Delay.one, FramesListView, framesList, movement);
            AssignMovementValues();
            await jsonSaverAndLoader.JsonAutoSaver(RepeatTimesBox.Text);
        }

        private async void MinDelayAll_Click(object sender, RoutedEventArgs e)
        {
            isChangingControls = true;
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Wait, 1);
            blocker.BlockControls(locked: true, Blocker1, Blocker2, Blocker3);
            blocker.SetStopButtonZIndexToBlock(false, Blocker2, StopPlayback);
            delayControl.SwitchMinimizeDelay(DelayControl.Delay.all, FramesListView, framesList, movement);
            AssignMovementValues();
            isChangingControls = false;
            blocker.BlockControls(locked: false, Blocker1, Blocker2, Blocker3);
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
            await jsonSaverAndLoader.JsonAutoSaver(RepeatTimesBox.Text);
        }


        private void RepeatTimesBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            controls.CheckBoxOnlyNumber(e);
        }

        private void RepeatTimesBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            controls.BoxFocusOut(e, ControllerPage);
        }

        private async void RepeatTimesBox_LostFocus(object sender, RoutedEventArgs e)
        {
            AssignMovementValues();
            await controls.BoxesMaxNumberLimiter(Controls.ControlsIndex.RepeatTimesBox,
                                                 RepeatTimesBox,
                                                 FrameSpeedBox,
                                                 DelayBox);
            await jsonSaverAndLoader.JsonAutoSaver(RepeatTimesBox.Text);
        }
        #endregion

        #region PLAYBACK
        private async void PlayTop_Click(object sender, RoutedEventArgs e)
        {
            await jsonSaverAndLoader.JsonAutoSaver(RepeatTimesBox.Text);
            repeatTimes = Convert.ToInt32(RepeatTimesBox.Text);
            currentFramePosition = 0;
            playback.SetPlayingStatus(isON: true, playbackTimer, Blocker1, Blocker2, Blocker3, StopPlayback);
        }

        private async void PlaySelected_Click(object sender, RoutedEventArgs e)
        {
            await jsonSaverAndLoader.JsonAutoSaver(RepeatTimesBox.Text);
            repeatTimes = Convert.ToInt32(RepeatTimesBox.Text);
            currentFramePosition = FramesListView.SelectedIndex;
            playback.SetPlayingStatus(isON: true, playbackTimer, Blocker1, Blocker2, Blocker3, StopPlayback);
        }

        private async void StopPlayback_Click(object sender, RoutedEventArgs e)
        {
            await jsonSaverAndLoader.JsonAutoSaver(RepeatTimesBox.Text);
            playback.SetPlayingStatus(isON: false, playbackTimer, Blocker1, Blocker2, Blocker3, StopPlayback);
        }
        #endregion

        #region JSON - SAVE/OPEN
        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await jsonSaverAndLoader.SaveJsonWithFilePicker(RepeatTimesBox.Text);
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog("Error saving the file.\n" + ex.Message);
                await dialog.ShowAsync();
            }
        }

        private async void Open_Click(object sender, RoutedEventArgs e)
        {
            isOkToSendMoviments = false;
            try
            {
                await jsonSaverAndLoader.OpenJsonWithFilePicker(FramesListView, RepeatTimesBox, movement);
            }
            catch
            {
                var dialog = new MessageDialog("File Invalid or Corrupted!", "Error");
                await dialog.ShowAsync();
            }
            AssignMovementValues();
            isOkToSendMoviments = true;
        }
        #endregion

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            await httpRequests.SendMovementToRobot(movement);
            await jsonSaverAndLoader.JsonAutoSaver(RepeatTimesBox.Text);
        }

        private async void ResetControlsButton_Click(object sender, RoutedEventArgs e)
        {
            reset.ResetControls(Eixo1Slider,
                                Eixo2Slider,
                                Eixo3Slider,
                                Eixo4Slider,
                                GarraSlider,
                                RepeatTimesBox,
                                FrameSpeedBox,
                                DelayBox,
                                FramesListView,
                                framesList,
                                defaultMovement);
            await jsonSaverAndLoader.JsonAutoDeleter();
        }

        private async void InsertFrame_Click(object sender, RoutedEventArgs e)
        {
            frames.InsertFrameFunction(FramesListView, movement);
            await jsonSaverAndLoader.JsonAutoSaver(RepeatTimesBox.Text);
        }

        private async void OverwriteFrame_Click(object sender, RoutedEventArgs e)
        {
            frames.OverwriteFrameFunction(FramesListView, movement);
            await jsonSaverAndLoader.JsonAutoSaver(RepeatTimesBox.Text);
        }

        private async void DeleteFrame_Click(object sender, RoutedEventArgs e)
        {
            frames.DeleteFrameFunction(FramesListView);
            await jsonSaverAndLoader.JsonAutoSaver(RepeatTimesBox.Text);
        }

        private void FramesListView_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            frames.SendItemsToSlidersWhenDoubleTapped(FrameSpeedBox,
                                                      DelayBox,
                                                      FramesListView,
                                                      Eixo1Slider,
                                                      Eixo2Slider,
                                                      Eixo3Slider,
                                                      Eixo4Slider,
                                                      GarraSlider);
        }

        private async void FramesListView_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Delete)
            {
                frames.DeleteFrameFunction(FramesListView);
                await jsonSaverAndLoader.JsonAutoSaver(RepeatTimesBox.Text);
            }
        }
        #endregion

        #region TIMERS
        private async void WifiCheckerTimer_Tick(object sender, object e)
        {
            status = await wiFiAPConnection.WifiConnectionStatus(isDisconnected: false, isConnecting: false);
            if (status == WiFiAPConnection.Status.Connected && isPlaying != true && isChangingControls != true)
                blocker.BlockControls(locked: false, Blocker1, Blocker2, Blocker3);
            else
                blocker.BlockControls(locked: true, Blocker1, Blocker2, Blocker3);

            if (status != WiFiAPConnection.Status.Connected)
                blocker.SetStopButtonZIndexToBlock(isButtonAhead: false, Blocker2, StopPlayback);
            QuantidadeItens.Text = Convert.ToString(framesList.Count);
        }

        private void PlaybackTimer_Tick(object sender, object e)
        {
            //SliderArray = SlidersControls();
            playback.FramePlayback(playbackTimer,
                                   Blocker1,
                                   Blocker2,
                                   Blocker3,
                                   RepeatTimesBox,
                                   FrameSpeedBox,
                                   DelayBox,
                                   FramesListView,
                                   Eixo1Slider,
                                   Eixo2Slider,
                                   Eixo3Slider,
                                   Eixo4Slider,
                                   GarraSlider,
                                   StopPlayback);
        }
        #endregion

        #region OTHER FUNCTIONS
        public void AssignMovementValues()
        {
            try
            {
                movement.Garra = (int)GarraSlider.Value;
                movement.Axis4 = (int)Eixo4Slider.Value;
                movement.Axis3 = (int)Eixo3Slider.Value;
                movement.Axis2 = (int)Eixo2Slider.Value;
                movement.Axis1 = (int)Eixo1Slider.Value;
                movement.Speed = Convert.ToInt16(FrameSpeedBox.Text);
                movement.Delay = Convert.ToInt32(DelayBox.Text);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("AssignMovementValues() Exception: " + ex.Message);
            }
        }

        public Slider[] SlidersControls()
        {
            Slider[] SliderArray = new Slider[numberOfAxis] { GarraSlider, Eixo4Slider, Eixo3Slider, Eixo2Slider, Eixo1Slider };
            //GarraSlider.Value = x;
            /*for (int i = 0; i < numberOfControls; i++) 
            {
            
            }*/
            return SliderArray;
        }
        #endregion
    }
}