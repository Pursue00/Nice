using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteboardProject.Common;
using WhiteboardProject.Model;

namespace WhiteboardProject.UC
{
    public class BottomNavigationBarViewModel : BindingModelBase
    {
        #region Fileds
        public RelayCommand<object> BtnCommand { get; private set; }
        #endregion

        #region Property
        private bool _isVisibilityHardpen;
        /// <summary>
        /// 画笔
        /// </summary>

        public bool IsVisibilityHardpen
        {
            get { return _isVisibilityHardpen; }
            set { _isVisibilityHardpen = value; OnPropertyChanged(()=> IsVisibilityHardpen); }
        }

        private bool _isVisibilityEraser;
        /// <summary>
        /// 橡皮擦
        /// </summary>

        public bool IsVisibilityEraser
        {
            get { return _isVisibilityEraser; }
            set { _isVisibilityEraser = value; OnPropertyChanged(() => IsVisibilityEraser); }
        }

        private bool _isVisibilityShape;
        /// <summary>
        /// 形状
        /// </summary>

        public bool IsVisibilityShape
        {
            get { return _isVisibilityShape; }
            set { _isVisibilityShape = value; OnPropertyChanged(() => IsVisibilityShape); }
        }

        
        private bool _isVisibilityRoaming;
        /// <summary>
        /// 漫游
        /// </summary>

        public bool IsVisibilityRoaming
        {
            get { return _isVisibilityRoaming; }
            set { _isVisibilityRoaming = value; OnPropertyChanged(() => IsVisibilityRoaming); }
        }

        //private bool _isVisibilityHardpen;

        //public bool IsVisibilityHardpen
        //{
        //    get { return _isVisibilityHardpen; }
        //    set { _isVisibilityHardpen = value; OnPropertyChanged(() => IsVisibilityHardpen); }
        //}
        #endregion

        #region Constructure
        public BottomNavigationBarViewModel()
        {
            this.BtnCommand = new RelayCommand<object>(BtnCommandExcute);
        }
        #endregion

        #region Public Method
        public void BtnCommandExcute(object arg)
        {
            AppMessage am = new AppMessage();
            switch (arg)
            {
                case "arrow":
                   
                    break;
                case "hardpen":
                    this.IsVisibilityHardpen = true;
                    am.MsgType = AppMsg.Hardpen;
                    EventHub.SysEvents.PubEvent(am);
                    break;
                case "eraser":
                    this.IsVisibilityEraser = true;
                    am.MsgType = AppMsg.Eraser;
                    EventHub.SysEvents.PubEvent(am);
                    break;
                case "writingbrush":
                    this.IsVisibilityRoaming = true;
                    break;
                case "highlighter":
                 
                    am.MsgType = AppMsg.Highlighter;
                    EventHub.SysEvents.PubEvent(am);
                    break;
                case "seal":

                    am.MsgType = AppMsg.Seal;
                    EventHub.SysEvents.PubEvent(am);
                    break;
                case "return":
                  
                    break;
                case "reset":
                
                    break;
            }
        }
        #endregion
    }
}
