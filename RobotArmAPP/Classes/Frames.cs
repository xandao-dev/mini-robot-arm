using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotArmAPP.Classes
{
    class Frames
    {
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
    }
}
