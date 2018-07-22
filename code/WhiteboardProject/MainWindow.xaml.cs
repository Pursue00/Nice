using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WhiteboardProject.Common;

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

        private bool isSeal;
        private int childCount;
        Panel uIElementCollection;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            IsVisibilityColorfulFollow = Visibility.Collapsed;
            EventHub.SysEvents.SubEvent<AppMessage>(OnRecMsg, Prism.Events.ThreadOption.UIThread);
        }

        private void OnRecMsg(AppMessage appMessage)
        {
            isSeal = false;
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
                IsVisibilityColorfulFollow = Visibility.Collapsed;
            }
            else if (appMessage.MsgType == AppMsg.ClearErase)
            {
                for (int i = 0; i < this.uIElementCollection.Children.Count; i++)
                {
                    Polygon rectangle = this.uIElementCollection.Children[i] as Polygon;
                    if (rectangle != null)
                        this.gridTop.Children.Remove(rectangle);
                }
            }
            else if (appMessage.MsgType == AppMsg.Softpen)
            {

            }
            else if (appMessage.MsgType==AppMsg.WritingBrush)
            {
                Messenger.Default.Send("pack://application:,,,/Image/Brush/重墨.png");
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

        private void gridTop_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isSeal)
            {
                Point bottom = e.MouseDevice.GetPosition(bnb);
                if (bottom.Y >= -20) return;
                Point p = e.MouseDevice.GetPosition(this);  //获取鼠标相对位置

                Polygon ply = new Polygon();

                ply.Fill = Brushes.Orange;

                ply.Stroke = Brushes.Black;

                ply.StrokeThickness = 1;

                ply.Points = new PointCollection();

                for (int i = 0; i < 5; i++)
                {
                    double angle = i * 4 * Math.PI / 5;
                    Point pt = new Point(50 * Math.Sin(angle),
                        -50 * Math.Cos(angle));
                    ply.Points.Add(pt);
                }
          
                ply.HorizontalAlignment = HorizontalAlignment.Left;
                ply.VerticalAlignment = VerticalAlignment.Top;
                ply.Margin = new Thickness(p.X, p.Y, 0, 0);
                this.gridTop.Children.Add(ply);
                this.uIElementCollection = this.gridTop;
            }

        }

        private void canvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
