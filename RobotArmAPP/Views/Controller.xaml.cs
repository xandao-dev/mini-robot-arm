using RobotArmAPP.Classes;
using System;
using System.Collections.Generic;
using Windows.System;
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
        public static bool okToSend = true;
        public static bool playing = false; //usado para funções de controle programaveis( SetPlayingStatus(),PlayAtTop,PlayFromSelected ... )
        public static bool changingControls = false;
        public bool liveBoxStatus = true; //Váriavel de leitura, mudar ela não altera a liveBoxStatus
        public static int repeatTimes = 0; // numero de repetiçoes das sequencias, essa variavel serve pra armazenar o valor original
        public static int currentFrame = 0;//usado para funções de controle programaveis( SetPlayingStatus(),PlayAtTop,PlayFromSelected ... )
        public static int[] currentFrameArray; //usado para funções de controle programaveis( SetPlayingStatus(),PlayAtTop,PlayFromSelected ... )
        #endregion

        #region OBJECTS
        public static List<int[]> framesList = new List<int[]>(); //Lista é um objeto que armazena variáveis
        #endregion

        #region INSTANCE FIELDS
        private DispatcherTimer WifiCheckerTimer;
        private DispatcherTimer playbackTimer;

        WiFiAPConnection wiFiAPConnection = new WiFiAPConnection();
        WiFiAPConnection.Status status = new WiFiAPConnection.Status();
        HTTPRequests httpRequests = new HTTPRequests();
        JsonSaverAndLoader jsonSaverAndLoader = new JsonSaverAndLoader();
        Controls controls = new Controls();
        Frames frames = new Frames();
        Playback playback = new Playback();
        Movement movement = new Movement(90,90,90,90,90,100,1000,0);
        Movement defaultMovement = new Movement(90, 90, 90, 90, 90, 100, 1000,0);
        #endregion

        #region INITIALIZATION
        public Controller()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            wiFiAPConnection.RequestWifiAccess();
            await jsonSaverAndLoader.JsonAutoLoader(FramesListView, RepeatTimesBox, movement, defaultMovement);
            AssignMovementValues();
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
            AssignMovementValues();
            await controls.SendSlidersValues(liveBoxStatus, okToSend, playing, 1, Eixo1SliderBox, Eixo2SliderBox, Eixo3SliderBox, Eixo4SliderBox, GarraSliderBox, Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider, movement);
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
            await controls.WhenSliderBoxLoseFocus(liveBoxStatus, okToSend, 1, Eixo1SliderBox, Eixo2SliderBox, Eixo3SliderBox, Eixo4SliderBox, GarraSliderBox, Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider, movement);
        }


        private async void Eixo2Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            AssignMovementValues();
            await controls.SendSlidersValues(liveBoxStatus, okToSend, playing, 2, Eixo1SliderBox, Eixo2SliderBox, Eixo3SliderBox, Eixo4SliderBox, GarraSliderBox, Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider, movement);
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
            await controls.WhenSliderBoxLoseFocus(liveBoxStatus, okToSend, 2, Eixo1SliderBox, Eixo2SliderBox, Eixo3SliderBox, Eixo4SliderBox, GarraSliderBox, Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider, movement);
        }


        private async void Eixo3Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            AssignMovementValues();
            await controls.SendSlidersValues(liveBoxStatus, okToSend, playing, 3, Eixo1SliderBox, Eixo2SliderBox, Eixo3SliderBox, Eixo4SliderBox, GarraSliderBox, Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider, movement);
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
            await controls.WhenSliderBoxLoseFocus(liveBoxStatus, okToSend, 3, Eixo1SliderBox, Eixo2SliderBox, Eixo3SliderBox, Eixo4SliderBox, GarraSliderBox, Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider, movement);
        }


        private async void Eixo4Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            AssignMovementValues();
            await controls.SendSlidersValues(liveBoxStatus, okToSend, playing, 4, Eixo1SliderBox, Eixo2SliderBox, Eixo3SliderBox, Eixo4SliderBox, GarraSliderBox, Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider, movement);
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
            await controls.WhenSliderBoxLoseFocus(liveBoxStatus, okToSend, 4, Eixo1SliderBox, Eixo2SliderBox, Eixo3SliderBox, Eixo4SliderBox, GarraSliderBox, Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider, movement);
        }


        private async void GarraSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            AssignMovementValues();
            await controls.SendSlidersValues(liveBoxStatus, okToSend, playing, 5, Eixo1SliderBox, Eixo2SliderBox, Eixo3SliderBox, Eixo4SliderBox, GarraSliderBox, Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider, movement);
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
            await controls.WhenSliderBoxLoseFocus(liveBoxStatus, okToSend, 5, Eixo1SliderBox, Eixo2SliderBox, Eixo3SliderBox, Eixo4SliderBox, GarraSliderBox, Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider, movement);
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
            /**/await controls.BoxesMaxNumberLimiter(1, RepeatTimesBox, FrameSpeedBox, DelayBox);
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
            await controls.BoxesMaxNumberLimiter(2, RepeatTimesBox, FrameSpeedBox, DelayBox);
        }


        private async void MinDelay_Click(object sender, RoutedEventArgs e)//CALCULADO APENAS PARA O MG995
        {
            controls.SwitchMinimizeDelay(1, FrameSpeedBox, FramesListView, framesList, movement);
            await jsonSaverAndLoader.JsonAutoSaver(RepeatTimesBox.Text);
        }

        private async void MinDelayAll_Click(object sender, RoutedEventArgs e)
        {
            changingControls = true;
            controls.LockControls(true, true, Blocker1, Blocker2, Blocker3);
            controls.SetStopButtonZIndex(false, Blocker2, StopPlayback);

            controls.SwitchMinimizeDelay(0, FrameSpeedBox, FramesListView, framesList, movement);

            changingControls = false;
            controls.LockControls(false, false, Blocker1, Blocker2, Blocker3);

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
            await controls.BoxesMaxNumberLimiter(3, RepeatTimesBox, FrameSpeedBox, DelayBox);
            await jsonSaverAndLoader.JsonAutoSaver(RepeatTimesBox.Text);
        }
        #endregion

        #region CONTROLS
        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            await httpRequests.SendMovementToRobot(movement);
            await jsonSaverAndLoader.JsonAutoSaver(RepeatTimesBox.Text);
        }

        private async void ResetControlsButton_Click(object sender, RoutedEventArgs e)
        {
            controls.ResetControls(Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider, RepeatTimesBox, FrameSpeedBox, DelayBox, FramesListView, framesList, defaultMovement);
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
            frames.SendItemsToSlidersWhenDoubleTapped(FrameSpeedBox, DelayBox, FramesListView, Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider);
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

        #region PLAYBACK
        private async void PlayTop_Click(object sender, RoutedEventArgs e)
        {
            await jsonSaverAndLoader.JsonAutoSaver(RepeatTimesBox.Text);
            repeatTimes = Convert.ToInt32(RepeatTimesBox.Text);
            currentFrame = 0;
            playback.SetPlayingStatus(true, playbackTimer, Blocker1, Blocker2, Blocker3, StopPlayback);
        }

        private async void PlaySelected_Click(object sender, RoutedEventArgs e)
        {
            await jsonSaverAndLoader.JsonAutoSaver(RepeatTimesBox.Text);
            repeatTimes = Convert.ToInt32(RepeatTimesBox.Text);
            currentFrame = FramesListView.SelectedIndex;
            playback.SetPlayingStatus(true, playbackTimer, Blocker1, Blocker2, Blocker3, StopPlayback);
        }

        private async void StopPlayback_Click(object sender, RoutedEventArgs e)
        {
            await jsonSaverAndLoader.JsonAutoSaver(RepeatTimesBox.Text);
            playback.SetPlayingStatus(false, playbackTimer, Blocker1, Blocker2, Blocker3, StopPlayback);
        }
        #endregion
        
        #region TIMERS
        private async void WifiCheckerTimer_Tick(object sender, object e)
        {
            status = await wiFiAPConnection.WifiStatus(false, false);
            if (status == WiFiAPConnection.Status.Connected && playing != true && changingControls != true)
            {
                controls.LockControls(null, false, Blocker1, Blocker2, Blocker3);
            }
            else
            {
                controls.LockControls(null, true, Blocker1, Blocker2, Blocker3);
            }

            if (status != WiFiAPConnection.Status.Connected)
            {
                controls.SetStopButtonZIndex(false, Blocker2, StopPlayback);
            }
            QuantidadeItens.Text = Convert.ToString(framesList.Count);
        } //Verifica o Wifi Para bloquear os Controles

        private void PlaybackTimer_Tick(object sender, object e)
        {
            playback.FramePlayback(playbackTimer, Blocker1, Blocker2, Blocker3, RepeatTimesBox, FrameSpeedBox, DelayBox, FramesListView, Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider, StopPlayback);
        } // Timer que envia as sequencias no Play
        #endregion

        #region JSON - SAVE/OPEN
        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            await jsonSaverAndLoader.SaveJsonWithFilePicker(RepeatTimesBox.Text);
        }

        private async void Open_Click(object sender, RoutedEventArgs e)
        {
            okToSend = false;
            await jsonSaverAndLoader.OpenJsonWithFilePicker(FramesListView, RepeatTimesBox,movement);
            okToSend = true;
        }
        #endregion

        #region FUNCTIONS
        public void AssignMovementValues()
        {
            try
            {
                movement.garra = (int)GarraSlider.Value;
                movement.axis4 = (int)Eixo4Slider.Value;
                movement.axis3 = (int)Eixo3Slider.Value;
                movement.axis2 = (int)Eixo2Slider.Value;
                movement.axis1 = (int)Eixo1Slider.Value;
                movement.speed = Convert.ToInt16(FrameSpeedBox.Text);
                movement.delay = Convert.ToInt32(DelayBox.Text);
            }
            catch { }
        }
        #endregion
    }
}