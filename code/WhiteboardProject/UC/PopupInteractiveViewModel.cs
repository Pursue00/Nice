using PdfSharp.Drawing;
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
using WhiteboardProject.Model;

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

        #endregion

        #region Constructure
        public PopupInteractiveViewModel()
        {
            this.BtnCommand = new RelayCommand<object>(BtnCommandExcute);
            this.IsVisibilityFile = Visibility.Collapsed;
            this.IsVisibilityExport = Visibility.Collapsed;
        }
        #endregion

        #region Public Method
        public void BtnCommandExcute(object arg)
        {
            switch (arg.ToString())
            {
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
                case "pdf":
                    //image to pdf
                    bw.RunWorkerAsync(new string[2] { srcFile, destFile });
                    bw.DoWork += bw_DoWork;
                    bw.RunWorkerCompleted += bw_RunWorkerCompleted;
                    break;
                case "pptx":
                    break;
                case "picture":
                    break;
                case "word":
                    //pdf to word
                    Spire.Pdf.PdfDocument pdf = new Spire.Pdf.PdfDocument();
                    pdf.LoadFromFile(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CefSharp.pdf"));
                    string output = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output.doc");
                    pdf.SaveToFile(output, FileFormat.DOC);
                    System.Diagnostics.Process.Start(output);


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
