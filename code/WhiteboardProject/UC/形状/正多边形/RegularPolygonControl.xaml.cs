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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WhiteboardProject.UC
{
    /// <summary>
    /// RegularPolygonControl.xaml 的交互逻辑
    /// </summary>
    public partial class RegularPolygonControl : UserControl
    {
        public RegularPolygonControl()
        {
            InitializeComponent();
        }

        private double radius = 20;

        private Brush selectBackground = new SolidColorBrush(Color.FromRgb(0xEB, 0x42, 0x00));

        private Brush unselectBackgroud = new SolidColorBrush(Color.FromRgb(0x99, 0x93, 0x93));

        /// <summary>
        /// 半径
        /// </summary>
        public double Radius
        {
            get
            {
                object result = GetValue(RadiusProperty);

                if (result == null)
                {
                    return radius;
                }

                return (double)result;
            }

            set
            {
                SetValue(RadiusProperty, value);

                this.InvalidateVisual();
            }
        }

        public static DependencyProperty RadiusProperty =
           DependencyProperty.Register("Radius", typeof(double), typeof(RegularPolygonControl), new UIPropertyMetadata());


        /// <summary>
        /// 选中颜色
        /// </summary>
        public Brush SelectBackground
        {
            get
            {
                object result = GetValue(SelectBackgroundProperty);

                if (result == null)
                {
                    return selectBackground;
                }

                return (Brush)result;
            }

            set
            {
                SetValue(SelectBackgroundProperty, value);

                //this.InvalidateVisual();
            }
        }

        public static DependencyProperty SelectBackgroundProperty =
           DependencyProperty.Register("SelectBackground", typeof(Brush), typeof(RegularPolygonControl), new UIPropertyMetadata());

        /// <summary>
        /// 未选中颜色
        /// </summary>
        public Brush UnSelectBackground
        {
            get
            {
                object result = GetValue(UnSelectBackgroundProperty);

                if (result == null)
                {
                    return unselectBackgroud;
                }

                return (Brush)result;
            }

            set
            {
                SetValue(UnSelectBackgroundProperty, value);
            }
        }

        public static DependencyProperty UnSelectBackgroundProperty =
           DependencyProperty.Register("UnSelectBackground", typeof(Brush), typeof(RegularPolygonControl), new UIPropertyMetadata());

        protected override void OnRender(System.Windows.Media.DrawingContext dc)
        {
            base.OnRender(dc);

            Point center = new Point();

            PointCollection Points = GetPolygonPoint(center, 40, 4);

            Canvas ca = new Canvas();

            Polygon plg = new Polygon();

            plg.Points = Points;

            plg.Stroke = Brushes.Green;

            plg.StrokeThickness = 2;

            plg.Fill = Brushes.Red;

            plg.FillRule = FillRule.Nonzero;

            ca.Children.Add(plg);

            ca.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

            ca.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;

            this.Content = ca;
        }


        /// <summary>
        ///根据半径和圆心确定N个点
        /// </summary>
        /// <param name="center"></param>
        /// <returns></returns>
        private PointCollection GetPolygonPoint(Point center, double r, int polygonBound)
        {
            double g = 18;

            double perangle = 360 / polygonBound;

            double pi = Math.PI;

            List<Point> values = new List<Point>();

            for (int i = 0; i < (int)polygonBound; i++)
            {
                Point p2 = new Point(r * Math.Cos((g + 36) * pi / 180), r * Math.Sin((g + 36) * pi / 180));

                values.Add(p2);

                g += perangle;
            }

            PointCollection pcollect = new PointCollection(values);

            return pcollect;
        }
    }
}
