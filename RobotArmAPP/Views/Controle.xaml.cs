﻿using Newtonsoft.Json;
using RobotArmAPP.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
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
        private DispatcherTimer loadJsonTimer;

        WiFiAPConnection wiFiAPConnection = new WiFiAPConnection();
        #endregion

        #region INITIALIZATION
        public Controle()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            wiFiAPConnection.RequestWifiAcess();
            await LoadJsonSaved();

            try
            {
                await ReadyToSend(200);
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
            AutoSaveJson();
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
            AutoSaveJson();
        }

        private void MinDelay_Click(object sender, RoutedEventArgs e)//CALCULADO APENAS PARA O MG995
        {
            SwitchMinimizeDelay(1);
            AutoSaveJson();
        }

        private void MinDelayAll_Click(object sender, RoutedEventArgs e)
        {
            changingControls = true;
            LockControls(true, true);
            Canvas.SetZIndex(StopPlayback, 1);
            Canvas.SetZIndex(Blocker2, 2);

            SwitchMinimizeDelay(0);

            changingControls = false;
            LockControls(false, false);

            AutoSaveJson();
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
            AutoSaveJson();
        }
        #endregion

        #region CONTROLS
        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            await SendAxisValuesToEsp();
            AutoSaveJson();
        }

        private void ResetControlsButton_Click(object sender, RoutedEventArgs e)
        {
            ResetControls();
            DeleteJsonSaved();
        }

        private void InsertFrame_Click(object sender, RoutedEventArgs e)
        {
            InsertFrameFunction();
            AutoSaveJson();
        }

        private void OverwriteFrame_Click(object sender, RoutedEventArgs e)
        {
            OverwriteFrameFunction();
            AutoSaveJson();
        }

        private void DeleteFrame_Click(object sender, RoutedEventArgs e)
        {
            if (FramesListView.SelectedItems.Count == 0)
            {
                return;
            }

            DeleteFrameFunction();
            AutoSaveJson();
        }

        private void FramesListView_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            SendItemsToSlidersWhenDoubleTapped();
        }

        private void FramesListView_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Delete)
            {
                if (FramesListView.SelectedItems.Count == 0)
                {
                    return;
                }

                DeleteFrameFunction();

                AutoSaveJson();
            }
        }
        #endregion

        #region PLAYBACK
        private void PlayTop_Click(object sender, RoutedEventArgs e)
        {
            AutoSaveJson();
            repeatTimes = Convert.ToInt32(RepeatTimesBox.Text);
            currentFrame = 0;
            IsPlaying(true);
        }

        private void PlaySelected_Click(object sender, RoutedEventArgs e)
        {
            AutoSaveJson();
            repeatTimes = Convert.ToInt32(RepeatTimesBox.Text);
            currentFrame = FramesListView.SelectedIndex;
            IsPlaying(true);
        }

        private void StopPlayback_Click(object sender, RoutedEventArgs e)
        {
            AutoSaveJson();
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

        #region JSON - SAVE/LOAD/DELETE
        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (framesList.Count <= 0)
            {
                return;
            }

            FileSavePicker savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            savePicker.FileTypeChoices.Add("JSON File", new List<string>() { ".json" });
            savePicker.SuggestedFileName = "Frames";
            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                /*SERIALIZACAO MANUAL EM JSON*/
                try
                {
                    CachedFileManager.DeferUpdates(file); //Evita outros programas editarem o arquivo enquanto ele está sendo gravado
                    string repeatTimes = string.Format("{0:D5}", RepeatTimesBox.Text);
                    await FileIO.WriteTextAsync(file, "{\"repeatTimes\":" + repeatTimes + ",\"moves\":[");
                    for (int i = 0; i < framesList.Count; i++) //salva os frames linha por linha
                    {
                        string garra = string.Format("{0:D3}", framesList[i][0]);
                        string axis4 = string.Format("{0:D3}", framesList[i][1]);
                        string axis3 = string.Format("{0:D3}", framesList[i][2]);
                        string axis2 = string.Format("{0:D3}", framesList[i][3]);
                        string axis1 = string.Format("{0:D3}", framesList[i][4]);
                        string speed = string.Format("{0:D3}", framesList[i][5]);
                        string delay = string.Format("{0:D6}", framesList[i][6]);

                        await FileIO.AppendTextAsync(file, "{\"garra\":\"" + garra + "\",\"axis4\":\"" + axis4 + "\",\"axis3\":\"" + axis3 + "\",\"axis2\":\"" + axis2 + "\",\"axis1\":\"" + axis1 + "\",\"speed\":\"" + speed + "\",\"delay\":\"" + delay + "\"}");
                        if (i != framesList.Count - 1)
                        {
                            await FileIO.AppendTextAsync(file, ",");
                        }
                    }

                    await FileIO.AppendTextAsync(file, "]}");
                    FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file); //permite que outros programas editem o arquivo
                                                                                                  /*FIM DA SERIALIZACAO MANUAL*/
                }
                catch (Exception ex)
                {
                    var dialog = new MessageDialog("Error saving the file.\n" + ex.Message);
                    await dialog.ShowAsync();
                }
            }
        }

        private async void Open_Click(object sender, RoutedEventArgs e)
        {
            await OpenJsonWithFilePicker();
        }

        private async Task AutoSaveJson()
        {
            if (framesList.Count <= 0)
            {
                return;
            }
            await JsonSaver();
        }

        private async Task LoadJsonSaved()
        {
            await JsonLoader();
        }

        private async void DeleteJsonSaved()
        {
            await DeleteAutoSavedJson();
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
            try
            {
                await GetRequest(Convert.ToString(Eixo1Slider.Value), Convert.ToString(Eixo2Slider.Value), Convert.ToString(Eixo3Slider.Value), Convert.ToString(Eixo4Slider.Value), Convert.ToString(GarraSlider.Value));
            }
            catch { }
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
                try
                {
                    SwitchAxisSliderToBox(axis);
                    await GetRequest(Convert.ToString(Eixo1Slider.Value), Convert.ToString(Eixo2Slider.Value), Convert.ToString(Eixo3Slider.Value), Convert.ToString(Eixo4Slider.Value), Convert.ToString(GarraSlider.Value));
                }
                catch { }
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
            switch (type)
            {
                case 0:
                    try
                    {
                        for (int selected = 0; selected < FramesListView.Items.Count; selected++)
                        {
                            MinimizeDelayCalculus(selected);
                        }
                        FramesListView.SelectedIndex = FramesListView.Items.Count - 1;
                    }
                    catch { }
                    break;
                case 1:
                    try
                    {
                        int selected = FramesListView.SelectedIndex;
                        MinimizeDelayCalculus(selected);
                        FramesListView.SelectedIndex = selected;
                    }
                    catch { }
                    break;
            }
        }

        public void MinimizeDelayCalculus(int selected)
        {
            FramesListView.SelectedIndex = selected;
            if (selected == 0)
            {
                int[] selectedArray = framesList[selected];
                int[] lastArray = framesList[FramesListView.Items.Count - 1];
                int speed = selectedArray[5];

                if (selectedArray == lastArray)
                {
                    int minimum = 900 * 100 / speed;

                    framesList[selected] = new int[] { selectedArray[0], selectedArray[1], selectedArray[2], selectedArray[3], selectedArray[4], selectedArray[5], minimum };
                    FramesListView.Items.Insert(selected, SelectedItemListToString(minimum));
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
                    FramesListView.Items.Insert(selected, SelectedItemListToString(delayMin));
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
                FramesListView.Items.Insert(selected, SelectedItemListToString(delayMin));
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

                if (FramesListView.SelectedItems.Count > 0)
                {
                    framesList.Insert(FramesListView.SelectedIndex + 1, new int[] { (int)GarraSlider.Value, (int)Eixo4Slider.Value, (int)Eixo3Slider.Value, (int)Eixo2Slider.Value, (int)Eixo1Slider.Value, frameSpeed, frameDelay });
                    string add = SlidersToString();
                    FramesListView.Items.Insert(FramesListView.SelectedIndex + 1, add);
                    FramesListView.SelectedIndex = FramesListView.SelectedIndex + 1;
                }
                else
                {
                    framesList.Add(new int[] { (int)GarraSlider.Value, (int)Eixo4Slider.Value, (int)Eixo3Slider.Value, (int)Eixo2Slider.Value, (int)Eixo1Slider.Value, frameSpeed, frameDelay });
                    string add = SlidersToString();
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
                FramesListView.Items.Insert(selectedIndex, SlidersToString());
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



        private string SelectedItemListToString(int delay)
        {
            int selected = FramesListView.SelectedIndex;
            int[] selectedArray = framesList[selected];
            try
            {
                int frameSpeed = Convert.ToInt16(FrameSpeedBox.Text);
                int frameDelay = Convert.ToInt32(DelayBox.Text);
                string add = "[";
                add += selectedArray[0].ToString("000");
                add += ",";
                add += selectedArray[1].ToString("000");
                add += ",";
                add += selectedArray[2].ToString("000");
                add += ",";
                add += selectedArray[3].ToString("000");
                add += ",";
                add += selectedArray[4].ToString("000");
                add += "]";
                add += " Speed: ";
                add += selectedArray[5].ToString("000");
                add += ", Delay: ";
                add += delay.ToString("000000");
                add += "ms";
                return add;
            }
            catch
            {
                return null;
            }
        }

        private string SlidersToString()
        {
            try
            {
                int frameSpeed = Convert.ToInt16(FrameSpeedBox.Text);
                int frameDelay = Convert.ToInt32(DelayBox.Text);
                string add = "[";
                add += GarraSlider.Value.ToString("000");
                add += ",";
                add += Eixo4Slider.Value.ToString("000");
                add += ",";
                add += Eixo3Slider.Value.ToString("000");
                add += ",";
                add += Eixo2Slider.Value.ToString("000");
                add += ",";
                add += Eixo1Slider.Value.ToString("000");
                add += "]";
                add += " Speed: ";
                add += frameSpeed.ToString("000");
                add += ", Delay: ";
                add += frameDelay.ToString("000000");
                add += "ms";
                return add;
            }
            catch
            {
                return null;
            }
        }

        private string DirectFromListToString(int garra, int axis4, int axis3, int axis2, int axis1, int speed, int delay)
        {
            try
            {
                int frameSpeed = Convert.ToInt16(FrameSpeedBox.Text);
                int frameDelay = Convert.ToInt32(DelayBox.Text);
                string add = "[";
                add += garra.ToString("000");
                add += ",";
                add += axis4.ToString("000");
                add += ",";
                add += axis3.ToString("000");
                add += ",";
                add += axis2.ToString("000");
                add += ",";
                add += axis1.ToString("000");
                add += "]";
                add += " Speed: ";
                add += speed.ToString("000");
                add += ", Delay: ";
                add += delay.ToString("000000");
                add += "ms";
                return add;
            }
            catch
            {
                return null;
            }
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


        public async Task JsonSaver()
        {
            try
            {
                StorageFile autoSaveFile = await ApplicationData.Current.LocalFolder.CreateFileAsync("autoSaveRobot.json", CreationCollisionOption.ReplaceExisting);
                CachedFileManager.DeferUpdates(autoSaveFile); //Evita outros programas editarem o arquivo enquanto ele está sendo gravado

                string repeatTimes = string.Format("{0:D5}", RepeatTimesBox.Text);
                await FileIO.WriteTextAsync(autoSaveFile, "{\"rpt\":" + repeatTimes + ",\"mov\":");
                var json = JsonConvert.SerializeObject(framesList);
                await FileIO.AppendTextAsync(autoSaveFile, json);
                await FileIO.AppendTextAsync(autoSaveFile, "}");
                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(autoSaveFile); //permite que outros programas editem o arquivo
            }
            catch { }
        }

        public async Task JsonLoader()
        {
            if (App.FirstStart == false)
            {
                try
                {
                    StorageFile autoSaveFile = await ApplicationData.Current.LocalFolder.GetFileAsync("autoSaveRobot.json");
                    string text = await FileIO.ReadTextAsync(autoSaveFile);
                    var moves = Moves.FromJson(text);
                    for (int i = 0; i < moves.Mov.Count; i++)
                    {
                        int garra = Convert.ToInt16(moves.Mov[i][0].ToString());
                        int axis4 = Convert.ToInt16(moves.Mov[i][1].ToString());
                        int axis3 = Convert.ToInt16(moves.Mov[i][2].ToString());
                        int axis2 = Convert.ToInt16(moves.Mov[i][3].ToString());
                        int axis1 = Convert.ToInt16(moves.Mov[i][4].ToString());
                        int speed = Convert.ToInt16(moves.Mov[i][5].ToString());
                        int delay = Convert.ToInt32(moves.Mov[i][6].ToString());
                        framesList.Add(new int[] { garra, axis4, axis3, axis2, axis1, speed, delay });
                        FramesListView.Items.Add(DirectFromListToString(garra, axis4, axis3, axis2, axis1, speed, delay));
                    }
                    RepeatTimesBox.Text = moves.Rpt.ToString();
                    FramesListView.UpdateLayout();
                    FramesListView.SelectedIndex = FramesListView.Items.Count - 1;
                }
                catch
                { //manipular
                    Thread.Sleep(250);
                    try
                    {
                        await GetRequest(Convert.ToString(90), Convert.ToString(90), Convert.ToString(90), Convert.ToString(90), Convert.ToString(90));
                    }
                    catch { }
                }
            }
            else if (App.FirstStart == true)
            {
                App.FirstStart = false;
                try
                {
                    StorageFile autoSaveFile = await ApplicationData.Current.LocalFolder.GetFileAsync("autoSaveRobot.json");
                    if (autoSaveFile != null)
                    {
                        var dialog = new MessageDialog("Do you want to restore the last session?", "Restoration");
                        dialog.Commands.Add(new UICommand { Label = "Yes", Id = 0 });
                        dialog.Commands.Add(new UICommand { Label = "No", Id = 1 });
                        var resposta = await dialog.ShowAsync();
                        if ((int)resposta.Id == 0)
                        {
                            try
                            {
                                string text = await FileIO.ReadTextAsync(autoSaveFile);

                                var moves = Moves.FromJson(text);
                                for (int i = 0; i < moves.Mov.Count; i++)
                                {
                                    int garra = Convert.ToInt16(moves.Mov[i][0]);
                                    int axis4 = Convert.ToInt16(moves.Mov[i][1]);
                                    int axis3 = Convert.ToInt16(moves.Mov[i][2]);
                                    int axis2 = Convert.ToInt16(moves.Mov[i][3]);
                                    int axis1 = Convert.ToInt16(moves.Mov[i][4]);
                                    int speed = Convert.ToInt16(moves.Mov[i][5]);
                                    int delay = Convert.ToInt32(moves.Mov[i][6]);
                                    framesList.Add(new int[] { garra, axis4, axis3, axis2, axis1, speed, delay });
                                    FramesListView.Items.Add(DirectFromListToString(garra, axis4, axis3, axis2, axis1, speed, delay));
                                }
                                RepeatTimesBox.Text = moves.Rpt.ToString();
                                FramesListView.UpdateLayout();
                                FramesListView.SelectedIndex = FramesListView.Items.Count - 1;
                            }
                            catch
                            {
                                Thread.Sleep(250);
                                try
                                {
                                    await GetRequest(Convert.ToString(90), Convert.ToString(90), Convert.ToString(90), Convert.ToString(90), Convert.ToString(90));
                                }
                                catch { }
                            }
                        }
                        else
                        {
                            DeleteJsonSaved();
                        }
                    }
                }
                catch { }
            }
        }

        public async Task OpenJsonWithFilePicker()
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            picker.FileTypeFilter.Add(".json");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                try
                {
                    framesList.Clear();
                    FramesListView.Items.Clear();

                    var stream = await file.OpenAsync(FileAccessMode.Read);
                    using (StreamReader reader = new StreamReader(stream.AsStream()))
                    {
                        while (reader.Peek() > 0)
                        {
                            okToSend = false;

                            double garraCurrent = GarraSlider.Value;
                            double axis4Current = Eixo4Slider.Value;
                            double axis3Current = Eixo3Slider.Value;
                            double axis2Current = Eixo2Slider.Value;
                            double axis1Current = Eixo1Slider.Value;
                            string speedCurrent = FrameSpeedBox.Text;
                            string delayCurrent = DelayBox.Text;

                            var moves = Moves.FromJson(reader.ReadToEnd());
                            for (int i = 0; i < moves.Movements.Count; i++)
                            {
                                int garra = Convert.ToInt16(moves.Movements[i].Garra);
                                int axis4 = Convert.ToInt16(moves.Movements[i].Axis4);
                                int axis3 = Convert.ToInt16(moves.Movements[i].Axis3);
                                int axis2 = Convert.ToInt16(moves.Movements[i].Axis2);
                                int axis1 = Convert.ToInt16(moves.Movements[i].Axis1);
                                int speed = Convert.ToInt16(moves.Movements[i].Speed);
                                int delay = Convert.ToInt32(moves.Movements[i].Delay);
                                framesList.Add(new int[] { garra, axis4, axis3, axis2, axis1, speed, delay });
                                FramesListView.Items.Add(SlidersToString());
                            }

                            RepeatTimesBox.Text = moves.RepeatTimes.ToString();
                            FramesListView.SelectedIndex = FramesListView.Items.Count - 1;

                            GarraSlider.Value = garraCurrent;
                            Eixo4Slider.Value = axis4Current;
                            Eixo3Slider.Value = axis3Current;
                            Eixo2Slider.Value = axis2Current;
                            Eixo1Slider.Value = axis1Current;
                            FrameSpeedBox.Text = speedCurrent;
                            DelayBox.Text = delayCurrent;

                            okToSend = true;
                        }
                    }
                }
                catch
                {
                    var dialog = new MessageDialog("File Invalid or Corrupted!", "Error");
                    await dialog.ShowAsync();
                }
            }
        }

        public async Task DeleteAutoSavedJson()
        {
            try
            {
                StorageFile autoSaveFile = await ApplicationData.Current.LocalFolder.GetFileAsync("autoSaveRobot.json");
                if (autoSaveFile != null)
                {
                    await autoSaveFile.DeleteAsync();
                }
            }
            catch { }
        }

        #region HTTP REQUESTS 
        private async Task<string> GetRequest(string eixo1Valor, string eixo2Valor, string eixo3Valor, string eixo4Valor, string garraValor)
        {
            HttpClient cliente = new HttpClient();
            string resultado = await cliente.GetStringAsync("http://10.10.10.10:18/?param1=" + eixo1Valor + "&param2=" + eixo2Valor + "&param3=" + eixo3Valor + "&param4=" + eixo4Valor + "&param5=" + garraValor);
            return resultado;
        }

        private async Task<string> GetRequestPlayer(string eixo1Valor, string eixo2Valor, string eixo3Valor, string eixo4Valor, string garraValor, string speed)
        {
            HttpClient cliente = new HttpClient();
            string resultado = await cliente.GetStringAsync("http://10.10.10.10:18/?param1=" + eixo1Valor + "&param2=" + eixo2Valor + "&param3=" + eixo3Valor + "&param4=" + eixo4Valor + "&param5=" + garraValor + "&param6=" + speed);
            return resultado;
        }

        private async Task<string> ReadyToSend(int readyToSend)
        {
            HttpClient cliente = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            string resultado = await cliente.GetStringAsync("http://10.10.10.10/readytosend/?param=" + readyToSend);
            return resultado;
        }
        #endregion
        #endregion
    }
}

/*
 *NOTAS: 
 * Chamar um botão: button_Click(this, new RoutedEventArgs());
 * Delays: Thread.Sleep(1000); -> Trava o UI         Thread.Sleep(1000);  -> Não trava o UI, sai e depois volta pra função
 */

