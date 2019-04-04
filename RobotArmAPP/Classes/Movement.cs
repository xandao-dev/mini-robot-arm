using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotArmAPP.Classes
{
    class Movement
    {
        public int Garra { get; set; }
        public int Axis4 { get; set; }
        public int Axis3 { get; set; }
        public int Axis2 { get; set; }
        public int Axis1 { get; set; }
        public int Speed { get; set; }
        public int Delay { get; set; }
        public int RepeatTimes { get; set; }

        public Movement(int garra, int axis4, int axis3, int axis2, int axis1, int speed, int delay, int repeatTimes)
        {
            this.Garra = garra;
            this.Axis4 = axis4;
            this.Axis3 = axis3;
            this.Axis2 = axis2;
            this.Axis1 = axis1;
            this.Speed = speed;
            this.Delay = delay;
            this.RepeatTimes = repeatTimes;
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
                    return this.Garra + "," + this.Axis4 + "," + this.Axis3 + "," + this.Axis2 + "," + this.Axis1 + "," + this.Speed + "," + this.Delay;
                case StringType.allWithInfo:
                    return "[" + this.Garra.ToString("000") + "," + this.Axis4.ToString("000") + "," + this.Axis3.ToString("000") + "," + this.Axis2.ToString("000") + "," + this.Axis1.ToString("000") + "] Speed: " + this.Speed.ToString("000") + ", Delay: " + this.Delay.ToString("000000") + "ms";
                case StringType.onlyAxis:
                    return this.Garra + "," + this.Axis4 + "," + this.Axis3 + "," + this.Axis2 + "," + this.Axis1;
            }
            return null;
        }

        public int[] MovesToIntVector()
        {
            int[] Move = new int[7];
            Move[0] = this.Garra;
            Move[1] = this.Axis4;
            Move[2] = this.Axis3;
            Move[3] = this.Axis2;
            Move[4] = this.Axis1;
            Move[5] = this.Speed;
            Move[6] = this.Delay;

            return Move;
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
    }
}
