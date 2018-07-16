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
            if (appMessage.MsgType == AppMsg.Highlighter)
            {
                IsVisibilityColorfulFollow = Visibility.Visible;

            }
            else if(appMessage.MsgType==AppMsg.Eraser)
            {

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged<T>(Expression<Func<T>> expression)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(((MemberExpression)expression.Body).Member.Name));
            }
        }
    }
}
