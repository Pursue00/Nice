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

        public bool IsVisibilityHardpen
        {
            get { return _isVisibilityHardpen; }
            set { _isVisibilityHardpen = value; OnPropertyChanged(()=> IsVisibilityHardpen); }
        }

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
                   
                    am.MsgType = AppMsg.Eraser;
                    EventHub.SysEvents.PubEvent(am);
                    break;
                case "writingbrush":
                  
                    break;
                case "highlighter":
                 
                    am.MsgType = AppMsg.Highlighter;
                    EventHub.SysEvents.PubEvent(am);
                    break;
                case "seal":
                 
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
