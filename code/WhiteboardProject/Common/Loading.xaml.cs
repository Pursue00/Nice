using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WhiteboardProject.Common
{
    /// <summary>
    /// Loading.xaml 的交互逻辑
    /// </summary>
    public partial class Loading : Window
    {
        AnimationClock c1;
        AnimationClock c2;
        DoubleAnimation da1;
        DoubleAnimation da2;
        public Loading()
        {
            InitializeComponent();
            Loaded += Loading_Loaded;
        }

        private void Loading_Loaded(object sender, RoutedEventArgs e)
        {
            da1 = new DoubleAnimation()
            {
                From = 0,
                To = 360,
                Duration = TimeSpan.FromSeconds(3),
                RepeatBehavior = RepeatBehavior.Forever,
            };
            da2 = new DoubleAnimation()
            {
                From = 360,
                To = 0,
                Duration = TimeSpan.FromSeconds(3),
                RepeatBehavior = RepeatBehavior.Forever,
            };

            c1 = da1.CreateClock();
            c2 = da2.CreateClock();
            tra1.ApplyAnimationClock(RotateTransform.AngleProperty, c1);
            tra2.ApplyAnimationClock(RotateTransform.AngleProperty, c2);
        }
    }
}
