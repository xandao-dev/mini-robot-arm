using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace RobotArmAPP.Classes
{
    class UpdateSlidersAndBoxes
    {
        Slider GarraSlider, Eixo4Slider, Eixo3Slider, Eixo2Slider, Eixo1Slider;
        TextBox GarraSliderBox, Eixo4SliderBox, Eixo3SliderBox, Eixo2SliderBox, Eixo1SliderBox;
        TextBox FrameSpeedBox, DelayBox, RepeatTimesBox;

        double garra, axis4, axis3, axis2, axis1;
        int speed, delay, repeatTimes;

        public UpdateSlidersAndBoxes(Slider GarraSlider,
                                     Slider Eixo4Slider,
                                     Slider Eixo3Slider,
                                     Slider Eixo2Slider,
                                     Slider Eixo1Slider)
        {
            this.GarraSlider = GarraSlider;
            this.Eixo4Slider = Eixo4Slider;
            this.Eixo3Slider = Eixo3Slider;
            this.Eixo2Slider = Eixo2Slider;
            this.Eixo1Slider = Eixo1Slider;
        }

        public UpdateSlidersAndBoxes(TextBox GarraSliderBox,
                                     TextBox Eixo4SliderBox,
                                     TextBox Eixo3SliderBox,
                                     TextBox Eixo2SliderBox,
                                     TextBox Eixo1SliderBox,
                                     TextBox FrameSpeedBox,
                                     TextBox DelayBox,
                                     TextBox RepeatTimesBox)
        {
            this.GarraSliderBox = GarraSliderBox;
            this.Eixo4SliderBox = Eixo4SliderBox;
            this.Eixo3SliderBox = Eixo3SliderBox;
            this.Eixo2SliderBox = Eixo2SliderBox;
            this.Eixo1SliderBox = Eixo1SliderBox;
        }

        public UpdateSlidersAndBoxes(TextBox FrameSpeedBox, TextBox DelayBox, TextBox RepeatTimesBox)
        {
            this.FrameSpeedBox = FrameSpeedBox;
            this.DelayBox = DelayBox;
            this.RepeatTimesBox = RepeatTimesBox;
        }

        public void SendValuesToSliders()
        {
            GarraSlider.Value = garra;
            Eixo4Slider.Value = axis4;
            Eixo3Slider.Value = axis3;
            Eixo2Slider.Value = axis2;
            Eixo1Slider.Value = axis1;
        }
        public void SendValuesToSlidersBoxes()
        {
            GarraSliderBox.Text = garra.ToString();
            Eixo4SliderBox.Text = axis4.ToString();
            Eixo3SliderBox.Text = axis3.ToString();
            Eixo2SliderBox.Text = axis2.ToString();
            Eixo1SliderBox.Text = axis1.ToString();
        }
        public void SendValuesToControlsBoxes()
        {
            FrameSpeedBox.Text = speed.ToString();
            DelayBox.Text = delay.ToString();
            RepeatTimesBox.Text = repeatTimes.ToString();
        }
    }
}
