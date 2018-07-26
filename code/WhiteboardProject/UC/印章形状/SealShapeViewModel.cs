using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteboardProject.Common;
using WhiteboardProject.Model;

namespace WhiteboardProject.UC
{
   public class SealShapeViewModel
    {
        #region Fileds
        public RelayCommand<object> BtnCommand { get; private set; }
        #endregion


        #region Constructure
        public SealShapeViewModel()
        {
            this.BtnCommand = new RelayCommand<object>(BtnCommandExcute);
        }
        #endregion

        #region Public Method
        public void BtnCommandExcute(object arg)
        {
            AppMessage am = new AppMessage();
            am.MsgType = AppMsg.Seal;
            am.Tag = (SealShapeEnum)Enum.Parse(typeof(SealShapeEnum), arg.ToString());
            EventHub.SysEvents.PubEvent(am);
        }
        #endregion
    }
}
