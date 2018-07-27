using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
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
        public DrawingView()
        {
            InitializeComponent();
            vm = new DrawingViewModel();
            this.DataContext = vm;
            //this.InkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            EventHub.SysEvents.SubEvent<AppMessage>(OnRecMsg, Prism.Events.ThreadOption.UIThread);
        }

        private void OnRecMsg(AppMessage appMessage)
        {

            //else if (appMessage.MsgType == AppMsg.Hardpen)
            //{
            //    this.InkCanvas.EditingMode = InkCanvasEditingMode.None;
            //    DrawingAttributes drawingAttributes;
            //    //创建 DrawingAttributes 类的一个实例  

            //    drawingAttributes = new DrawingAttributes();

            //    //将 InkCanvas 的 DefaultDrawingAttributes 属性的值赋成创建的 DrawingAttributes 类的对象的引用  

            //    //InkCanvas 通过 DefaultDrawingAttributes 属性来获取墨迹的各种设置，该属性的类型为 DrawingAttributes 型  

            //    this.InkCanvas.DefaultDrawingAttributes = drawingAttributes;


            //    //设置 DrawingAttributes 的 Color 属性设置颜色  

            //    drawingAttributes.Color = Colors.Red;

            //    //利用 DrawingAttributes 的 Width 和 Height 属性来设置墨迹的宽度和高度  

            //    drawingAttributes.Width = 20;

            //    drawingAttributes.Height = 10;

            //    //利用 DrawingAttributes 的StylusTip 属性可以设置墨迹触笔的形状，默认值是 StylusTip.Ellipse  

            //    //将墨迹的宽度和高度都设置为稍大一些可以清楚的看到差别，如果较小则不太容易看出差别  

            //    drawingAttributes.StylusTip = StylusTip.Rectangle;


            //    //将 FitToCurve 属性设置为 true 会在你绘制完一次墨迹后自动利用贝塞尔曲线来对你的墨迹进行平滑处理  

            //    drawingAttributes.FitToCurve = true;


            //    //设置 IsHighlighter 属性为 true ，则墨迹显示的时候看上去像是荧光笔  

            //    //使 Stroke 变的透明一些
            //    drawingAttributes.IsHighlighter = false;

            //    //设置 IgnorePressure 属性为 true墨迹粗细会随压力的增大而增大  

            //    drawingAttributes.IgnorePressure = true;
            //}
            if (appMessage.MsgType == AppMsg.Highlighter || appMessage.MsgType == AppMsg.Seal)
            {
                this.InkCanvas.EditingMode = InkCanvasEditingMode.None;
            }
            else if (appMessage.MsgType == AppMsg.Softpen)
            {
                this.InkCanvas.EditingMode = InkCanvasEditingMode.Ink;
                Messenger.Default.Send("pack://application:,,,/Image/Brush/" + colorType + "/图层3.png");
            }
            else if (appMessage.MsgType == AppMsg.WritingBrush)
            {
                this.InkCanvas.EditingMode = InkCanvasEditingMode.Ink;
                //InkCanvas.DefaultDrawingAttributes.Color = Colors.SpringGreen;
                Messenger.Default.Send("pack://application:,,,/Image/Brush/" + colorType + "/图层4.png");
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
                this.InkCanvas.EditingMode = InkCanvasEditingMode.Select;
            }
            else if (appMessage.MsgType == AppMsg.ColorChanged)
            {
                Messenger.Default.Send("pack://application:,,,/Image/Brush/" + appMessage.Tag + "/图层1.png");
            }
        }

    }
}

