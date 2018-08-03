using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// UcShape.xaml 的交互逻辑
    /// </summary>
    public partial class UcShape : UserControl
    {
        UcShapeViewModel vm;
        public UcShape()
        {
            InitializeComponent();
            vm = new UcShapeViewModel();
            this.DataContext = vm;
        }
        bool isShousuo=true;
        private void ContextMenu_Click(object sender, RoutedEventArgs e)
        {
            if (isShousuo)
            {
                Selector.SetIsSelected(contentControl, false);
                this.menuItem.Header = "展开缩放";
                isShousuo = false;
            }
            else
            {
                Selector.SetIsSelected(contentControl, true);
                this.menuItem.Header = "隐藏缩放";
                isShousuo = true;
            }
        }
    }
}
