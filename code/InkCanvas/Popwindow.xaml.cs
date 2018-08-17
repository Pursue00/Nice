using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace BitsOfStuff
{
    /// <summary>
    /// Interaction logic for Popwindow.xaml
    /// </summary>
    public partial class Popwindow : Window
    {
        Point Center;
        public Popwindow(Point xy)
        {
            Center = new Point();
            Center = xy;
            InitializeComponent();
            DataContext = this;
            Loaded += Popwindow_Loaded;
        }


        void Popwindow_Loaded(object sender, RoutedEventArgs e)
        {

            Loaded -= Popwindow_Loaded;
            SetSize();
            //Appear();
        }


        private void Appear()//出现
        {
            DoubleAnimation Ani = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(.5))
            };



            DoubleAnimation Rni = new DoubleAnimation()
            {
                From = 0,
                To = 720,
                Duration = new Duration(TimeSpan.FromSeconds(.5))

            };

            
       
            _scale.BeginAnimation(ScaleTransform.ScaleXProperty, Ani);
            _scale.BeginAnimation(ScaleTransform.ScaleYProperty, Ani);

            _rot.BeginAnimation(RotateTransform.AngleProperty, Rni);
        }
        private void UnloadedAppear()//消失
        {
            DoubleAnimation Ani = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(.5))
            };



            DoubleAnimation Rni = new DoubleAnimation()
            {
                From =720 ,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(.5))

            };

            _scale.BeginAnimation(ScaleTransform.ScaleXProperty, Ani);
            _scale.BeginAnimation(ScaleTransform.ScaleYProperty, Ani);
            _rot.BeginAnimation(RotateTransform.AngleProperty, Rni);
          
        }
        #region <<属性
        public ImageSource PictureSource
        {
            get { return (ImageSource)GetValue(PictureSourceProperty); }
            set { SetValue(PictureSourceProperty, value); }
        }
        public static readonly DependencyProperty PictureSourceProperty =
                   DependencyProperty.Register("PictureSource", typeof(ImageSource), typeof(Popwindow), new UIPropertyMetadata(null));


        public static readonly DependencyProperty MediaSourceProperty =
            DependencyProperty.Register("MediaSource", typeof(Uri), typeof(Popwindow), new UIPropertyMetadata(null));
        public  Uri MediaSource
                {
                    get { return (Uri)GetValue(MediaSourceProperty); }
                    set { SetValue(MediaSourceProperty, value); }
                }
               
                
       

        public string Introduce;

        public double X
        {
            set { Canvas.SetLeft(this, value); }
            get { return Canvas.GetLeft(this); }
        }
        public double Y
        {
            set { Canvas.SetTop(this, value); }
            get { return Canvas.GetTop(this); }
        }
        #endregion
        private void SetSize()
        {

        }

        private void Image_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            //Commons.StartPrtsc();
        }

        

        private void Image_PreviewTouchDown_1(object sender, TouchEventArgs e)
        {
            
        }

        private void Image_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Image_PreviewMouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.Content = null;
        }

        private void Image_PreviewTouchDown_2(object sender, TouchEventArgs e)
        {
            this.Content = null;
        }

        private void UserControl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //UnloadedAppear();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DoubleAnimation_Completed(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
