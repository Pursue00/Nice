using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteboardProject.Model;

namespace WhiteboardProject.UC
{
    public class DrawingViewModel: BindingModelBase
    {
        #region Property
        private string _ImageBrushSource;

        public string ImageBrushSource
        {
            get { return _ImageBrushSource; }
            set { _ImageBrushSource = value; }
        }

        #endregion

        #region Constructure
        public DrawingViewModel()
        {
           
        }
        #endregion
    }
}
