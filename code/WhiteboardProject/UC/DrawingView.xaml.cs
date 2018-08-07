using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WhiteboardProject.Common;
using WhiteboardProject.Model;

namespace WhiteboardProject.UC
{
    /// <summary>
    /// DrawingView.xaml 的交互逻辑
    /// </summary>
    public partial class DrawingView : UserControl
    {
        DrawingViewModel vm;
        private string colorType = "黑色";
        private string brushType = string.Empty;
        private bool isBrush;
        public DrawingView()
        {
            InitializeComponent();
            vm = new DrawingViewModel();
            this.DataContext = vm;
            this.InkCanvas.EditingMode = InkCanvasEditingMode.None;
            EventHub.SysEvents.SubEvent<AppMessage>(OnRecMsg, Prism.Events.ThreadOption.UIThread);
        }

        private void OnRecMsg(AppMessage appMessage)
        {
            if (appMessage.MsgType == AppMsg.Highlighter || appMessage.MsgType == AppMsg.Seal || appMessage.MsgType == AppMsg.ShapeChanged)
            {
                isBrush = false;
                this.InkCanvas.EditingMode = InkCanvasEditingMode.None;
            }
            else if (appMessage.MsgType == AppMsg.Softpen)
            {
                isBrush = true;
                this.InkCanvas.EditingMode = InkCanvasEditingMode.Ink;
                if (Convert.ToDouble(appMessage.Tag) == 0)
                {
                    this.InkCanvas.DefaultDrawingAttributes.Width = 1;
                    this.InkCanvas.DefaultDrawingAttributes.Height = 1;
                }
                else
                {
                    this.InkCanvas.DefaultDrawingAttributes.Width = Convert.ToDouble(appMessage.Tag);
                    this.InkCanvas.DefaultDrawingAttributes.Height = Convert.ToDouble(appMessage.Tag);
                }
               
                this.InkCanvas.DefaultDrawingAttributes.Color = (Color)ColorConverter.ConvertFromString(GlobalUIConfig.ColorDescription[colorType]);
            }
            else if (appMessage.MsgType == AppMsg.WritingBrush)
            {
                isBrush = true;
                this.InkCanvas.EditingMode = InkCanvasEditingMode.Ink;
                //InkCanvas.DefaultDrawingAttributes.Color = Colors.SpringGreen;
                Messenger.Default.Send("pack://application:,,,/Image/Brush/" + colorType + "/图层4.png");
                this.InkCanvas.DefaultDrawingAttributes.Width = 1;
                this.InkCanvas.DefaultDrawingAttributes.Height = 1;

            }
            else if (appMessage.MsgType == AppMsg.SelectErase)
            {
                this.InkCanvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
            }
            else if (appMessage.MsgType == AppMsg.PointErase)
            {
                this.InkCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
            }
            else if (appMessage.MsgType == AppMsg.ClearErase)
            {
                /// this.InkCanvas.EditingMode = InkCanvasEditingMode.Select;
                this.InkCanvas.Strokes.Clear();
            }

            else if (appMessage.MsgType == AppMsg.ColorChanged)
            {
                if (isBrush)
                {
                    colorType = appMessage.Tag.ToString();
                    Messenger.Default.Send("pack://application:,,,/Image/Brush/" + colorType + "/图层1.png");
                    this.InkCanvas.DefaultDrawingAttributes.Color = (Color)ColorConverter.ConvertFromString(GlobalUIConfig.ColorDescription[colorType]);
                }
               
            }
            else if (appMessage.MsgType == AppMsg.FileDealWith)
            {
                switch (appMessage.Tag.ToString())
                {
                    case "save":
                        SaveStrokes();
                        break;
                    case "new":
                        break;
                    case "open":
                        //LoadStrokes();
                        break;
                    case "saveas":
                        break;

                }
            }
            else if (appMessage.MsgType == AppMsg.BrushSliderValueChanged)
            {
                this.InkCanvas.DefaultDrawingAttributes.Width = Convert.ToDouble(appMessage.Tag);
                this.InkCanvas.DefaultDrawingAttributes.Height = Convert.ToDouble(appMessage.Tag);
            }
        }

        void SaveStrokes()
        {
            using (FileStream fs = new FileStream(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"inkstrokes.isf"), FileMode.Create))
            {
                this.InkCanvas.Strokes.Save(fs);
                fs.Close();
            }
            HandWriting();
        }

        void LoadStrokes(int index=0)
        {
            this.InkCanvas.EditingMode = InkCanvasEditingMode.Ink;

            FileStream fs = new FileStream(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "record", index.ToString() + "inkstrokes.isf"), FileMode.Open, FileAccess.Read);
            StrokeCollection strokes = new StrokeCollection(fs);
            //this.InkCanvas.StrokeWidth = 22;
            foreach (var item in strokes)
            {
                CustomStroke customStroke = new CustomStroke(item.StylusPoints);
                customStroke.StrokeWidth = 23;
                //customStroke._selectedColor = "pack://application:,,,/Image/Brush/绿色/图层1.png";
                this.InkCanvas.Strokes.Add(item);
            }
            //this.InkCanvas.Strokes = strokes;
            //Messenger.Default.Send("pack://application:,,,/Image/Brush/绿色/图层1.png");
            fs.Close();
            
        }

        private void HandWriting()
        {
            MemoryStream ms = new MemoryStream();
            this.InkCanvas.Strokes.Save(ms);//canvas为InkCanvas
            Microsoft.Ink.Ink ink = new Microsoft.Ink.Ink();
            ink.Load(ms.ToArray());
            string[] resultArray = null;//存放识别的结果
            using (Microsoft.Ink.RecognizerContext myRecoContext = new Microsoft.Ink.RecognizerContext())
            {
                Microsoft.Ink.RecognitionStatus status;//识别的结果状态，可用于判断是否识别成功
                Microsoft.Ink.RecognitionResult recoResult;
                myRecoContext.Strokes = ink.Strokes;
                try
                {
                    recoResult = myRecoContext.Recognize(out status);
                    Microsoft.Ink.RecognitionAlternates als = recoResult.GetAlternatesFromSelection();
                    int resultCount = als.Count;
                    resultArray = new string[resultCount];
                    for (int i = 0; i < resultCount; i++)
                    {
                        resultArray[i] = als[i].ToString();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
        }

    }
}

