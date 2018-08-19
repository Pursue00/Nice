using System;
using System.IO;
using System.Net;
using System.Windows;                       //STANDARD for WPF App
using System.Windows.Controls;              //STANDARD for WPF App
using System.Windows.Data;                  //STANDARD for WPF App
using System.Windows.Media.Animation;       //STANDARD for WPF App
using System.Windows.Navigation;            //STANDARD for WPF App
using System.Windows.Controls.Primitives;   //STANDARD for WPF App
using System.Windows.Media;                 //For : DrawingGroup
using System.Windows.Shapes;                //For : Geometric shapes like Line
using System.Windows.Input;                 //For : ExecutedRoutedEventArgs
using Microsoft.Win32;                      //For : OpenFileDialog / SaveFileDialog
using System.Windows.Ink;                   //For : InkCanvas
using System.Windows.Markup;                //For : XamlWriter
using System.Windows.Media.Imaging;         //For : BitmapImage etc etc
using System.Windows.Input.StylusPlugIns;
using System.Drawing;
using System.Windows.Interop;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Threading;   //For : DrawingAttributes
using GalaSoft.MvvmLight.Messaging;

namespace BitsOfStuff
{


	public partial class InkPadWindow :Window
	{
        // Make the pad 4 inches by 5 inches.
        public static readonly double widthCanvas=8*96;
        public static readonly double heightCanvas=5*96;
        string file_pre;
        public InkPadWindow(string sPath,string filestr)
		{
            file_pre = filestr;
            Commons.PrtScPath = sPath;
			this.InitializeComponent();

            this.Loaded+=InkPadWindow_Loaded;
		}
        private void InkPadWindow_Loaded(object obj,EventArgs e )
        {
            ShowInTaskbar = false;
            this.radInk.IsChecked = true;

            this.bt4.AddHandler(System.Windows.Controls.Button.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.bt4_MouseLeftButtonDown_1), true);
            this.bt4.AddHandler(System.Windows.Controls.Button.MouseLeftButtonUpEvent, new MouseButtonEventHandler(this.bt4_MouseLeftButtonUp_1), true);


            DrawingAttributes inkDA = new DrawingAttributes();
            
            inkDA.Width = 10;
            inkDA.Height =10;
            inkDA.Color = this.inkCanv.DefaultDrawingAttributes.Color;
            inkDA.IsHighlighter = this.inkCanv.DefaultDrawingAttributes.IsHighlighter;
            this.inkCanv.DefaultDrawingAttributes = inkDA;

            this.inkCanv.DefaultDrawingAttributes.Color = Colors.Red;
            //IntPtr hwnd = new WindowInteropHelper(this).Handle;
            //Commons.SetForegroundWindow(hwnd);
        }

        // File Save : display SaveFileDialog.
        private void btnSave_Click(object sender, RoutedEventArgs args)
        {
            
            Bitmap bmpScreenshot;
            Graphics gfxScreenshot;
            //first, we create&set a bitmap object to the size of the screen
            bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            //Using the bitmap, we create a graphics object
            gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            //Copy the rectangle (the screen in our case) - from the upper left corner to the right bottom corner
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);

            // Do the saving, outputing a file by supplying the name and format it as PNG
            bmpScreenshot.Save(Commons.PrtScPath + file_pre + "_" + DateTime.Now.ToString("yyyyMMddsshhmmss") + ".png", ImageFormat.Png);
            System.Windows.Point p=new System.Windows.Point();
            p.X=SystemParameters.PrimaryScreenWidth/2;
            p.Y=SystemParameters.PrimaryScreenHeight/2;
            Popwindow pw = new Popwindow(p);
            pw.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            pw.WindowStyle = System.Windows.WindowStyle.None;
            pw.content.Text = Commons.PrtScPath + file_pre + "_" + DateTime.Now.ToString("yyyyMMddsshhmmss") + ".png";
            pw.Show();
            //System.Windows.MessageBox.Show("图片已经保存！（" + Commons.PrtScPath + file_pre + "_" + DateTime.Now.ToString("yyyyMMddsshhmmss") + ".png" + "）", "上海信颐提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
       

        
        // Delete command: delete all selected strokes
        private void btnDelete_Click(object sender, RoutedEventArgs args)
        {
            if (this.inkCanv.GetSelectedStrokes().Count > 0)
            {
                foreach (Stroke strk in this.inkCanv.GetSelectedStrokes())
                    this.inkCanv.Strokes.Remove(strk); 
            }
        }

        // SelectAll command: select all strokes
        private void btnSelectAll_Click(object sender, RoutedEventArgs args)
        {
            this.inkCanv.Select(this.inkCanv.Strokes);
        }

