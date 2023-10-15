using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace WaterAutomat.Animation
{
    public static class CoinAnimation
    {
        public async static Task TakingCoin(Button button)
        {
            button.Background = new SolidColorBrush(Colors.Yellow);
            await Task.Delay(1000);
            button.Background = new SolidColorBrush(Colors.Black);
        }
    }
}
