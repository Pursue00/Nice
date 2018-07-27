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

     

        private bool isSeal;
        private string strokeColor;
        Panel uIElementCollection;
        SealShapeEnum shape;
        string  shapeEnum;
       
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
                shape = (SealShapeEnum)appMessage.Tag;
                IsVisibilityColorfulFollow = Visibility.Collapsed;
            }
            else if (appMessage.MsgType == AppMsg.ClearErase)
            {
                for (int i = 0; i < this.uIElementCollection.Children.Count; i++)
                {
                    UserControl rectangle = this.uIElementCollection.Children[i] as UserControl;
                    if (rectangle != null)
                    {
                        if (rectangle.Tag == null) continue;
                        if (rectangle.Tag.ToString() == "Shape")
                            this.gridTop.Children.Remove(rectangle);
                    }

                }
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
                isSeal = false;
                shapeEnum =appMessage.Tell;

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
            else
            {
                UcShape ucShape = new UcShape();
                var vm = ucShape.DataContext as UcShapeViewModel;
                ucShape.Tag = "Shape";
                ucShape.HorizontalAlignment = HorizontalAlignment.Left;
                ucShape.VerticalAlignment = VerticalAlignment.Top;
                ucShape.Margin = new Thickness(p.X, p.Y, 0, 0);
                this.gridTop.Children.Add(ucShape);
                vm.PathData = GlobalUIConfig.IndicatorDescription[shapeEnum];
                vm.StrokeColor = strokeColor;


            }
            this.uIElementCollection = this.gridTop;
        }

        #region 绘制印章图形
        private void GetStar()
        {

        }
        #endregion
        private void canvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
