﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteboardProject.Common;
using WhiteboardProject.Model;

namespace WhiteboardProject.UC
{
    public class PopupEraserViewModel: BindingModelBase
    {
        #region Fileds
        public RelayCommand<object> BtnCommand { get; private set; }
        #endregion

        #region Property
        private string eraserName;

        public string EraserName
        {
            get { return eraserName; }
            set { eraserName = value; OnPropertyChanged(()=> EraserName); }
        }

        #endregion

        #region Constructure
        public PopupEraserViewModel()
        {
            this.BtnCommand = new RelayCommand<object>(BtnCommandExcute);
            this.EraserName = "橡皮擦";
        }
        #endregion

        #region Public Method
        public void BtnCommandExcute(object arg)
        {
            AppMessage am = new AppMessage();
            switch (arg)
            {
                //点擦除
                case "point":
                    am.MsgType = AppMsg.PointErase;
                    EventHub.SysEvents.PubEvent(am);
                    this.EraserName = "点擦除";
                    break;
                //选择擦除
                case "select":
                    am.MsgType = AppMsg.SelectErase;
                    EventHub.SysEvents.PubEvent(am);
                    this.EraserName = "区域擦除";
                    break;
                //清空
                case "clear":
                    am.MsgType = AppMsg.ClearErase;
                    EventHub.SysEvents.PubEvent(am);
                    this.EraserName = "清空";
                    break;
                //手势擦除
                case "gesture":
                    am.MsgType = AppMsg.GestureErase;
                    EventHub.SysEvents.PubEvent(am);
                    this.EraserName = "手势擦除";
                    break;
               
            }
        }
        #endregion
    }
}
