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
        int index = 0;
        #endregion

        #region Property
        private string number;

        public string Number
        {
            get { return number; }
            set { number = value; OnPropertyChanged(() => Number); }
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
                case "add":
                    index += 1;
                    break;
                case "previous":
                    index -= 1;
                    break;
                case "next":
                    index += 1;
                    break;
                case "writingbrush":

                    break;

            }
            this.Number = index.ToString();
        }
        #endregion
    }
}
