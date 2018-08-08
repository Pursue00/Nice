using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;
using WhiteboardProject.Common;

namespace WhiteboardProject.Model
{
    public class CustomRenderingInkCanvas : InkCanvas
    {
        private CustomDynamicRenderer customRenderer = new CustomDynamicRenderer();
        public double StrokeWidth;
        public string _selectedColor;
        private bool isSoftBrush;
        private bool isCancel;
        private int index = 0;
        private int cancelIndex = 0;
        private string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "record");
        public CustomRenderingInkCanvas()
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
            Directory.CreateDirectory(path);
            StrokeWidth = 40;
            Messenger.Default.Register<double>(this, NotificationFunc);
            Messenger.Default.Register<string>(this, ColorPathNotificationFunc);
            Messenger.Default.Register<Color>(this, ColorNotificationFunc);
            customRenderer.StrokeWidth = StrokeWidth;
            //base.DynamicRenderer = this.customRenderer;
            SolidColorBrush sb = (SolidColorBrush)Brushes.Red;
            DrawingAttributes inkDA = new DrawingAttributes();
            inkDA.Width = 1;
            inkDA.Height = 1;
            inkDA.Color = sb.Color;
            inkDA.IsHighlighter = base.DefaultDrawingAttributes.IsHighlighter;
            EventHub.SysEvents.SubEvent<AppMessage>(OnRecMsg, Prism.Events.ThreadOption.UIThread);
            base.DefaultDrawingAttributes = inkDA;
        }

        private void OnRecMsg(AppMessage appMessage)
        {
            if (appMessage.MsgType == AppMsg.Softpen)
            {
                isSoftBrush = true;
            }
            else if (appMessage.MsgType == AppMsg.WritingBrush)
            {
                isSoftBrush = false;
                base.DynamicRenderer = this.customRenderer;
            }
            else if (appMessage.MsgType == AppMsg.BrushCancel)
            {
                isCancel = true;
                if (cancelIndex >= 0)
                {
                    switch (appMessage.Tag)
                    {
                        case "cancel":
                            LoadStrokes(cancelIndex -= 1);
                            break;
                        case "redo":
                            LoadStrokes(cancelIndex += 1);
                            break;
                    }
                }
               
            }
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
            //this.StrokeWidth = 22;
        }

        protected override void OnStrokeCollected(InkCanvasStrokeCollectedEventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(path);
            if (isCancel)
            {
                int currentIndex = dir.GetFiles().Length;
                for (int i = cancelIndex; i < currentIndex; i++)
                {
                    File.Delete(System.IO.Path.Combine(path, (i + 1).ToString() + "inkstrokes.isf"));
                }
                index = cancelIndex;
            }
            index++;
            //base.Strokes.Remove(e.Stroke);
            if (isSoftBrush)
            {
               
            }
            else
            {
                CustomStroke item = new CustomStroke(e.Stroke.StylusPoints);
                item.StrokeWidth = StrokeWidth;
                item._selectedColor = this._selectedColor;
                base.Strokes.Add(item);
                InkCanvasStrokeCollectedEventArgs args = new InkCanvasStrokeCollectedEventArgs(item);
                base.OnStrokeCollected(args);
            }

            using (FileStream fs = new FileStream(System.IO.Path.Combine(path, index.ToString() + "inkstrokes.isf"), FileMode.Create))
            {
                base.Strokes.Save(fs);
                fs.Close();
            }
            
            int fileNum = dir.GetFiles().Length;
            cancelIndex = fileNum;
        }

        void LoadStrokes(int index = 0)
        {
           
            //base.Strokes.EditingMode = InkCanvasEditingMode.Ink;
            if (index <= 0) return;
            string filePath = System.IO.Path.Combine(path, index.ToString() + "inkstrokes.isf");
            if (!File.Exists(filePath))
                return;
            base.Strokes.Clear();
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            StrokeCollection strokes = new StrokeCollection(fs);
            //this.InkCanvas.StrokeWidth = 22;
            foreach (var item in strokes)
            {
                CustomStroke customStroke = new CustomStroke(item.StylusPoints);
                customStroke.StrokeWidth = 23;
                //customStroke._selectedColor = "pack://application:,,,/Image/Brush/绿色/图层1.png";
                base.Strokes.Add(item);
            }
            //this.InkCanvas.Strokes = strokes;
            //Messenger.Default.Send("pack://application:,,,/Image/Brush/绿色/图层1.png");
            fs.Close();
         
        }

    }
}
