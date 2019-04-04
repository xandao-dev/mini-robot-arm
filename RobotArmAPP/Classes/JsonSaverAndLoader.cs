using Newtonsoft.Json;
using RobotArmAPP.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace RobotArmAPP.Classes
{
    class JsonSaverAndLoader
    {
        HTTPRequests httpRequests = new HTTPRequests();

        public async Task JsonAutoSaver(string RepeatTimes)
        {
            if (Controller.framesList.Count <= 0)
                return;

            try
            {
                var autoSaveFile = await ApplicationData.Current.LocalFolder.CreateFileAsync("autoSaveRobot.json", CreationCollisionOption.ReplaceExisting);
                CachedFileManager.DeferUpdates(autoSaveFile);
                string repeatTimes = string.Format("{0:D5}", RepeatTimes);
                await FileIO.WriteTextAsync(autoSaveFile, "{\"rpt\":" + repeatTimes + ",\"mov\":");
                var json = JsonConvert.SerializeObject(Controller.framesList);
                await FileIO.AppendTextAsync(autoSaveFile, json);
                await FileIO.AppendTextAsync(autoSaveFile, "}");
                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(autoSaveFile);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("JsonAutoSaver() Exception: " + ex.Message);
            }
        }

        public async Task JsonAutoLoader(ListView FramesListView,
                                         TextBox RepeatTimesBox,
                                         Movement movement,
                                         Movement defaultMovement)
        {
            if (App.FirstStart == false)
            {
                try
                {
                    StorageFile autoSaveFile = await ApplicationData.Current.LocalFolder.GetFileAsync("autoSaveRobot.json");
                    await ConvertingJsonToList(file: autoSaveFile, isAutoSavedJson: true, FramesListView, RepeatTimesBox, movement);
                }
                catch
                {
                    Thread.Sleep(250);
                    await httpRequests.SendMovementToRobot(defaultMovement);
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
                            await ConvertingJsonToList(file: autoSaveFile, isAutoSavedJson: true, FramesListView, RepeatTimesBox, movement);
                        else
                            await JsonAutoDeleter();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("JsonAutoLoader() Exception: " + ex.Message);
                }
                //Thread.Sleep(250);
                await httpRequests.SendMovementToRobot(defaultMovement);
            }
        }

        public async Task JsonAutoDeleter()
        {
            try
            {
                var autoSaveFile = await ApplicationData.Current.LocalFolder.GetFileAsync("autoSaveRobot.json");
                if (autoSaveFile != null)
                    await autoSaveFile.DeleteAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("JsonAutoDeleter() Exception: " + ex.Message);
            }
        }


        public async Task OpenJsonWithFilePicker(ListView FramesListView, TextBox RepeatTimesBox, Movement movement)
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
                    Controller.framesList.Clear();
                    FramesListView.Items.Clear();
                    await ConvertingJsonToList(file, isAutoSavedJson: false, FramesListView, RepeatTimesBox, movement);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("JsonAutoSaver() Exception: " + ex.Message);
                    throw;
                }
            }
        }

        public async Task SaveJsonWithFilePicker(string RepeatTimes)
        {
            if (Controller.framesList.Count <= 0)
                return;

            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            savePicker.FileTypeChoices.Add("JSON File", new List<string>() { ".json" });
            savePicker.SuggestedFileName = "Frames";
            var file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                /*SERIALIZACAO MANUAL EM JSON*/
                try
                {
                    CachedFileManager.DeferUpdates(file); //Evita outros programas editarem o arquivo enquanto ele está sendo gravado
                    string repeatTimes = string.Format("{0:D5}", RepeatTimes);
                    await FileIO.WriteTextAsync(file, "{\"repeatTimes\":" + repeatTimes + ",\"moves\":[");
                    for (int i = 0; i < Controller.framesList.Count; i++) //salva os frames linha por linha
                    {
                        string garra = string.Format("{0:D3}", Controller.framesList[i][0]);
                        string axis4 = string.Format("{0:D3}", Controller.framesList[i][1]);
                        string axis3 = string.Format("{0:D3}", Controller.framesList[i][2]);
                        string axis2 = string.Format("{0:D3}", Controller.framesList[i][3]);
                        string axis1 = string.Format("{0:D3}", Controller.framesList[i][4]);
                        string speed = string.Format("{0:D3}", Controller.framesList[i][5]);
                        string delay = string.Format("{0:D6}", Controller.framesList[i][6]);

                        await FileIO.AppendTextAsync(file, "{\"garra\":\"" + garra + "\",\"axis4\":\"" + axis4 + "\",\"axis3\":\"" + axis3 + "\",\"axis2\":\"" + axis2 + "\",\"axis1\":\"" + axis1 + "\",\"speed\":\"" + speed + "\",\"delay\":\"" + delay + "\"}");
                        if (i != Controller.framesList.Count - 1)
                        {
                            await FileIO.AppendTextAsync(file, ",");
                        }
                    }

                    await FileIO.AppendTextAsync(file, "]}");
                    var status = await CachedFileManager.CompleteUpdatesAsync(file);
                    /*FIM DA SERIALIZACAO MANUAL*/
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("SaveJsonWithFilePicker() Exception: " + ex.Message);
                    throw;
                }
            }
        }


        private async Task ConvertingJsonToList(StorageFile file,
                                                bool isAutoSavedJson,
                                                ListView FramesListView,
                                                TextBox RepeatTimesBox,
                                                Movement movement)
        {
            string text = await FileIO.ReadTextAsync(file);
            var moves = Moves.FromJson(text);

            if (isAutoSavedJson == false)
            {
                for (int i = 0; i < moves.Movements.Count; i++)
                {
                    movement.Garra = Convert.ToInt16(moves.Movements[i].Garra);
                    movement.Axis4 = Convert.ToInt16(moves.Movements[i].Axis4);
                    movement.Axis3 = Convert.ToInt16(moves.Movements[i].Axis3);
                    movement.Axis2 = Convert.ToInt16(moves.Movements[i].Axis2);
                    movement.Axis1 = Convert.ToInt16(moves.Movements[i].Axis1);
                    movement.Speed = Convert.ToInt16(moves.Movements[i].Speed);
                    movement.Delay = Convert.ToInt32(moves.Movements[i].Delay);
                    Controller.framesList.Add(movement.MovesToIntVector());
                    FramesListView.Items.Add(movement.MovesToString(Movement.StringType.allWithInfo));
                }
                RepeatTimesBox.Text = moves.RepeatTimes.ToString();
                FramesListView.SelectedIndex = FramesListView.Items.Count - 1;
            }
            else
            {
                for (int i = 0; i < moves.Mov.Count; i++)
                {
                    movement.Garra = Convert.ToInt16(moves.Mov[i][0].ToString());
                    movement.Axis4 = Convert.ToInt16(moves.Mov[i][1].ToString());
                    movement.Axis3 = Convert.ToInt16(moves.Mov[i][2].ToString());
                    movement.Axis2 = Convert.ToInt16(moves.Mov[i][3].ToString());
                    movement.Axis1 = Convert.ToInt16(moves.Mov[i][4].ToString());
                    movement.Speed = Convert.ToInt16(moves.Mov[i][5].ToString());
                    movement.Delay = Convert.ToInt32(moves.Mov[i][6].ToString());
                    Controller.framesList.Add(movement.MovesToIntVector());
                    FramesListView.Items.Add(movement.MovesToString(Movement.StringType.allWithInfo));
                }
                RepeatTimesBox.Text = moves.Rpt.ToString();
                FramesListView.SelectedIndex = FramesListView.Items.Count - 1;
            }

        }
    }
}