        private void rad_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Image rad = sender as System.Windows.Controls.Image;
            this.inkCanv.EditingMode = (InkCanvasEditingMode)rad.Tag;
        }

       


        private void txtPopClose_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //this.popAbout.IsOpen = false;
        }



        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            GC.Collect();
            Messenger.Default.Send("Max");
        }


        private void penSize_Click(object sender, RoutedEventArgs e)
        {

            System.Windows.Controls.RadioButton rad = sender as System.Windows.Controls.RadioButton;
            DrawingAttributes inkDA = new DrawingAttributes();
            inkDA.Width = rad.FontSize;
            inkDA.Height = rad.FontSize;
            inkDA.Color = this.inkCanv.DefaultDrawingAttributes.Color;
            inkDA.IsHighlighter = this.inkCanv.DefaultDrawingAttributes.IsHighlighter;
            this.inkCanv.DefaultDrawingAttributes = inkDA;
            //this.expB.IsExpanded = false;
        }

        

        
        /// <summary>
        /// 画笔粗细选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            System.Windows.Controls.Image rad = sender as System.Windows.Controls.Image;
            if (int.Parse(rad.Tag.ToString()) ==15)
            {
                bt2.Tag = "6";
            }
            else
            {
                bt2.Tag = int.Parse(rad.Tag.ToString()) + 3;
                
            }
            switch (bt2.Tag.ToString())
            {
                case "6":
                    this.bt2.Source = new BitmapImage(new Uri("resources/img/Bj1.png", UriKind.RelativeOrAbsolute)); 
                    break;
                case "9":
                    this.bt2.Source = new BitmapImage(new Uri("resources/img/Bj2.png",UriKind.RelativeOrAbsolute)); 
                    break;
                case "12":
                    this.bt2.Source = new BitmapImage(new Uri("resources/img/Bj3.png", UriKind.RelativeOrAbsolute)); 
                    break;
                case "15":
                    this.bt2.Source = new BitmapImage(new Uri("resources/img/Bj4.png", UriKind.RelativeOrAbsolute)); 
                    break;
            }
            DrawingAttributes inkDA = new DrawingAttributes();
            inkDA.Width = int.Parse(rad.Tag.ToString());
            inkDA.Height = int.Parse(rad.Tag.ToString());
            inkDA.Color = this.inkCanv.DefaultDrawingAttributes.Color;
            inkDA.IsHighlighter = this.inkCanv.DefaultDrawingAttributes.IsHighlighter;
            this.inkCanv.DefaultDrawingAttributes = inkDA;
        }
        /// <summary>
        /// 画笔及颜色选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

            System.Windows.Controls.Image rad = sender as System.Windows.Controls.Image;
            if (rad.Tag.ToString() == "Blk")
            {
                this.inkCanv.DefaultDrawingAttributes.Color = Colors.Red;
                this.bt3.Source = new BitmapImage(new Uri("resources/img/hong.png", UriKind.RelativeOrAbsolute)); 
                 rad.Tag = "Red";
            }
            else if (rad.Tag.ToString() == "Red")
            {
                this.inkCanv.DefaultDrawingAttributes.Color = Colors.Yellow;
                this.bt3.Source = new BitmapImage(new Uri("resources/img/huang.png", UriKind.RelativeOrAbsolute)); 
                rad.Tag = "Yew";
            }
            else if (rad.Tag.ToString() == "Yew")
            {
                this.inkCanv.DefaultDrawingAttributes.Color = Colors.DarkGreen;
                this.bt3.Source = new BitmapImage(new Uri("resources/img/lv.png", UriKind.RelativeOrAbsolute)); 
                rad.Tag = "DarkGreen";
            }
            else if (rad.Tag.ToString() == "DarkGreen")
            {
                this.inkCanv.DefaultDrawingAttributes.Color = Colors.Black;
                this.bt3.Source = new BitmapImage(new Uri("resources/img/hei.png", UriKind.RelativeOrAbsolute)); 
                rad.Tag = "Blk";
            }
        }
        
        
        
        
        bool isdrag = false;
        private void bt4_MouseMove_2(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (isdrag == true)
            {
                System.Windows.Point p = e.GetPosition(null);
                this.cc.Margin = new Thickness(p.X, p.Y, 0, 0);
            }
        }
        

        private void bt4_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            isdrag = true;
        }

        private void bt4_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            isdrag = false;
        }

        private void bt4_Click_1(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Image rad = sender as System.Windows.Controls.Image;
            
            this.inkCanv.EditingMode =( (InkCanvasEditingMode)rad.Tag);
        }

        private void bt4_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            System.Windows.Controls.Image rad = sender as System.Windows.Controls.Image;

            this.inkCanv.EditingMode = ((InkCanvasEditingMode)rad.Tag);
        }

        private void bt3_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            System.Windows.Controls.Image rad = sender as System.Windows.Controls.Image;
            if (rad.Tag.ToString() == "Blk")
            {
                this.inkCanv.DefaultDrawingAttributes.Color = Colors.Red;
                this.bt3.Source = new BitmapImage(new Uri("resources/img/hong.png", UriKind.RelativeOrAbsolute));
                rad.Tag = "Red";
            }
            else if (rad.Tag.ToString() == "Red")
            {
                this.inkCanv.DefaultDrawingAttributes.Color = Colors.Yellow;
                this.bt3.Source = new BitmapImage(new Uri("resources/img/huang.png", UriKind.RelativeOrAbsolute));
                rad.Tag = "Yew";
            }
            else if (rad.Tag.ToString() == "Yew")
            {
                this.inkCanv.DefaultDrawingAttributes.Color = Colors.DarkGreen;
                this.bt3.Source = new BitmapImage(new Uri("resources/img/lv.png", UriKind.RelativeOrAbsolute));
                rad.Tag = "DarkGreen";
            }
            else if (rad.Tag.ToString() == "DarkGreen")
            {
                this.inkCanv.DefaultDrawingAttributes.Color = Colors.Black;
                this.bt3.Source = new BitmapImage(new Uri("resources/img/hei.png", UriKind.RelativeOrAbsolute));
                rad.Tag = "Blk";
            }
        }

        private void bt2_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            System.Windows.Controls.Image rad = sender as System.Windows.Controls.Image;
            if (int.Parse(rad.Tag.ToString()) == 15)
            {
                bt2.Tag = "6";
            }
            else
            {
                bt2.Tag = int.Parse(rad.Tag.ToString()) + 3;

            }
            switch (bt2.Tag.ToString())
            {
                case "6":
                    this.bt2.Source = new BitmapImage(new Uri("resources/img/Bj1.png", UriKind.RelativeOrAbsolute));
                    break;
                case "9":
                    this.bt2.Source = new BitmapImage(new Uri("resources/img/Bj2.png", UriKind.RelativeOrAbsolute));
                    break;
                case "12":
                    this.bt2.Source = new BitmapImage(new Uri("resources/img/Bj3.png", UriKind.RelativeOrAbsolute));
                    break;
                case "15":
                    this.bt2.Source = new BitmapImage(new Uri("resources/img/Bj4.png", UriKind.RelativeOrAbsolute));
                    break;
            }
            DrawingAttributes inkDA = new DrawingAttributes();
            inkDA.Width = int.Parse(rad.Tag.ToString());
            inkDA.Height = int.Parse(rad.Tag.ToString());
            inkDA.Color = this.inkCanv.DefaultDrawingAttributes.Color;
            inkDA.IsHighlighter = this.inkCanv.DefaultDrawingAttributes.IsHighlighter;
            this.inkCanv.DefaultDrawingAttributes = inkDA;
        }

        private void Image_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            System.Windows.Controls.Image rad = sender as System.Windows.Controls.Image;
            this.inkCanv.EditingMode = (InkCanvasEditingMode)rad.Tag;
        }

        private void Image_PreviewTouchDown_1(object sender, TouchEventArgs e)
        {
            Bitmap bmpScreenshot;
            Graphics gfxScreenshot;
            //first, we create&set a bitmap object to the size of the screen
            bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            //Using the bitmap, we create a graphics object
            gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            //Copy the rectangle (the screen in our case) - from the upper left corner to the right bottom corner
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);

            // Do the saving, outputing a file by supplying the name and format it as PNG
            bmpScreenshot.Save(Commons.PrtScPath + file_pre + "_" + DateTime.Now.ToString("yyyyMMddsshhmmss") + ".png", ImageFormat.Png);
            System.Windows.Point p = new System.Windows.Point();
            p.X = SystemParameters.PrimaryScreenWidth / 2;
            p.Y = SystemParameters.PrimaryScreenHeight / 2;
            Popwindow pw = new Popwindow(p);
            pw.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            pw.WindowStyle = System.Windows.WindowStyle.None;
            pw.content.Text = Commons.PrtScPath + file_pre + "_" + DateTime.Now.ToString("yyyyMMddsshhmmss") + ".png";
            pw.Show();
        }

        private void Image_PreviewTouchDown_2(object sender, TouchEventArgs e)
        {
            this.Close();
            GC.Collect();
        }

        private void Image_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            gridPPT.Visibility = Visibility.Visible;
        }

        Loading w1;
        private string _path = Environment.CurrentDirectory;
        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            this.img.Source = null;
            if (list.Items.Count > 0)
            {
                this.list.Items.Clear();
            }
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();

            var showDialog = openFileDialog.ShowDialog(this);
            if ((bool)showDialog)
            {
                w1 = new Loading();
                w1.Show();
                var fileName = openFileDialog.FileName;
                _path = string.Format(@"{0}\{1}", _path, DateTime.Now.ToString("yyyyMMddHHmmssms"));
                System.IO.Directory.CreateDirectory(_path);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            gridPPT.Visibility = Visibility.Collapsed;
        }
    }
}