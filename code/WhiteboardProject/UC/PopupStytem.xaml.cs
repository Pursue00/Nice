using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WhiteboardProject.UC
{
    /// <summary>
    /// PopopStytem.xaml 的交互逻辑
    /// </summary>
    public partial class PopopStytem : UserControl
    {
        PopupStytemViewModel vm;
        public PopopStytem()
        {
            InitializeComponent();
            vm = new PopupStytemViewModel();
            this.DataContext = this;
        }
    }
}
