using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace RobotArmAPP.Classes
{
    class Reset
    {
        public void ResetControls(Slider Eixo1Slider,
                                  Slider Eixo2Slider,
                                  Slider Eixo3Slider,
                                  Slider Eixo4Slider,
                                  Slider GarraSlider,
                                  TextBox RepeatTimesBox,
                                  TextBox FrameSpeedBox,
                                  TextBox DelayBox,
                                  ListView FramesListView,
                                  List<int[]> framesList,
                                  Movement defaultMovement)
        {
            Eixo1Slider.Value = defaultMovement.axis1;
            Eixo2Slider.Value = defaultMovement.axis2;
            Eixo3Slider.Value = defaultMovement.axis3;
            Eixo4Slider.Value = defaultMovement.axis4;
            GarraSlider.Value = defaultMovement.garra;
            FrameSpeedBox.Text = Convert.ToString(defaultMovement.speed);
            DelayBox.Text = Convert.ToString(defaultMovement.delay);
            RepeatTimesBox.Text = Convert.ToString(defaultMovement.repeatTimes);
            FramesListView.Items.Clear();
            framesList.Clear();
        }
    }
}
