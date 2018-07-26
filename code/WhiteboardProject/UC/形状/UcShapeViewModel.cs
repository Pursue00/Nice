using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteboardProject.Model;

namespace WhiteboardProject.UC
{
   public class UcShapeViewModel: BindingModelBase
    {
        #region Property
        private string _PathData;

        public string PathData
        {
            get { return _PathData; }
            set { _PathData = value; OnPropertyChanged(()=> PathData); }
        }

        #endregion

        #region Constructure
        public UcShapeViewModel()
        {
           
        }
        #endregion
    }
}
