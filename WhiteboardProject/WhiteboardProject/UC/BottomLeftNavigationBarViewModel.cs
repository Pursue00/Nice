﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteboardProject.Model;

namespace WhiteboardProject.UC
{
    public class BottomLeftNavigationBarViewModel:BindingModelBase
    {
        #region Fileds
        public RelayCommand<object> BtnCommand { get; private set; }
        #endregion

        #region Property
        private bool _isVisibilityInteractive;

        public bool IsVisibilityInteractive
        {
            get { return _isVisibilityInteractive; }
            set { _isVisibilityInteractive = value;NotifyChanged("IsVisibilityInteractive"); }
        }

        #endregion

        #region Property
        public BottomLeftNavigationBarViewModel()
        {
            this.BtnCommand = new RelayCommand<object>(BtnCommandExcute);
        }
        #endregion

        #region Public Method
        public void BtnCommandExcute(object arg)
        {
            switch (arg)
            {
                case "interactive":
                    this.IsVisibilityInteractive = true;
                    break;
            }
        }
        #endregion
    }
}
