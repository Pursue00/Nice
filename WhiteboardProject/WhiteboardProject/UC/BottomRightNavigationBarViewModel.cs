using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteboardProject.Model;

namespace WhiteboardProject.UC
{
  public  class BottomRightNavigationBarViewModel: BindingModelBase
    {
        #region Fileds
        public RelayCommand<object> BtnCommand { get; private set; }
        #endregion

        #region Property
        private bool _isVisibilityHardpen;

        public bool IsVisibilityHardpen
        {
            get { return _isVisibilityHardpen; }
            set { _isVisibilityHardpen = value; OnPropertyChanged(() => IsVisibilityHardpen); }
        }

        #endregion

        #region Constructure
        public BottomRightNavigationBarViewModel()
        {
            this.BtnCommand = new RelayCommand<object>(BtnCommandExcute);
        }
        #endregion

        #region Public Method
        public void BtnCommandExcute(object arg)
        {
            switch (arg)
            {
                case "arrow":
                    this.IsVisibilityHardpen = true;
                    break;
                case "hardpen":
                    this.IsVisibilityHardpen = true;
                    break;
                case "eraser":
                    this.IsVisibilityHardpen = true;
                    break;
                case "writingbrush":
                    this.IsVisibilityHardpen = true;
                    break;
                case "highlighter":
                    this.IsVisibilityHardpen = true;
                    break;
                case "seal":
                    this.IsVisibilityHardpen = true;
                    break;
                case "return":
                    this.IsVisibilityHardpen = true;
                    break;
                case "reset":
                    this.IsVisibilityHardpen = true;
                    break;
            }
        }
        #endregion
    }
}
