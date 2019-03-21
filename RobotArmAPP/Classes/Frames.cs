using RobotArmAPP.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace RobotArmAPP.Classes
{
    class Frames
    {

        public void InsertFrameFunction(ListView FramesListView, Movement movement)
        {
            try
            {
                string MovesToString =  movement.MovesToString(Movement.StringType.allWithInfo);
                int[] MovesInVector = movement.MovesToIntVector();

                if (FramesListView.SelectedItems.Count > 0)
                {
                    Controller.framesList.Insert(FramesListView.SelectedIndex + 1, MovesInVector);
                    FramesListView.Items.Insert(FramesListView.SelectedIndex + 1, MovesToString);
                    FramesListView.SelectedIndex += 1;
                }
                else
                {
                    Controller.framesList.Add(MovesInVector);
                    FramesListView.Items.Add(MovesToString);
                    FramesListView.SelectedIndex = Controller.framesList.Count() - 1;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("InsertFrameFunction() Exception: " + ex.Message);
            }
        }

        public void OverwriteFrameFunction(ListView FramesListView, Movement movement)
        {
            try
            {
                int selectedIndex = FramesListView.SelectedIndex;
                Controller.framesList[selectedIndex] = movement.MovesToIntVector();
                FramesListView.Items.Insert(selectedIndex, movement.MovesToString(Movement.StringType.allWithInfo));
                FramesListView.Items.RemoveAt(selectedIndex + 1);
                FramesListView.SelectedIndex = selectedIndex;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("OverwriteFrameFunction() Exception: " + ex.Message);
            }
        }

        public void DeleteFrameFunction(ListView FramesListView)
        {
            if (FramesListView.SelectedItems.Count <= 0)
            {
                return;
            }

            int selected = FramesListView.SelectedIndex;
            Controller.framesList.RemoveAt(selected);
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
                catch(Exception ex)
                {
                    Debug.WriteLine("DeleteFrameFunction() Exception: " + ex.Message);
                }
            }
        }

        public void SendItemsToSlidersWhenDoubleTapped(TextBox FrameSpeedBox, TextBox DelayBox, ListView FramesListView, Slider Eixo1Slider, Slider Eixo2Slider, Slider Eixo3Slider, Slider Eixo4Slider, Slider GarraSlider)
        {
            try
            {
                int[] selectedArray = Controller.framesList[FramesListView.SelectedIndex];

                GarraSlider.Value = selectedArray[0];
                Eixo4Slider.Value = selectedArray[1];
                Eixo3Slider.Value = selectedArray[2];
                Eixo2Slider.Value = selectedArray[3];
                Eixo1Slider.Value = selectedArray[4];
                FrameSpeedBox.Text = Convert.ToString(selectedArray[5]);
                DelayBox.Text = Convert.ToString(selectedArray[6]);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SendItemsToSlidersWhenDoubleTapped() Exception: " + ex.Message);
            }
        }
    }
}
