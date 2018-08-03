using GalaSoft.MvvmLight.Messaging;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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

namespace WhiteboardProject
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
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
        private string srcFile, destFile;
        public MainWindow()
        {
            InitializeComponent();
            //gridTop.PreviewMouseMove += canvas_MouseMove;
            //gridTop.PreviewMouseLeftButtonUp += canvas_MouseUp;
            //gridTop.PreviewMouseLeftButtonDown += canvas_MouseDown;
            this.DataContext = this;
            this.Loaded += MainWindow_Loaded;
            IsVisibilityColorfulFollow = Visibility.Collapsed;
            EventHub.SysEvents.SubEvent<AppMessage>(OnRecMsg, Prism.Events.ThreadOption.UIThread);
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
                //            this.gridTop.Children.Remove(rectangle);
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
                string localFilePath, fileNameExt, newFileName, FilePath, filter, picturePath;
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
                    else if (localFilePath.EndsWith("word"))
                        picturePath = localFilePath.Replace("word", "png");
                    else
                        picturePath = localFilePath;
                    //获取文件名，不带路径   
                    fileNameExt = localFilePath.Substring(localFilePath.LastIndexOf("\\") + 1);
                    //获取文件路径，不带文件名   
                    //string FilePath = localFilePath.Substring(0, localFilePath.LastIndexOf("\\"));   
                    //给文件名前加上时间   
                    newFileName = DateTime.Now.ToString("yyyyMMdd") + fileNameExt;
                    //System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();//输出文件 

                    RenderTargetBitmap rtb = new RenderTargetBitmap(Convert.ToInt32(drawingView.ActualWidth), Convert.ToInt32(drawingView.ActualHeight), 96, 96, PixelFormats.Pbgra32);
                    rtb.Render(drawingView);
                    //fs输出带文字或图片的文件，就看需求了 
                    PngBitmapEncoder png = new PngBitmapEncoder();
                    png.Frames.Add(BitmapFrame.Create(rtb));
                    using (Stream fs = File.Create(picturePath))
                    {
                        png.Save(fs);
                    }

                    switch (appMessage.Tag.ToString())
                    {
                        //case "picture":
                           
                        //    break;
                        case "pdf":
                            //image to pdf
                          
                            bw.RunWorkerAsync(new string[2] { picturePath, localFilePath });
                            bw.DoWork += bw_DoWork;
                            bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
                            break;
                        case "pptx":
                            break;
                       
                        case "word":
                            //pdf to word
                            Spire.Pdf.PdfDocument pdf = new Spire.Pdf.PdfDocument();
                            pdf.LoadFromFile(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CefSharp.pdf"));
                            string output = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output.doc");
                            pdf.SaveToFile(output, FileFormat.DOC);
                            System.Diagnostics.Process.Start(output);
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
            throw new NotImplementedException();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
            base.OnKeyDown(e);
        }
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
            Point bottom = e.MouseDevice.GetPosition(bnb);
            if (bottom.Y >= -20) return;
            Point p = e.MouseDevice.GetPosition(this);  //获取鼠标相对位置
            if (isSeal)
            {
                switch (shape)
                {
                    case SealShapeEnum.Love:
                        Love love = new Love();
                        love.Tag = "Shape";
                        love.HorizontalAlignment = HorizontalAlignment.Left;
                        love.VerticalAlignment = VerticalAlignment.Top;
                        love.Margin = new Thickness(p.X, p.Y, 0, 0);
                        this.gridTop.Children.Add(love);
                        break;
                    case SealShapeEnum.MapleLeaf:
                        MapleLeaf mapleLeaf = new MapleLeaf();
                        mapleLeaf.Tag = "Shape";
                        mapleLeaf.HorizontalAlignment = HorizontalAlignment.Left;
                        mapleLeaf.VerticalAlignment = VerticalAlignment.Top;
                        mapleLeaf.Margin = new Thickness(p.X, p.Y, 0, 0);
                        this.gridTop.Children.Add(mapleLeaf);
                        break;
                    case SealShapeEnum.Smiley:
                        Smile smile = new Smile();
                        smile.Tag = "Shape";
                        smile.HorizontalAlignment = HorizontalAlignment.Left;
                        smile.VerticalAlignment = VerticalAlignment.Top;
                        smile.Margin = new Thickness(p.X, p.Y, 0, 0);
                        this.gridTop.Children.Add(smile);
                        break;
                    case SealShapeEnum.Sun:
                        Sun sun = new Sun();
                        sun.Tag = "Shape";
                        sun.HorizontalAlignment = HorizontalAlignment.Left;
                        sun.VerticalAlignment = VerticalAlignment.Top;
                        sun.Margin = new Thickness(p.X, p.Y, 0, 0);
                        this.gridTop.Children.Add(sun);
                        break;
                    case SealShapeEnum.Star:
                        Star star = new Star();
                        star.Tag = "Shape";
                        star.HorizontalAlignment = HorizontalAlignment.Left;
                        star.VerticalAlignment = VerticalAlignment.Top;
                        star.Margin = new Thickness(p.X, p.Y, 0, 0);
                        this.gridTop.Children.Add(star);
                        //this.uIElementCollection = this.gridTop;
                        break;
                    default:
                        break;
                }
            }
            if (isShape)
            {
                UcShape ucShape = new UcShape();
                var vm = ucShape.DataContext as UcShapeViewModel;
                vm.PathData = GlobalUIConfig.IndicatorDescription[shapeEnum];
             
                if (!String.IsNullOrEmpty(strokeColor))
                    vm.StrokeColor = strokeColor;
                ucShape.Tag = "Shape";
                ucShape.HorizontalAlignment = HorizontalAlignment.Left;
                ucShape.VerticalAlignment = VerticalAlignment.Top;
                ucShape.Margin = new Thickness(p.X + 40, p.Y + 40, 0, 0);
                this.gridCanvas.Children.Add(ucShape);
            }
            //this.uIElementCollection = this.gridTop;
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
        private Point anchorPoint;
        #region 绘画集合图形
        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //capture the mouse on the canvas
            //(this also helps us keep track of whether or not we're drawing)
            gridTop.CaptureMouse();

            //Path pathFromCode = XamlReader.Load($"<Path xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'><Path.Data>{'M15,0 L60,0 75,30 0,30 z'}</Path.Data></Path>") as Path;

            anchorPoint = e.MouseDevice.GetPosition(gridTop);
            elip = new System.Windows.Shapes.Path
            {
                Data = Geometry.Parse("M15,0 L60,0 75,30 0,30 z"),
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            gridTop.Children.Add(elip);
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            //if we are not drawing, we don't need to do anything when the mouse moves
            if (!gridTop.IsMouseCaptured)
                return;

            Point location = e.MouseDevice.GetPosition(gridTop);

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

        private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // we are now no longer drawing
            gridTop.ReleaseMouseCapture();
        }
        #endregion
    }
}
