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
                    case "open":
                        LoadData();
                        break;
                    case "save":
                    case "saveas":
                        SaveData();
                        break;
                    case "new":
                        if (this.InkCanvas.Strokes.Count > 0)
                        {
                            System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("当前有未保存的内容，是否保存？", "提示信息", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                            if (dialogResult.ToString() == "OK")
                            {
                                SaveData();
                            }
                            else
                                this.InkCanvas.Strokes.Clear();
                        }

                        break;
                  
                }
            }
            else if (appMessage.MsgType == AppMsg.BrushSliderValueChanged)
            {
                this.InkCanvas.DefaultDrawingAttributes.Width = Convert.ToDouble(appMessage.Tag);
                this.InkCanvas.DefaultDrawingAttributes.Height = Convert.ToDouble(appMessage.Tag);
            }
        }

        private void SaveData()
        {
            string filter = ".isf";
            System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            //设置文件类型   
            saveFileDialog1.Filter = " " + filter + " files(*." + filter + ")|*." + filter + "|All files(*.*)|*.*";
            //设置默认文件类型显示顺序   
            saveFileDialog1.FilterIndex = 1;
            //保存对话框是否记忆上次打开的目录   
            saveFileDialog1.RestoreDirectory = true;
            //点了保存按钮进入   
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //获得文件路径   
                string localFilePath = saveFileDialog1.FileName.ToString();
                if (File.Exists(localFilePath))
                    LoadStrokes(localFilePath);
                else
                    SaveStrokes(localFilePath);
            }
        }

        private void LoadData()
        {
            string filter = ".isf";
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            //设置文件类型   
            openFileDialog.Filter = " " + filter + " files(*." + filter + ")|*." + filter + "|All files(*.*)|*.*";
            //设置默认文件类型显示顺序   
            openFileDialog.FilterIndex = 1;
            //保存对话框是否记忆上次打开的目录   
            openFileDialog.RestoreDirectory = true;
            //点了保存按钮进入   
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //获得文件路径   
                string localFilePath = openFileDialog.FileName.ToString();
                if (File.Exists(localFilePath))
                    LoadStrokes(localFilePath);
              
            }
        }

        void SaveStrokes(string filepath)
        {
            using (FileStream fs = new FileStream(filepath, FileMode.Create))
            {
                this.InkCanvas.Strokes.Save(fs);
                fs.Close();
            }
           
        }

        void LoadStrokes(string filepath,int index=0)
        {
            this.InkCanvas.EditingMode = InkCanvasEditingMode.Ink;

            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
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

