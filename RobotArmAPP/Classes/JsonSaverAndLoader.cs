using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.UI.Popups;

namespace RobotArmAPP.Classes
{
    class JsonSaverAndLoader
    {
        HTTPRequests httpRequests = new HTTPRequests();
        ConvertToString convertToString = new ConvertToString();

        public async Task JsonAutoSaver(List<int[]> framesList, string RepeatTimes)
        {
            if (framesList.Count <= 0)
            {
                return;
            }

            try
            {
                StorageFile autoSaveFile = await ApplicationData.Current.LocalFolder.CreateFileAsync("autoSaveRobot.json", CreationCollisionOption.ReplaceExisting);
                CachedFileManager.DeferUpdates(autoSaveFile); //Evita outros programas editarem o arquivo enquanto ele está sendo gravado
                string repeatTimes = string.Format("{0:D5}", RepeatTimes);
                await FileIO.WriteTextAsync(autoSaveFile, "{\"rpt\":" + repeatTimes + ",\"mov\":");
                var json = JsonConvert.SerializeObject(framesList);
                await FileIO.AppendTextAsync(autoSaveFile, json);
                await FileIO.AppendTextAsync(autoSaveFile, "}");
                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(autoSaveFile); //permite que outros programas editem o arquivo
            }
            catch { }
        }

        public async Task JsonAutoLoader()
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
                        FramesListView.Items.Add(convertToString.ConvertItemToString(garra, axis4, axis3, axis2, axis1, speed, delay));
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
                        await httpRequests.GetRequest(Convert.ToString(90), Convert.ToString(90), Convert.ToString(90), Convert.ToString(90), Convert.ToString(90));
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
                                    FramesListView.Items.Add(convertToString.ConvertItemToString(garra, axis4, axis3, axis2, axis1, speed, delay));
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
                                    await httpRequests.GetRequest(Convert.ToString(90), Convert.ToString(90), Convert.ToString(90), Convert.ToString(90), Convert.ToString(90));
                                }
                                catch { }
                            }
                        }
                        else
                        {
                            await JsonAutoDeleter();
                        }
                    }
                }
                catch { }
            }
        }

        public async Task JsonAutoDeleter()
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


        public async Task OpenJsonWithFilePicker(bool okToSend)
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
                                FramesListView.Items.Add(convertToString.ConvertItemToString(garra, axis4, axis3, axis2, axis1, speed, delay));
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

        public async Task SaveJsonWithFilePicker(List<int[]> framesList, string RepeatTimes)
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
                    string repeatTimes = string.Format("{0:D5}", RepeatTimes);
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
    }
}
