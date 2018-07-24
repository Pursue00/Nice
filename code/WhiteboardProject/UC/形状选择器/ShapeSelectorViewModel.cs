using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteboardProject.Common;
using WhiteboardProject.Model;

namespace WhiteboardProject.UC
{
    public class ShapeSelectorViewModel
    {
        #region Fileds
        public RelayCommand<object> BtnCommand { get; private set; }
        #endregion

        #region Constructure
        public ShapeSelectorViewModel()
        {
            this.BtnCommand = new RelayCommand<object>(BtnCommandExcute);
        }
        #endregion

        #region Public Method
        public void BtnCommandExcute(object arg)
        {
            AppMessage am = new AppMessage();
            am.MsgType = AppMsg.ShapeChanged;
            am.Tag = arg;
            EventHub.SysEvents.PubEvent(am);
        }
        #endregion
    }
}
