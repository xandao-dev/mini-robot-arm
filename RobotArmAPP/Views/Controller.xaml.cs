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
        private bool okToSend = true;
        private bool playing = false; //usado para funções de controle programaveis( IsPlaying(),PlayAtTop,PlayFromSelected ... )
        private bool changingControls = false;
        private bool liveBoxStatus = true; //Váriavel de leitura, mudar ela não altera a liveBoxStatus
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
        Controls controls = new Controls();
        Frames frames = new Frames();
        Playback playback = new Playback();
        #endregion

        #region INITIALIZATION
        public Controller()
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
            await controls.SendSlidersValues(liveBoxStatus, okToSend, playing, 1, Eixo1SliderBox, Eixo2SliderBox, Eixo3SliderBox, Eixo4SliderBox, GarraSliderBox, Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider);
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
            await controls.WhenSliderBoxLostFocus(liveBoxStatus, okToSend, 1, Eixo1SliderBox, Eixo2SliderBox, Eixo3SliderBox, Eixo4SliderBox, GarraSliderBox, Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider);
        }


        private async void Eixo2Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            await controls.SendSlidersValues(liveBoxStatus, okToSend, playing, 2, Eixo1SliderBox, Eixo2SliderBox, Eixo3SliderBox, Eixo4SliderBox, GarraSliderBox, Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider);
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
            await controls.WhenSliderBoxLostFocus(liveBoxStatus, okToSend, 2, Eixo1SliderBox, Eixo2SliderBox, Eixo3SliderBox, Eixo4SliderBox, GarraSliderBox, Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider);
        }


        private async void Eixo3Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            await controls.SendSlidersValues(liveBoxStatus, okToSend, playing, 3, Eixo1SliderBox, Eixo2SliderBox, Eixo3SliderBox, Eixo4SliderBox, GarraSliderBox, Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider);
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
            await controls.WhenSliderBoxLostFocus(liveBoxStatus, okToSend, 3, Eixo1SliderBox, Eixo2SliderBox, Eixo3SliderBox, Eixo4SliderBox, GarraSliderBox, Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider);
        }


        private async void Eixo4Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            await controls.SendSlidersValues(liveBoxStatus, okToSend, playing, 4, Eixo1SliderBox, Eixo2SliderBox, Eixo3SliderBox, Eixo4SliderBox, GarraSliderBox, Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider);
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
            await controls.WhenSliderBoxLostFocus(liveBoxStatus, okToSend, 4, Eixo1SliderBox, Eixo2SliderBox, Eixo3SliderBox, Eixo4SliderBox, GarraSliderBox, Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider);
        }


        private async void GarraSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            await controls.SendSlidersValues(liveBoxStatus, okToSend, playing, 5, Eixo1SliderBox, Eixo2SliderBox, Eixo3SliderBox, Eixo4SliderBox, GarraSliderBox, Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider);
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
            await controls.WhenSliderBoxLostFocus(liveBoxStatus, okToSend, 5, Eixo1SliderBox, Eixo2SliderBox, Eixo3SliderBox, Eixo4SliderBox, GarraSliderBox, Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider);
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
            await controls.BoxesMaxNumberLimiter(1, RepeatTimesBox, FrameSpeedBox, DelayBox);
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
            await controls.BoxesMaxNumberLimiter(2, RepeatTimesBox, FrameSpeedBox, DelayBox);
        }


        private async void MinDelay_Click(object sender, RoutedEventArgs e)//CALCULADO APENAS PARA O MG995
        {
            controls.SwitchMinimizeDelay(1, FrameSpeedBox, FramesListView, framesList);
            await jsonSaverAndLoader.JsonAutoSaver(framesList, RepeatTimesBox.Text);
        }

        private async void MinDelayAll_Click(object sender, RoutedEventArgs e)
        {
            changingControls = true;
            controls.LockControls(true, true, Blocker1, Blocker2, Blocker3);
            Canvas.SetZIndex(StopPlayback, 1);
            Canvas.SetZIndex(Blocker2, 2);

            controls.SwitchMinimizeDelay(0, FrameSpeedBox, FramesListView, framesList);

            changingControls = false;
            controls.LockControls(false, false, Blocker1, Blocker2, Blocker3);

            await jsonSaverAndLoader.JsonAutoSaver(framesList, RepeatTimesBox.Text);
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
            await controls.BoxesMaxNumberLimiter(3, RepeatTimesBox, FrameSpeedBox, DelayBox);
            await jsonSaverAndLoader.JsonAutoSaver(framesList, RepeatTimesBox.Text);
        }
        #endregion

        #region CONTROLS
        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            await httpRequests.GetRequest(Convert.ToString(Eixo1Slider.Value), Convert.ToString(Eixo2Slider.Value), Convert.ToString(Eixo3Slider.Value), Convert.ToString(Eixo4Slider.Value), Convert.ToString(GarraSlider.Value));
            await jsonSaverAndLoader.JsonAutoSaver(framesList, RepeatTimesBox.Text);
        }

        private async void ResetControlsButton_Click(object sender, RoutedEventArgs e)
        {
            controls.ResetControls(Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider, RepeatTimesBox, FrameSpeedBox, DelayBox, FramesListView, framesList);
            await jsonSaverAndLoader.JsonAutoDeleter();
        }

        private async void InsertFrame_Click(object sender, RoutedEventArgs e)
        {
            frames.InsertFrameFunction(FrameSpeedBox, DelayBox, FramesListView, framesList, Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider);
            await jsonSaverAndLoader.JsonAutoSaver(framesList, RepeatTimesBox.Text);
        }

        private async void OverwriteFrame_Click(object sender, RoutedEventArgs e)
        {
            frames.OverwriteFrameFunction(FrameSpeedBox, DelayBox, FramesListView, framesList, Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider);
            await jsonSaverAndLoader.JsonAutoSaver(framesList, RepeatTimesBox.Text);
        }

        private async void DeleteFrame_Click(object sender, RoutedEventArgs e)
        {
            if (FramesListView.SelectedItems.Count == 0)
            {
                return;
            }

            frames.DeleteFrameFunction(FramesListView, framesList);
            await jsonSaverAndLoader.JsonAutoSaver(framesList, RepeatTimesBox.Text);
        }

        private void FramesListView_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            frames.SendItemsToSlidersWhenDoubleTapped(FrameSpeedBox, DelayBox, FramesListView, framesList, Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider);
        }

        private async void FramesListView_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Delete)
            {
                if (FramesListView.SelectedItems.Count == 0)
                {
                    return;
                }

                frames.DeleteFrameFunction(FramesListView, framesList);
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
            playback.IsPlaying(true, playing, okToSend, framesList, playbackTimer, Blocker1, Blocker2, Blocker3, StopPlayback);
        }

        private async void PlaySelected_Click(object sender, RoutedEventArgs e)
        {
            await jsonSaverAndLoader.JsonAutoSaver(framesList, RepeatTimesBox.Text);
            repeatTimes = Convert.ToInt32(RepeatTimesBox.Text);
            currentFrame = FramesListView.SelectedIndex;
            playback.IsPlaying(true, playing, okToSend, framesList, playbackTimer, Blocker1, Blocker2, Blocker3, StopPlayback);
        }

        private async void StopPlayback_Click(object sender, RoutedEventArgs e)
        {
            await jsonSaverAndLoader.JsonAutoSaver(framesList, RepeatTimesBox.Text);
            playback.IsPlaying(false, playing, okToSend, framesList, playbackTimer, Blocker1, Blocker2, Blocker3, StopPlayback);
        }
        #endregion

        #region TIMERS
        private async void WifiCheckerTimer_Tick(object sender, object e)
        {
            int status = await wiFiAPConnection.WifiStatus(false, false);
            if (status == 1 && playing != true && changingControls != true)
            {
                controls.LockControls(null, false, Blocker1, Blocker2, Blocker3);
            }
            else if (status != 1)
            {
                Canvas.SetZIndex(StopPlayback, 1);
                Canvas.SetZIndex(Blocker2, 2);
            }
            else
            {
                controls.LockControls(null, true, Blocker1, Blocker2, Blocker3);
            }
            QuantidadeItens.Text = Convert.ToString(framesList.Count);
        }  /*ATIVAR*/ //Verifica o Wifi Para bloquear os Controles

        private void PlaybackTimer_Tick(object sender, object e)
        {
            playback.PlaybackFunction(playing, okToSend, currentFrame, repeatTimes, currentFrameArray, framesList, playbackTimer, Blocker1, Blocker2, Blocker3, RepeatTimesBox, FrameSpeedBox, DelayBox, FramesListView, Eixo1Slider, Eixo2Slider, Eixo3Slider, Eixo4Slider, GarraSlider, StopPlayback);
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
    }
}

/*
 *NOTAS: 
 * Chamar um botão: button_Click(this, new RoutedEventArgs());
 * Delays: Thread.Sleep(1000); -> Trava o UI         Thread.Sleep(1000);  -> Não trava o UI, sai e depois volta pra função
 */

