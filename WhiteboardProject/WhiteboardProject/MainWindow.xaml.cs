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
            else if (appMessage.MsgType==AppMsg.Eraser)
            {
                for (int i = 0; i < this.gridTop.Children.Count; i++)
                {
                    Rectangle rectangle = this.gridTop.Children[i] as Rectangle;
                    if (rectangle != null)
                        this.gridTop.Children.Remove(rectangle);
                }
               
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
                Point p = e.MouseDevice.GetPosition(this);  //获取鼠标相对位置

                Rectangle rectangle = new Rectangle();
                rectangle.Width = 100;
                rectangle.Height = 50;
                rectangle.Fill = Brushes.Red;
                rectangle.HorizontalAlignment = HorizontalAlignment.Left;
                rectangle.VerticalAlignment = VerticalAlignment.Top;
                rectangle.Margin = new Thickness(p.X, p.Y, 0, 0);
                this.gridTop.Children.Add(rectangle);
            }
           
        }

        private void canvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
