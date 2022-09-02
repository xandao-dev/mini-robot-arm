using RobotArmAPP.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace RobotArmAPP.Classes
{
    class DelayControl
    {
        public int MinimumForMG995 { get; set; } = 900;

        int[] currentArray = new int[7];
        int[] previousArray = new int[7];
        int[] lastArray = new int[7];

        public static List<int[]> axisDelayList = new List<int[]>();

        public enum Delay
        {
            all,
            one
        }

        public string SelectedFrameToString(int index, int delay, Movement movement)
        {
            int[] selectedArray = Controller.framesList[index];
            movement.Garra = selectedArray[0];
            movement.Axis4 = selectedArray[1];
            movement.Axis3 = selectedArray[2];
            movement.Axis2 = selectedArray[3];
            movement.Axis1 = selectedArray[4];
            movement.Speed = selectedArray[5];
            movement.Delay = delay;
            return movement.MovesToString(Movement.StringType.allWithInfo);
        }

        public void SwitchMinimizeDelay(Delay delay,
                                        ListView FramesListView,
                                        List<int[]> framesList,
                                        Movement movement)
        {
            if (framesList.Count <= 0)
                return;

            switch (delay)
            {
                case Delay.all:
                    for (int selected = 0; selected < FramesListView.Items.Count; selected++)
                    {
                        MinimizeDelayCalculus(selected, FramesListView, framesList, movement);
                    }
                    FramesListView.SelectedIndex = FramesListView.Items.Count - 1;
                    break;
                case Delay.one:
                    int index = FramesListView.SelectedIndex;
                    MinimizeDelayCalculus(index, FramesListView, framesList, movement);
                    FramesListView.SelectedIndex = index;
                    break;
            }
        }

        private void MinimizeDelayCalculus(int selected,
                                           ListView FramesListView,
                                           List<int[]> framesList,
                                           Movement movement)
        {
            if (framesList.Count <= 0)
                return;

            FramesListView.SelectedIndex = selected;
            int index = FramesListView.SelectedIndex;

            if (selected == 0)
            {

                int[] selectedArray = framesList[selected];
                int[] lastArray = framesList[FramesListView.Items.Count - 1];
                int speed = selectedArray[5];

                if (selectedArray == lastArray)
                {
                    int minimum = MinimumForMG995 * 100 / speed;
                }
                else
                {
                    int eixo1difference = Math.Abs(selectedArray[0] - lastArray[0]);
                    int eixo2difference = Math.Abs(selectedArray[1] - lastArray[1]);
                    int eixo3difference = Math.Abs(selectedArray[2] - lastArray[2]);
                    int eixo4difference = Math.Abs(selectedArray[3] - lastArray[3]);
                    int garradifference = Math.Abs(selectedArray[4] - lastArray[4]);

                    int biggest = Math.Max(Math.Max(Math.Max(eixo1difference, eixo2difference), eixo3difference), Math.Max(eixo4difference, garradifference));
                    int delayMin = (biggest * (MinimumForMG995 / 180)) * 100 / speed;

                    framesList[selected] = new int[] { selectedArray[0], selectedArray[1], selectedArray[2], selectedArray[3], selectedArray[4], selectedArray[5], delayMin };
                    FramesListView.Items.Insert(selected, SelectedFrameToString(index, delayMin, movement));
                    FramesListView.Items.RemoveAt(selected + 1);
                    FramesListView.SelectedIndex = selected;
                }
            }
            else
            {
                int[] selectedArray = framesList[selected];
                int[] previousArray = framesList[selected - 1];

                int speed = selectedArray[5];
                int eixo1difference = Math.Abs(selectedArray[0] - previousArray[0]);
                int eixo2difference = Math.Abs(selectedArray[1] - previousArray[1]);
                int eixo3difference = Math.Abs(selectedArray[2] - previousArray[2]);
                int eixo4difference = Math.Abs(selectedArray[3] - previousArray[3]);
                int garradifference = Math.Abs(selectedArray[4] - previousArray[4]);

                int biggest = Math.Max(Math.Max(Math.Max(eixo1difference, eixo2difference), eixo3difference), Math.Max(eixo4difference, garradifference));
                int delayMin = (biggest * (MinimumForMG995 / 180)) * 100 / speed;

                framesList[selected] = new int[] { selectedArray[0], selectedArray[1], selectedArray[2], selectedArray[3], selectedArray[4], selectedArray[5], delayMin };
                FramesListView.Items.Insert(selected, SelectedFrameToString(index, delayMin, movement));
                FramesListView.Items.RemoveAt(selected + 1);
                FramesListView.SelectedIndex = selected;
            }
        }

        public List<int[]> MinimumAxisDelay(List<int[]> framesList)
        {
            int numberOfItems = framesList.Count;

            if (numberOfItems <= 0)
                return null;
            for (int i = 0; i < numberOfItems; i++)
            {

                currentArray = framesList[i];
                lastArray = framesList[framesList.Count - 1];

                if (i > 0)
                {
                    previousArray = framesList[i - 1];
                    int speed = currentArray[5];
                    int eixo1delay = (Math.Abs(currentArray[0] - previousArray[0]) * (MinimumForMG995 / 180)) * 100 / speed;
                    int eixo2delay = (Math.Abs(currentArray[1] - previousArray[1]) * (MinimumForMG995 / 180)) * 100 / speed;
                    int eixo3delay = (Math.Abs(currentArray[2] - previousArray[2]) * (MinimumForMG995 / 180)) * 100 / speed;
                    int eixo4delay = (Math.Abs(currentArray[3] - previousArray[3]) * (MinimumForMG995 / 180)) * 100 / speed;
                    int garradelay = (Math.Abs(currentArray[4] - previousArray[4]) * (MinimumForMG995 / 180)) * 100 / speed;
                    int[] differenceVector = new int[6] { garradelay, eixo4delay, eixo3delay, eixo2delay, eixo1delay, speed };
                    axisDelayList.Add(differenceVector);

                }
                else
                {
                    int speed = currentArray[5];
                    int eixo1delay = (Math.Abs(currentArray[0] - lastArray[0]) * (MinimumForMG995 / 180)) * 100 / speed;
                    int eixo2delay = (Math.Abs(currentArray[1] - lastArray[1]) * (MinimumForMG995 / 180)) * 100 / speed;
                    int eixo3delay = (Math.Abs(currentArray[2] - lastArray[2]) * (MinimumForMG995 / 180)) * 100 / speed;
                    int eixo4delay = (Math.Abs(currentArray[3] - lastArray[3]) * (MinimumForMG995 / 180)) * 100 / speed;
                    int garradelay = (Math.Abs(currentArray[4] - lastArray[4]) * (MinimumForMG995 / 180)) * 100 / speed;
                    int[] differenceVector = new int[6] { garradelay, eixo4delay, eixo3delay, eixo2delay, eixo1delay, speed };
                    axisDelayList.Add(differenceVector);
                }

            }
            return axisDelayList;
        }
    }
}
