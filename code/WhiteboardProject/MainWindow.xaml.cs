using GalaSoft.MvvmLight.Messaging;
//using Microsoft.Office.Core;
//using Microsoft.Office.Interop.PowerPoint;
using Microsoft.Win32;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WhiteboardProject.Common;
using WhiteboardProject.UC;
//using Application = Microsoft.Office.Interop.PowerPoint.Application;
namespace WhiteboardProject
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Application _application;
        private string _path = Environment.CurrentDirectory;

        private ObservableCollection<string> _images;
        Loading w1;

        #region Property
        private Visibility _IsVisibilityColorfulFollow;

        public Visibility IsVisibilityColorfulFollow
        {
            get { return _IsVisibilityColorfulFollow; }
            set { _IsVisibilityColorfulFollow = value;
                OnPropertyChanged(()=> IsVisibilityColorfulFollow);
            }
        }

        private double sliderValue;

        public double SliderValue
        {
            get { return sliderValue; }
            set { sliderValue = value;
                OnPropertyChanged(()=> SliderValue);
            }
        }
        
        private bool isSeal;
        private bool isShape;
        private string strokeColor;
        Panel uIElementCollection;
        SealShapeEnum shape;
        string  shapeEnum;
        private DoubleAnimation c_daListAnimation;
        public bool c_bState = true;//记录菜单栏状态 false隐藏 true显示
        private bool isdrag = false;
        private bool isExpand = false;
        #endregion
        BackgroundWorker bw = new BackgroundWorker();
        private string tempFilePath, localFilePath;

        public MainWindow()
        {
            InitializeComponent();
            //gridCanvas.PreviewMouseMove += canvas_MouseMove;
            //gridCanvas.PreviewMouseLeftButtonUp += canvas_MouseUp;
            //gridCanvas.PreviewMouseLeftButtonDown += canvas_MouseDown;
            this.DataContext = this;
            this.SliderValue = 1;
            this.Loaded += MainWindow_Loaded;
            IsVisibilityColorfulFollow = Visibility.Collapsed;
            EventHub.SysEvents.SubEvent<AppMessage>(OnRecMsg, Prism.Events.ThreadOption.UIThread);

            _images = new ObservableCollection<string>();
            //_application = new Application();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            c_daListAnimation = new DoubleAnimation();
        }

        private void OnRecMsg(AppMessage appMessage)
        {
            isSeal = false;
            isShape = false;
            if (appMessage.MsgType == AppMsg.Highlighter)
            {
                IsVisibilityColorfulFollow = Visibility.Visible;
            }
            else if (appMessage.MsgType == AppMsg.Hardpen)
            {
                IsVisibilityColorfulFollow = Visibility.Collapsed;
            }
            else if (appMessage.MsgType == AppMsg.Seal)
            {
                isSeal = true;
                shape = (SealShapeEnum)appMessage.Tag;
                IsVisibilityColorfulFollow = Visibility.Collapsed;
            }
            else if (appMessage.MsgType == AppMsg.ClearErase)
            {
                this.gridCanvas.Children.Clear();
                //for (int i = 0; i < this.uIElementCollection.Children.Count; i++)
                //{
                //    UserControl rectangle = this.uIElementCollection.Children[i] as UserControl;
                //    if (rectangle != null)
                //    {
                //        if (rectangle.Tag == null) continue;
                //        if (rectangle.Tag.ToString() == "Shape")
                //            this.gridCanvas.Children.Remove(rectangle);
                //    }

                //}

            }
            else if (appMessage.MsgType == AppMsg.Softpen)
            {
                IsVisibilityColorfulFollow = Visibility.Collapsed;
            }
            else if (appMessage.MsgType == AppMsg.WritingBrush)
            {
                IsVisibilityColorfulFollow = Visibility.Collapsed;
            }
            else if (appMessage.MsgType == AppMsg.ShapeChanged)
            {
                isShape = true;
                shapeEnum = appMessage.Tell;
            }
            else if (appMessage.MsgType == AppMsg.ColorChanged)
            {
                strokeColor = GlobalUIConfig.ColorDescription[appMessage.Tag.ToString()];
            }
            else if (appMessage.MsgType == AppMsg.SliderValueChanged)
            {
                this.SliderValue = Convert.ToDouble(appMessage.Tag);
            }
            else if (appMessage.MsgType == AppMsg.BottomLeftNavigation)
            {
                isExpand = true;
                switch (appMessage.Tag.ToString())
                {
                    case "interactive":
                        var popupInteractive = new PopupInteractive();
                        popupInteractive.HorizontalAlignment = HorizontalAlignment.Left;
                        popupInteractive.VerticalAlignment = VerticalAlignment.Bottom;
                        popupInteractive.Margin = new Thickness(0, 0, 0, 15);
                        OutLayer.Child = popupInteractive;

                        break;
                    case "system":
                        var popupSystem = new PopopStytem();
                        popupSystem.HorizontalAlignment = HorizontalAlignment.Left;
                        popupSystem.VerticalAlignment = VerticalAlignment.Bottom;
                        OutLayer.Child = popupSystem;
                        break;
                    case "pptassistant":
                        gridPPT.Visibility = Visibility.Visible;
                        break;
                    case "change":
                        this.WindowState = WindowState.Minimized;
                        BitsOfStuff.InkPadWindow INK = new BitsOfStuff.InkPadWindow(@"D:\", "1");
                        INK.Show();
                       
                        break;
                }
                OutLayer.Margin = new Thickness(-500, 0, 0, 0);
                OutLayer.Visibility = Visibility.Visible;
                var moveLeft = new ThicknessAnimation
                {
                    From = new Thickness(-500, 0, 0, 0),
                    To = new Thickness(0, 0, 0, 0),
                    Duration = TimeSpan.FromSeconds(0.3)
                };

                OutLayer.BeginAnimation(Border.MarginProperty, moveLeft);
            }
            else if (appMessage.MsgType == AppMsg.ExportFile)
            {
                string fileNameExt, newFileName, FilePath, filter, picturePath;
                filter = appMessage.Tag.ToString();
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
                    localFilePath = saveFileDialog1.FileName.ToString();
                    if (localFilePath.EndsWith("pdf"))
                        picturePath = localFilePath.Replace("pdf", "png");
                    else if (localFilePath.EndsWith("pptx"))
                        picturePath = localFilePath.Replace("pptx", "png");
                    else if (localFilePath.EndsWith("doc"))
                        picturePath = localFilePath.Replace("doc", "png");
                    else
                        picturePath = localFilePath;
                    //获取文件名，不带路径   
                    fileNameExt = localFilePath.Substring(localFilePath.LastIndexOf("\\") + 1);
                    //获取文件路径，不带文件名   
                    //string FilePath = localFilePath.Substring(0, localFilePath.LastIndexOf("\\"));   
                    //给文件名前加上时间   
                    newFileName = DateTime.Now.ToString("yyyyMMdd") + fileNameExt;
                    //System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();//输出文件 

                    RenderTargetBitmap rtb = new RenderTargetBitmap(Convert.ToInt32(gd_canvas.ActualWidth), Convert.ToInt32(gd_canvas.ActualHeight), 96, 96, PixelFormats.Pbgra32);
                    rtb.Render(gd_canvas);
                    //fs输出带文字或图片的文件，就看需求了 
                    PngBitmapEncoder png = new PngBitmapEncoder();
                    png.Frames.Add(BitmapFrame.Create(rtb));
                    using (Stream fs = File.Create(picturePath))
                    {
                        png.Save(fs);
                    }

                    switch (appMessage.Tag.ToString())
                    {
                        case "png":
                            MessageBox.Show("导出图片成功！");
                            break;
                        case "pdf":
                            //image to pdf
                            bw.RunWorkerAsync(new string[2] { picturePath, localFilePath });
                            bw.DoWork += bw_DoWork;
                            bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
                            break;
                        case "pptx":
                            break;
                       
                        case "doc":
                            //png to pdf
                            tempFilePath =  localFilePath.Replace("doc", "pdf");
                            bw.RunWorkerAsync(new string[2] { picturePath, tempFilePath });
                            bw.DoWork += bw_DoWork;
                            bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
                            break;
                    }
                }
            }
        }
        
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string source = (e.Argument as string[])[0];
                string destinaton = (e.Argument as string[])[1];

                PdfSharp.Pdf.PdfDocument doc = new PdfSharp.Pdf.PdfDocument();
                doc.Pages.Add(new PdfPage());
                XGraphics xgr = XGraphics.FromPdfPage(doc.Pages[0]);
                XImage img = XImage.FromFile(source);

                xgr.DrawImage(img, 0, 0);
                doc.Save(destinaton);
                doc.Close();
                //success = true;
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (localFilePath.EndsWith("pdf"))
                MessageBox.Show("导出pdf文件成功！");
            else
            {
                //pdf to word
                Spire.Pdf.PdfDocument pdf = new Spire.Pdf.PdfDocument();
                pdf.LoadFromFile(tempFilePath);
                //string output = System.IO.Path.Combine(picturePath, "output.doc");
                pdf.SaveToFile(localFilePath, FileFormat.DOC);
                //System.Diagnostics.Process.Start(output);
                MessageBox.Show("导出word文件成功！");
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
            base.OnKeyDown(e);
        }

        #region PPT助手
        /// <summary>
        /// 将ppt转成图片
        /// </summary>
        /// <param name="fileName"></param>
        //private void SaveToImages(object fileName)
        //{
        //    var presentation = _application.Presentations.Open((string)fileName, MsoTriState.msoFalse, MsoTriState.msoFalse,
        //                                                       MsoTriState.msoFalse);
        //    Console.WriteLine(presentation.Application.Version);
        //    presentation.SaveAs(_path, PpSaveAsFileType.ppSaveAsJPG, MsoTriState.msoTrue);
        //    presentation.Close();

        //    var files = System.IO.Directory.GetFiles(_path);
        //    if (files != null)
        //    {
        //        foreach (var f in files)
        //        {
        //            this.Dispatcher.Invoke(new Action(() =>
        //            {
        //                ImageUC uc = new ImageUC();
        //                uc.MouseLeftButtonDown += uc_MouseLeftButtonDown;
        //                uc.IMG.Source = new BitmapImage(new Uri(f, UriKind.RelativeOrAbsolute));

        //                list.Items.Add(uc);
        //            }));
        //        }
        //    }
        //    this.Dispatcher.Invoke(new Action(() =>
        //    {
        //        w1.Close();
        //    }));

        //}

        void uc_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            img.Source = (sender as ImageUC).IMG.Source;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            gridPPT.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.img.Source = null;
            if (list.Items.Count > 0)
            {
                this.list.Items.Clear();
            }
            var openFileDialog = new OpenFileDialog();

            var showDialog = openFileDialog.ShowDialog(this);
            if ((bool)showDialog)
            {
                w1 = new Loading();

                w1.Show();
                var fileName = openFileDialog.FileName;
                _path = string.Format(@"{0}\{1}", _path, DateTime.Now.ToString("yyyyMMddHHmmssms"));
                System.IO.Directory.CreateDirectory(_path);
                
                //ThreadPool.QueueUserWorkItem(new WaitCallback(SaveToImages), fileName);
            }
        }
        #endregion
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged<T>(Expression<Func<T>> expression)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(((MemberExpression)expression.Body).Member.Name));
            }
        }

        private void gridTop_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            
        }

        #region 绘制印章图形
        private void GetStar()
        {

        }
        #endregion
       

        private void DrawingView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isExpand)
            {
                ThicknessAnimation moveRight = new ThicknessAnimation
                {
                    From = new Thickness(0, 0, 0, 0),
                    To = new Thickness(-500, 0, 0, 0),
                    Duration = TimeSpan.FromSeconds(0.3)
                };
                moveRight.Completed -= OnMoveRightCompleted;
                moveRight.Completed += OnMoveRightCompleted;

                OutLayer.BeginAnimation(MarginProperty, moveRight);

            }

            this.isdrag = true;
            System.Windows.Point bottom = e.MouseDevice.GetPosition(bnb);
            if (bottom.Y >= -20) return;
            System.Windows.Point p = e.MouseDevice.GetPosition(this);  //获取鼠标相对位置
            if (isSeal)
            {
                switch (shape)
                {
                    case SealShapeEnum.Love:
                        Love love = new Love();
                        love.Tag = "Shape";
                        love.HorizontalAlignment = HorizontalAlignment.Left;
                        love.VerticalAlignment = VerticalAlignment.Top;
                        love.Margin = new Thickness(p.X - 105, p.Y - 90, 0, 0);
                        this.gridCanvas.Children.Add(love);
                        break;
                    case SealShapeEnum.MapleLeaf:
                        MapleLeaf mapleLeaf = new MapleLeaf();
                        mapleLeaf.Tag = "Shape";
                        mapleLeaf.HorizontalAlignment = HorizontalAlignment.Left;
                        mapleLeaf.VerticalAlignment = VerticalAlignment.Top;
                        mapleLeaf.Margin = new Thickness(p.X - 80, p.Y - 117, 0, 0);
                        this.gridCanvas.Children.Add(mapleLeaf);
                        break;
                    case SealShapeEnum.Smiley:
                        Smile smile = new Smile();
                        smile.Tag = "Shape";
                        smile.HorizontalAlignment = HorizontalAlignment.Left;
                        smile.VerticalAlignment = VerticalAlignment.Top;
                        smile.Margin = new Thickness(p.X - 100, p.Y - 100, 0, 0);
                        this.gridCanvas.Children.Add(smile);
                        break;
                    case SealShapeEnum.Sun:
                        Sun sun = new Sun();
                        sun.Tag = "Shape";
                        sun.HorizontalAlignment = HorizontalAlignment.Left;
                        sun.VerticalAlignment = VerticalAlignment.Top;
                        sun.Margin = new Thickness(p.X - 85, p.Y - 85, 0, 0);
                        this.gridCanvas.Children.Add(sun);
                        break;
                    case SealShapeEnum.Star:
                        Star star = new Star();
                        star.Tag = "Shape";
                        star.HorizontalAlignment = HorizontalAlignment.Left;
                        star.VerticalAlignment = VerticalAlignment.Top;
                        star.Margin = new Thickness(p.X - 55.4, p.Y - 55, 0, 0);
                        this.gridCanvas.Children.Add(star);
                        //this.uIElementCollection = this.gridCanvas;
                        break;
                    default:
                        break;
                }
            }
            if (isShape)
            {
                UcShape ucShape = new UcShape();
                var vm = ucShape.DataContext as UcShapeViewModel;
                if (GlobalUIConfig.IndicatorDescription.Keys.Contains(shapeEnum))
                    vm.PathData = GlobalUIConfig.IndicatorDescription[shapeEnum];

                if (!String.IsNullOrEmpty(strokeColor))
                    vm.StrokeColor = strokeColor;
                ucShape.Tag = "Shape";
                ucShape.HorizontalAlignment = HorizontalAlignment.Left;
                ucShape.VerticalAlignment = VerticalAlignment.Top;
                ucShape.Margin = new Thickness(p.X - 40, p.Y - 40, 0, 0);
                this.gridCanvas.Children.Add(ucShape);
                //this.gridCanvas.
            }
            //this.uIElementCollection = this.gridCanvas;
        }

        private void gridTop_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
          
        }

        private void OnMoveRightCompleted(object sender, EventArgs e)
        {
            OutLayer.Visibility = Visibility.Collapsed;
            OutLayer.Child = null;
            isExpand = false;
        }
        private void btnshrink_Click(object sender, RoutedEventArgs e)
        {
            //if (c_bState)
            //{

            //    c_bState = false;
            //    c_daListAnimation.From = 0;
            //    c_daListAnimation.To = -154;
            //    popInteractive.Width = 200;
            //}
            //else
            //{
            //    c_bState = true;
            //    c_daListAnimation.From = -154;
            //    c_daListAnimation.To = 0;
            //    popInteractive.Width =0;
            //}
        }
        private System.Windows.Shapes.Path elip = new System.Windows.Shapes.Path();
        private System.Windows.Point anchorPoint;
        #region 绘画集合图形
        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //capture the mouse on the canvas
            //(this also helps us keep track of whether or not we're drawing)
            gridCanvas.CaptureMouse();

            //Path pathFromCode = XamlReader.Load($"<Path xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'><Path.Data>{'M15,0 L60,0 75,30 0,30 z'}</Path.Data></Path>") as Path;

            anchorPoint = e.MouseDevice.GetPosition(gridCanvas);
            elip = new System.Windows.Shapes.Path
            {
                Data = Geometry.Parse("M15,0 L60,0 75,30 0,30 z"),
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            gridCanvas.Children.Add(elip);
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            //if we are not drawing, we don't need to do anything when the mouse moves
            if (!gridCanvas.IsMouseCaptured)
                return;

            System.Windows.Point location = e.MouseDevice.GetPosition(gridCanvas);

            double minX = Math.Min(location.X, anchorPoint.X);
            double minY = Math.Min(location.Y, anchorPoint.Y);
            double maxX = Math.Max(location.X, anchorPoint.X);
            double maxY = Math.Max(location.Y, anchorPoint.Y);
            elip.HorizontalAlignment = HorizontalAlignment.Left;
            elip.VerticalAlignment = VerticalAlignment.Top;
            elip.Margin = new Thickness(minX, minY, 0, 0);
            //Canvas.SetTop(elip, minY);
            //Canvas.SetLeft(elip, minX);

            double height = maxY - minY;
            double width = maxX - minX;
            //Canvas.SetTop(thumb1, minY + 2);
            //Canvas.SetLeft(thumb1, minX);

            //Canvas.SetTop(thumb2, minY - Math.Abs(height));
            //Canvas.SetLeft(thumb2, minX);

            //Canvas.SetTop(thumb3, minY);
            //Canvas.SetLeft(thumb3, minX + Math.Abs(width));

            //Canvas.SetTop(thumb4, minY);
            //Canvas.SetLeft(thumb4, minX + Math.Abs(width));

            elip.Height = Math.Abs(minY - Math.Abs(height));
            elip.Width = Math.Abs(width);
        }

        private void OutLayer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // we are now no longer drawing
            gridCanvas.ReleaseMouseCapture();
        }
        #endregion
    }
}
