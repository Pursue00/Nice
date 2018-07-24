using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteboardProject.Common;
using WhiteboardProject.Model;

namespace WhiteboardProject.UC
{
    public class PopupShapeViewModel
    {
        #region Fileds
        public RelayCommand<object> BtnCommand { get; private set; }
        #endregion

        #region Constructure
        public PopupShapeViewModel()
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
                //点擦除
                case "point":
                    am.MsgType = AppMsg.PointErase;
                    EventHub.SysEvents.PubEvent(am);
                    break;
            }
        }
        #endregion
    }
}
