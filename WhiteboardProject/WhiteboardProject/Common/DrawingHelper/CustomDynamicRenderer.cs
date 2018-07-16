using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Input.StylusPlugIns;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WhiteboardProject.Model
{
    internal class CustomDynamicRenderer : DynamicRenderer
    {
        [ThreadStatic]
        private Point prevPoint;
        [ThreadStatic]
        public double StrokeWidth = 0.0;
        public string _selectedColor;
        protected override void OnDraw(DrawingContext drawingContext, StylusPointCollection stylusPoints, Geometry geometry, Brush fillBrush)
        {
            ImageSource imageSource = new BitmapImage(new Uri(_selectedColor));
            Point point = new Point(double.NegativeInfinity, double.NegativeInfinity);
            double num = this.StrokeWidth + 20.0;
            Point point2 = new Point(0.0, 0.0);
            Vector vector = new Vector();
            double num2 = 0.0;
            double num3 = 0.0;
            double strokeWidth = this.StrokeWidth;
            double x = 0.0;
            double y = 0.0;
            for (int i = 0; i < stylusPoints.Count; i++)
            {
                point2 = (Point)stylusPoints[i];
                vector = Point.Subtract(point, point2);
                num2 = (point2.Y - point.Y) / vector.Length;
                num3 = (point2.X - point.X) / vector.Length;
                if ((num - vector.Length) < this.StrokeWidth)
                {
                    strokeWidth = this.StrokeWidth;
                }
                else
                {
                    strokeWidth = num - vector.Length;
                }
                for (double j = 0.0; j < vector.Length; j++)
                {
                    x = 0.0;
                    y = 0.0;
                    if (((point.X == double.NegativeInfinity) || (point.Y == double.NegativeInfinity)) || ((double.PositiveInfinity == point.X) || (double.PositiveInfinity == point.Y)))
                    {
                        y = point2.Y;
                        x = point2.X;
                    }
                    else
                    {
                        y = point.Y + num2;
                        x = point.X + num3;
                    }
                    drawingContext.DrawImage(imageSource, new Rect(x - (strokeWidth / 2.0), y - (strokeWidth / 2.0), strokeWidth, strokeWidth));
                    point = new Point(x, y);
                    if (double.IsNegativeInfinity(vector.Length) || double.IsPositiveInfinity(vector.Length))
                    {
                        break;
                    }
                }
            }
            stylusPoints = null;
        }

        protected override void OnStylusDown(RawStylusInput rawStylusInput)
        {
            this.prevPoint = new Point(double.NegativeInfinity, double.NegativeInfinity);
            base.OnStylusDown(rawStylusInput);
        }
    }
}
