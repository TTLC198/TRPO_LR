using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WaterAutomat
{
    public static class BillAnimation
    {
        public static async Task TakingBill(Rectangle rectangle)
        {
            rectangle.Visibility = Visibility.Visible;
            var marginAnimation = new ThicknessAnimation();
            marginAnimation.From = rectangle.Margin;
            marginAnimation.To = new Thickness(0, 25, 0, 25);
            marginAnimation.Duration = TimeSpan.FromSeconds(2);
            rectangle.BeginAnimation(FrameworkElement.MarginProperty, marginAnimation);
            await Task.Delay(2000);
        }

        public static async Task ReturnBill(Rectangle rectangle)
        {
            var marginAnimation = new ThicknessAnimation();
            rectangle.Margin = new Thickness(0, 25, 0, 25);
            marginAnimation.From = rectangle.Margin;
            marginAnimation.To = new Thickness(0, 25, 0, -100);
            marginAnimation.Duration = TimeSpan.FromSeconds(2);
            rectangle.BeginAnimation(FrameworkElement.MarginProperty, marginAnimation);
            await Task.Delay(2000);
            rectangle.BeginAnimation(FrameworkElement.MarginProperty, null);
            rectangle.Visibility = Visibility.Collapsed;
        }

        public static void ResetAnimation(Rectangle rectangle)
        {
            rectangle.BeginAnimation(FrameworkElement.MarginProperty, null);
            rectangle.Visibility = Visibility.Collapsed;
            rectangle.Margin = new Thickness(0, 25, 0, -100);
        }
    }
}
