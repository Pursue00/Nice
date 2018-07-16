using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WhiteboardProject.Common
{
    public class ColorfulFollow : Colorful
    {
        public ColorfulFollow()
        {
            base.Cursor = Cursors.None;
            base.MouseMove += new MouseEventHandler(this.Colorful_FollowMouse_MouseMove);
        }
        private void Colorful_FollowMouse_MouseMove(object sender, MouseEventArgs e)
        {
            this.Zouqi(e.GetPosition(this).X, e.GetPosition(this).Y);
        }

        public override void Stop()
        {
            base.MouseMove -= new MouseEventHandler(this.Colorful_FollowMouse_MouseMove);
            base.Stop();
        }
    }
}
