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
        public double currentSliderValue;
        #endregion

        #region Property
        private string brushName;

        public string BrushName
        {
            get { return brushName; }
            set { brushName = value; OnPropertyChanged(()=> BrushName); }
        }

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
            this.BrushName = "画笔";
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
                    this.BrushName = "毛笔";
                    this.IsVisibilityColor = Visibility.Visible;
                    this.IsVisibilityShape = Visibility.Collapsed;
                    am.MsgType = AppMsg.WritingBrush;
                    EventHub.SysEvents.PubEvent(am);
                    break;
                //软笔
                case "softpen":
                    this.BrushName = "软笔";
                    this.IsVisibilityColor = Visibility.Visible;
                    this.IsVisibilityShape = Visibility.Collapsed;
                    am.MsgType = AppMsg.Softpen;
                    am.Tag = currentSliderValue;
                    EventHub.SysEvents.PubEvent(am);
                    break;
                //印章笔
                case "sealpen":
                    this.BrushName = "印章笔";
                    this.IsVisibilityColor = Visibility.Collapsed;
                    this.IsVisibilityShape = Visibility.Visible;
                    break;
                //荧光笔
                case "highlighter":
                    this.BrushName = "荧光笔";
                    am.MsgType = AppMsg.Highlighter;
                    EventHub.SysEvents.PubEvent(am);
                    break;
            }
        }
        #endregion
    }
}
