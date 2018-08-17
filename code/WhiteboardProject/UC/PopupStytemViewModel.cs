using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WhiteboardProject.Common;
using WhiteboardProject.Model;

namespace WhiteboardProject.UC
{
    public class PopupStytemViewModel: BindingModelBase
    {
        #region Fileds
        public RelayCommand<object> BtnCommand { get; private set; }
        #endregion

        #region Property
        /// <summary>
        /// 显示文件菜单栏
        /// </summary>
        private Visibility isVisibilityTheme;

        public Visibility IsVisibilityTheme
        {
            get { return isVisibilityTheme; }
            set
            {
                isVisibilityTheme = value;
                OnPropertyChanged(() => IsVisibilityTheme);
            }
        }
        #endregion

        #region Constructure
        public PopupStytemViewModel()
        {
            this.BtnCommand = new RelayCommand<object>(BtnCommandExcute);
        }
        #endregion

        #region Public Method
        public void BtnCommandExcute(object arg)
        {
            AppMessage am = new AppMessage();
            am.MsgType = AppMsg.BaiBaoXiang;
            am.Tag = arg;
            EventHub.SysEvents.PubEvent<AppMessage>(am);
        }
        #endregion
    }
}
