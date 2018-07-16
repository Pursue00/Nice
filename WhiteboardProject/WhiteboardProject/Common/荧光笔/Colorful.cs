using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WhiteboardProject.Common
{
    public class Colorful : Canvas
    {
        private class Data
        {
            private int _currentTime;
            private Ellipse _ellipseObject;
            public int CurrentTime
            {
                get
                {
                    return this._currentTime;
                }
                set
                {
                    this._currentTime = value;
                }
            }
            public Ellipse EllipseObject
            {
                get
                {
                    return this._ellipseObject;
                }
                set
                {
                    this._ellipseObject = value;
                }
            }
        }
        private const double intervaltime = 1000.0;
        private List<Dot> _dotGroup = new List<Dot>();
        private DispatcherTimer _timer;
        private List<Colorful.Data> list = new List<Colorful.Data>();
        private int lastTick;
        private int currentTick;
        private Ellipse ellipse;

        public Random random = new Random((int)DateTime.Now.Ticks);
        public Colorful()
        {
            base.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            this.Start();
        }
        private void loop_timer_Tick(object sender, EventArgs e)
        {
            this.moveDots();
            this.moveLines();
        }
        private void moveLines()
        {
            if (this.list.Count != 0)
            {
                this.lastTick = Environment.TickCount;
                for (int i = this.list.Count - 1; i >= 0; i--)
                {
                    Colorful.Data data = this.list[i];
                    double num = (double)this.lastTick - (double)data.CurrentTime;
                    Ellipse ellipseObject = this.list[i].EllipseObject;
                    ellipseObject.Opacity -= 0.03;
                    if (ellipseObject.Height >= 0.3 && ellipseObject.Width >= 0.3)
                    {
                        ellipseObject.Height -= 0.3;
                        ellipseObject.Width -= 0.3;
                    }
                    if (1000.0 <= num)
                    {
                        base.Children.Remove(this.list[i].EllipseObject);
                        this.list[i].EllipseObject = null;
                        this.list.Remove(data);
                    }
                }
            }
        }
        private void moveDots()
        {
            for (int i = this._dotGroup.Count - 1; i >= 0; i--)
            {
                Dot dot = this._dotGroup[i];
                dot.RunFirework();
                if (dot.Opacity <= 0.1)
                {
                    base.Children.Remove(dot);
                    this._dotGroup.Remove(dot);
                }
            }
        }
        public virtual void Zouqi(double x, double y)
        {
            this.addDotToGroup(x, y);
            this.DrawLine(x, y);
        }
        private void addDotToGroup(double x, double y)
        {
            long arg_0E_0 = DateTime.Now.Ticks;
            int num = 0;
            while ((double)num < 1.0)
            {
                double size = 1.0 + 4.0 * this.random.NextDouble();
                double xVelocity = 2.5 - 5.0 * this.random.NextDouble();
                double yVelocity = -2.5 * this.random.NextDouble();
                Dot dot = new Dot((byte)(128.0 + 128.0 * this.random.NextDouble()), (byte)(128.0 + 128.0 * this.random.NextDouble()), (byte)(128.0 + 128.0 * this.random.NextDouble()), size);
                dot.X = x;
                dot.Y = y;
                dot.XVelocity = xVelocity;
                dot.YVelocity = yVelocity;
                dot.Gravity = 0.05;
                dot.RunFirework();
                this._dotGroup.Add(dot);
                base.Children.Add(dot);
                num++;
            }
        }
        private void DrawLine(double x, double y)
        {
            Colorful.Data data = new Colorful.Data();
            this.currentTick = Environment.TickCount;
            this.ellipse = new Ellipse();
            this.ellipse.Width = 10.0;
            this.ellipse.Height = 10.0;
            this.ellipse.Fill = Brushes.Red;
            this.ellipse.Opacity = 1.0;
            base.Children.Add(this.ellipse);
            this.ellipse.Margin = new Thickness(x, y, 0.0, 0.0);
            data.CurrentTime = this.currentTick;
            data.EllipseObject = this.ellipse;
            this.list.Add(data);
        }
        public virtual void Start()
        {
            this._timer = new DispatcherTimer();
            this._timer.Interval = TimeSpan.FromMilliseconds(20.0);
            this._timer.Tick += new EventHandler(this.loop_timer_Tick);
            this._timer.Start();
        }

        public virtual void Stop()
        {
            _timer.Stop();
            _timer.Tick -= new EventHandler(this.loop_timer_Tick);
        }
    }
}
