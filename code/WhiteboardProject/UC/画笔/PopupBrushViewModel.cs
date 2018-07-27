using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WhiteboardProject.Common;
using WhiteboardProject.Model;

namespace WhiteboardProject.UC
{
    public class PopupBrushViewModel:BindingModelBase
    {
        #region Fileds
        public RelayCommand<object> BtnCommand { get; private set; }
   
        #endregion

        #region Property
        private Visibility isVisibilityColor;

        public Visibility IsVisibilityColor    
        {
            get { return isVisibilityColor; }
            set { isVisibilityColor = value;OnPropertyChanged(() => IsVisibilityColor); }
        }

        private Visibility isVisibilityShape;

        public Visibility IsVisibilityShape
        {
            get { return isVisibilityShape; }
            set { isVisibilityShape = value; OnPropertyChanged(() => IsVisibilityShape); }
        }

        #endregion

        #region Constructure
        public PopupBrushViewModel()
        {
            this.IsVisibilityColor = Visibility.Visible;
            this.IsVisibilityShape = Visibility.Collapsed;
            this.BtnCommand = new RelayCommand<object>(BtnCommandExcute);
        }
        #endregion

        #region Public Method
        public void BtnCommandExcute(object arg)
        {
            AppMessage am = new AppMessage();
            switch (arg)
            {
                //画笔
                case "brush":
                    this.IsVisibilityColor = Visibility.Visible;
                    this.IsVisibilityShape = Visibility.Collapsed;
                    am.MsgType = AppMsg.Hardpen;
                    EventHub.SysEvents.PubEvent(am);
                    break;
                //毛笔
                case "writingbrush":
                    this.IsVisibilityColor = Visibility.Visible;
                    this.IsVisibilityShape = Visibility.Collapsed;
                    am.MsgType = AppMsg.WritingBrush;
                    EventHub.SysEvents.PubEvent(am);
                    break;
                //软笔
                case "softpen":
                    this.IsVisibilityColor = Visibility.Visible;
                    this.IsVisibilityShape = Visibility.Collapsed;
                    am.MsgType = AppMsg.Softpen;
                    EventHub.SysEvents.PubEvent(am);
                    break;
                //印章笔
                case "sealpen":
                    this.IsVisibilityColor = Visibility.Collapsed;
                    this.IsVisibilityShape = Visibility.Visible;
                    break;
                //荧光笔
                case "highlighter":
                    am.MsgType = AppMsg.Highlighter;
                    EventHub.SysEvents.PubEvent(am);
                    break;
            }
        }
        #endregion
    }
}
