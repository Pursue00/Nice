using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteboardProject.Common;
using WhiteboardProject.Model;

namespace WhiteboardProject.UC
{
    public class PopupBrushViewModel
    {
        #region Fileds
        public RelayCommand<object> BtnCommand { get; private set; }
        #endregion

        #region Constructure
        public PopupBrushViewModel()
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
                //画笔
                case "brush":
                    am.MsgType = AppMsg.Hardpen;
                    EventHub.SysEvents.PubEvent(am);
                    break;
                //毛笔
                case "writingbrush":
                    am.MsgType = AppMsg.WritingBrush;
                    EventHub.SysEvents.PubEvent(am);
                    break;
                //软笔
                case "softpen":
                    am.MsgType = AppMsg.Softpen;
                    EventHub.SysEvents.PubEvent(am);
                    break;
                //印章笔
                case "sealpen":
                    am.MsgType = AppMsg.Seal;
                    EventHub.SysEvents.PubEvent(am);
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
