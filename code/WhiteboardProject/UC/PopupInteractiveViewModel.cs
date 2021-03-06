﻿using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WhiteboardProject.Common;
using WhiteboardProject.Model;
using WhiteboardProject.Views;

namespace WhiteboardProject.UC
{
    public class PopupInteractiveViewModel: BindingModelBase
    {
        #region Fileds
        public RelayCommand<object> BtnCommand { get; private set; }

        BackgroundWorker bw = new BackgroundWorker();
        private string srcFile, destFile;
        #endregion

        #region Property
        /// <summary>
        /// 显示文件菜单栏
        /// </summary>
        private Visibility isVisibilityFile;

        public Visibility IsVisibilityFile
        {
            get { return isVisibilityFile; }
            set { isVisibilityFile = value;
                OnPropertyChanged(()=> IsVisibilityFile); }
        }

        /// <summary>
        /// 是否显示导出
        /// </summary>
        private Visibility isVisibilityExport;

        public Visibility IsVisibilityExport
        {
            get { return isVisibilityExport; }
            set { isVisibilityExport = value;
                OnPropertyChanged(()=> IsVisibilityExport);
            }
        }

        /// <summary>
        /// 是否显示模式
        /// </summary>
        private Visibility isVisibilityMode;

        public Visibility IsVisibilityMode
        {
            get { return isVisibilityMode; }
            set
            {
                isVisibilityMode = value;
                OnPropertyChanged(() => IsVisibilityMode);
            }
        }

        #endregion

        #region Constructure
        public PopupInteractiveViewModel()
        {
            this.BtnCommand = new RelayCommand<object>(BtnCommandExcute);
            this.IsVisibilityFile = Visibility.Collapsed;
            this.IsVisibilityExport = Visibility.Collapsed;
            this.IsVisibilityMode = Visibility.Collapsed;
        }
        #endregion

        #region Public Method
        public void BtnCommandExcute(object arg)
        {
            AppMessage appMessage = new AppMessage();
            switch (arg.ToString())
            {
                case "save":
                case "new":
                case "open":
                case "saveas":
                case "pptassistant":
                    appMessage.MsgType = AppMsg.FileDealWith;
                    appMessage.Tag = arg;
                    EventHub.SysEvents.PubEvent<AppMessage>(appMessage);
                    break;
                case "exit":
                    Process.GetCurrentProcess().Kill();
                    break;
               
                case "file":
                    this.IsVisibilityExport = Visibility.Collapsed;
                    this.IsVisibilityFile = Visibility.Visible;
                    break;
                case "export":
                    this.IsVisibilityExport = Visibility.Visible;
                    this.IsVisibilityFile = Visibility.Collapsed;
                    break;
                case "mode":
                    this.IsVisibilityExport = Visibility.Collapsed;
                    this.IsVisibilityFile = Visibility.Collapsed;
                    this.IsVisibilityMode = Visibility.Visible;
                    break;
                case "pdf":
                case "pptx":
                case "png":
                case "doc":
                    appMessage.MsgType = AppMsg.ExportFile;
                    appMessage.Tag = arg;
                    EventHub.SysEvents.PubEvent<AppMessage>(appMessage);
                    break;
                case "setting":
                    Setting setting = new Setting();
                    setting.Owner = Application.Current.MainWindow;
                    setting.ShowDialog();
                    break;
                case "prepar":
                    Application.Current.MainWindow.WindowState = WindowState.Maximized;
                    Application.Current.MainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
                    Application.Current.MainWindow.ShowInTaskbar = true;
                    break;
                case "teaching":
                    Application.Current.MainWindow.ShowInTaskbar = false;
                    Application.Current.MainWindow.WindowState = WindowState.Maximized;
                    Application.Current.MainWindow.WindowStyle = WindowStyle.None;
                    
                    break;
                case "desktop":
                    Application.Current.MainWindow.WindowState = WindowState.Minimized;
                    BitsOfStuff.InkPadWindow INK = new BitsOfStuff.InkPadWindow(@"D:\", "1");
                    INK.Show();
                    break;
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string source = (e.Argument as string[])[0];
                string destinaton = (e.Argument as string[])[1];

                PdfSharp.Pdf.PdfDocument doc = new PdfSharp.Pdf.PdfDocument();
                doc.Pages.Add(new PdfPage());
                XGraphics xgr = XGraphics.FromPdfPage(doc.Pages[0]);
                XImage img = XImage.FromFile(source);

                xgr.DrawImage(img, 0, 0);
                doc.Save(destinaton);
                doc.Close();
                //success = true;
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           
        }
        #endregion
    }
}
