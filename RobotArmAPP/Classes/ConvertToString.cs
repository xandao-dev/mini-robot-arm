using RobotArmAPP.Views;
using System.Collections.Generic;

namespace RobotArmAPP.Classes
{
    class ConvertToString
    {
        public string ConvertItemToString(int garra, int axis4, int axis3, int axis2, int axis1, int speed, int delay)
        {
            try
            {
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

        public string SelectedItemToString(int index, int delay)
        {
            int[] selectedArray = Controller.framesList[index];
            string add = ConvertItemToString(selectedArray[0], selectedArray[1], selectedArray[2], selectedArray[3], selectedArray[4], selectedArray[5], delay);

            return add;
        }
    }
}