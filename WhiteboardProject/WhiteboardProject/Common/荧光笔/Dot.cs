using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WhiteboardProject.Common
{
    internal class Dot : Canvas
    {
        public double XVelocity = 1.0;
        public double YVelocity = 1.0;
        public double Gravity = 1.0;
        public double X
        {
            get
            {
                return (double)base.GetValue(Canvas.LeftProperty);
            }
            set
            {
                base.SetValue(Canvas.LeftProperty, value);
            }
        }
        public double Y
        {
            get
            {
                return (double)base.GetValue(Canvas.TopProperty);
            }
            set
            {
                base.SetValue(Canvas.TopProperty, value);
            }
        }
        public Dot(byte red, byte green, byte blue, double size)
        {
            double opacity = 1.0;
            Ellipse ellipse = new Ellipse();
            ellipse.Width = size;
            ellipse.Height = size;
            ellipse.Fill = new RadialGradientBrush
            {
                GradientStops =
                {
                    new GradientStop(Color.FromArgb(255, red, green, blue), 0.25),
                    new GradientStop(Color.FromArgb(0, red, green, blue), 1.0)
                }
            };
            ellipse.Opacity = opacity;
            ellipse.SetValue(Canvas.LeftProperty, -ellipse.Width / 2.0);
            ellipse.SetValue(Canvas.TopProperty, -ellipse.Height / 2.0);
            base.Children.Add(ellipse);
        }

        public void RunFirework()
        {
            this.X += this.XVelocity;
            this.Y += this.YVelocity;
            base.Opacity += -0.05;
            this.YVelocity += this.Gravity;
        }
    }
}
