using System;
using System.Collections.Generic;
using System.Linq;
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

namespace WhiteboardProject.UC
{
    /// <summary>
    /// PopupRoaming.xaml 的交互逻辑
    /// </summary>
    public partial class PopupRoaming : UserControl
    {
        PopupRoamingViewModel vm;
        public PopupRoaming()
        {
            InitializeComponent();
            vm = new PopupRoamingViewModel();
            this.DataContext = vm;
        }

        private void imageClose_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AppMessage appMessage = new AppMessage();
            appMessage.MsgType = AppMsg.CloseCommand;
            EventHub.SysEvents.PubEvent(appMessage);
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AppMessage appMessage = new AppMessage();
            appMessage.MsgType = AppMsg.SliderValueChanged;
            appMessage.Tag = this.slider.Value;
            EventHub.SysEvents.PubEvent(appMessage);
        }
    }
}
