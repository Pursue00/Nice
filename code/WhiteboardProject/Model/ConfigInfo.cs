using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteboardProject.Model
{
    public class ConfigInfo
    {
        //是否需要注册
        public bool IsRegist { get; set; }
        //是否是按着时间授权
        public bool IsTimeAuthorization { get; set; }
        //授权次数
        public int Number { get; set; }
        //授权天数
        public int Days { get; set; }
    }
}
