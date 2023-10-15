using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace WaterAutomat.Animation
{
    public static class BottleAnimation
    {
        public static readonly Dictionary<decimal, (double, double)> VolumeSettings = new Dictionary<decimal, (double, double)>()
        {
            {1, (0.25, 0.2) },
            {5, (0.5, 0.5) },
            { 10, (0.75, 0.6) },
            { 20, (1, 1) },
        };
        public static void ResetAnimation(ScaleTransform scaleTransform, Rectangle rectangle)
        {
            scaleTransform.ScaleX = 0;
            scaleTransform.ScaleY = 0;
            rectangle.BeginAnimation(Rectangle.HeightProperty, null);
            rectangle.Height = 0;
        }

        public static void FillBottle(Rectangle rectangle, int time)
        {
            DoubleAnimation recWaterAnimation = new DoubleAnimation();
            recWaterAnimation.From = rectangle.ActualHeight;
            recWaterAnimation.To = 320;
            recWaterAnimation.Duration = TimeSpan.FromSeconds(time);
            rectangle.BeginAnimation(Button.HeightProperty, recWaterAnimation);
        }
    }
}
