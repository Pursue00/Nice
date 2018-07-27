﻿using System;
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

        private string _StrokeColor;

        public string StrokeColor
        {
            get { return _StrokeColor; }
            set { _StrokeColor = value;OnPropertyChanged(()=> StrokeColor); }
        }

        #endregion

        #region Constructure
        public UcShapeViewModel()
        {
            StrokeColor = "#FF75C5EF";
        }
        #endregion
    }
}
