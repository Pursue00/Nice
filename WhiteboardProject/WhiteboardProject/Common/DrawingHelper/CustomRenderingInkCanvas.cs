using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;

namespace WhiteboardProject.Model
{
    public class CustomRenderingInkCanvas : InkCanvas
    {
        private CustomDynamicRenderer customRenderer = new CustomDynamicRenderer();
        private double StrokeWidth;
        public string _selectedColor;
        public CustomRenderingInkCanvas()
        {
            StrokeWidth = 40;
            Messenger.Default.Register<double>(this, NotificationFunc);
            Messenger.Default.Register<string>(this, ColorPathNotificationFunc);
            Messenger.Default.Register<Color>(this, ColorNotificationFunc);
            customRenderer.StrokeWidth = StrokeWidth;
            base.DynamicRenderer = this.customRenderer;
            SolidColorBrush sb = (SolidColorBrush)Brushes.Black;
            DrawingAttributes inkDA = new DrawingAttributes();
            inkDA.Width = 1;
            inkDA.Height = 1;
            inkDA.Color = sb.Color;
            inkDA.IsHighlighter = base.DefaultDrawingAttributes.IsHighlighter;
            base.DefaultDrawingAttributes = inkDA;
        }

        private void NotificationFunc(double size)
        {
            StrokeWidth = size;
            customRenderer.StrokeWidth = StrokeWidth;
            DrawingAttributes inkDA = new DrawingAttributes();
            //inkDA.Width = StrokeWidth;
            //inkDA.Height = StrokeWidth;
            inkDA.Color = base.DefaultDrawingAttributes.Color;
            inkDA.IsHighlighter = base.DefaultDrawingAttributes.IsHighlighter;
            base.DefaultDrawingAttributes = inkDA;
        }

        private void ColorNotificationFunc(Color color)
        {
            DrawingAttributes inkDA = new DrawingAttributes();
            inkDA.Width = base.DefaultDrawingAttributes.Width;
            inkDA.Height = base.DefaultDrawingAttributes.Height;
            inkDA.Color = color;
            inkDA.IsHighlighter = base.DefaultDrawingAttributes.IsHighlighter;
            base.DefaultDrawingAttributes = inkDA;
        }
        private void ColorPathNotificationFunc(string color)
        {
            this._selectedColor = color;
            customRenderer._selectedColor = this._selectedColor;
        }

        protected override void OnStrokeCollected(InkCanvasStrokeCollectedEventArgs e)
        {
            base.Strokes.Remove(e.Stroke);
            CustomStroke item = new CustomStroke(e.Stroke.StylusPoints);
            item.StrokeWidth = StrokeWidth;
            item._selectedColor = this._selectedColor;
            base.Strokes.Add(item);
            InkCanvasStrokeCollectedEventArgs args = new InkCanvasStrokeCollectedEventArgs(item);
            base.OnStrokeCollected(args);

        }
    }
}
