using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotArmAPP.Classes
{
    class Movement
    {
        public int garra;
        public int axis4;
        public int axis3;
        public int axis2;
        public int axis1;
        public int speed;
        public int delay;
        public int repeatTimes;

        public Movement(int garra, int axis4, int axis3, int axis2, int axis1, int speed, int delay, int repeatTimes) {
            this.garra = garra;
            this.axis4 = axis4;
            this.axis3 = axis3;
            this.axis2 = axis2;
            this.axis1 = axis1;
            this.speed = speed;
            this.delay = delay;
            this.repeatTimes = repeatTimes;
        }

        public enum StringType
        {
            all,
            allWithInfo,
            onlyAxis
        }

        public string MovesToString(StringType stringType)
        {
            switch (stringType)
            {
                case StringType.all:
                    return garra + "," + axis4 + "," + axis3 + "," + axis2 + "," + axis1 + "," + speed + "," + delay;
                case StringType.allWithInfo:
                    return "["+garra.ToString("000") + "," + axis4.ToString("000") + "," + axis3.ToString("000") + "," + axis2.ToString("000") + "," + axis1.ToString("000") + "] Speed: " + speed.ToString("000") + ", Delay: " + delay.ToString("000000") + "ms";
                case StringType.onlyAxis:
                    return garra + "," + axis4 + "," + axis3 + "," + axis2 + "," + axis1;
            }
            return null;
        }

        /*public List<string> MovesToStringList()
        {
            List<string> Move = new List<string>
            {
                garra.ToString(),
                axis4.ToString(),
                axis3.ToString(),
                axis2.ToString(),
                axis1.ToString(),
                speed.ToString(),
                delay.ToString()
            };
            return Move;
        }*/

        /*public List<int> MovesToIntList()
        {
            List<int> Move = new List<int>
            {
                garra,
                axis4,
                axis3,
                axis2,
                axis1,
                speed,
                delay
            };
            return Move;
        }*/

        /*public string[] MovesToStringVector()
        {
            string[] Move = new string[7];
            Move[0] = garra.ToString();
            Move[1] = axis4.ToString();
            Move[2] = axis3.ToString();
            Move[3] = axis2.ToString();
            Move[4] = axis1.ToString();
            Move[5] = speed.ToString();
            Move[6] = delay.ToString();

            return Move;
        }*/

        public int[] MovesToIntVector()
        {
            int[] Move = new int[7];
            Move[0] = garra;
            Move[1] = axis4;
            Move[2] = axis3;
            Move[3] = axis2;
            Move[4] = axis1;
            Move[5] = speed;
            Move[6] = delay;

            return Move;
        }
    }
}
