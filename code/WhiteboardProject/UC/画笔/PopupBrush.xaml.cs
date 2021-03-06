﻿using GalaSoft.MvvmLight.Messaging;
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
    /// PopupBrush.xaml 的交互逻辑
    /// </summary>
    public partial class PopupBrush : UserControl
    {
        PopupBrushViewModel vm;
        public PopupBrush()
        {
            InitializeComponent();
            vm = new PopupBrushViewModel();
            this.DataContext = vm;
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Messenger.Default.Send(this.slider.Value);
            AppMessage appMessage = new AppMessage();
            appMessage.Tag = this.slider.Value;
            if (vm != null)
                vm.currentSliderValue = this.slider.Value;
            appMessage.MsgType = AppMsg.BrushSliderValueChanged;
            EventHub.SysEvents.PubEvent<AppMessage>(appMessage);
        }

        private void imageClose_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AppMessage appMessage = new AppMessage();
            appMessage.MsgType = AppMsg.CloseCommand;
            EventHub.SysEvents.PubEvent(appMessage);
        }
    }
}
