using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WhiteboardProject.Model;

namespace WhiteboardProject.UC
{
    public class PopupInteractiveViewModel: BindingModelBase
    {
        #region Fileds
        public RelayCommand<object> BtnCommand { get; private set; }
        #endregion

        #region Constructure
        public PopupInteractiveViewModel()
        {
            this.BtnCommand = new RelayCommand<object>(BtnCommandExcute);
        }
        #endregion

        #region Public Method
        public void BtnCommandExcute(object arg)
        {
            switch (arg.ToString())
            {
                case "Exit":
                    Process.GetCurrentProcess().Kill();
                    break;
            }
        }
        #endregion
    }
}
