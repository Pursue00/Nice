using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using WhiteboardProject.Common;
using WhiteboardProject.Model;
using WhiteboardProject.UC;

namespace WhiteboardProject
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        NetWorkHelper _NetWorkHelper = new NetWorkHelper();

        public App()
        {
            //判断是否需要注册
            //IsHaveRegist();
            //授权限制
           // Authorization();
        }

        private void IsHaveRegist()
        {
            string _file = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "电子签名.txt");
            if (!File.Exists(_file))
            {
                ExpiredTips _ExpiredTips = new ExpiredTips();
                _ExpiredTips.tbTime.Content = "本软件需注册，请联系开发者";
                _ExpiredTips.tbBox.Text = _NetWorkHelper.getMacAddr_Local();
                _ExpiredTips.ShowDialog();
                if (_ExpiredTips.DialogResult == true)
                {
                    App.Current.Shutdown();
                }
            }
        }

        private void Authorization()
        {
            //解密过程
            string _keyPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "电子签名.txt");
            string _fileTime = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RegistTime.txt");
            string _fileNumber = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Number.txt");
            string _content = _NetWorkHelper.ReadTo(_keyPath);
            bool _Decrypt = false;
            string _jiemiResult = _NetWorkHelper.Decrypt(_content, _NetWorkHelper.getMacAddr_Local(), ref _Decrypt);
            if (_Decrypt == false)
            {
                ExpiredTips _ExpiredTips = new ExpiredTips();
                _ExpiredTips.tbTime.Content = "本软件需注册，请联系开发者";
                _ExpiredTips.tbBox.Text = _NetWorkHelper.getMacAddr_Local();
                _ExpiredTips.ShowDialog();
                if (_ExpiredTips.DialogResult == true)
                {
                    App.Current.Shutdown();
                }
            }
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            ConfigInfo _Config = jsonSerializer.Deserialize<ConfigInfo>(_jiemiResult);
            if (_Config.IsRegist)
            {
                if (File.Exists(_fileTime))
                    File.Delete(_fileTime);
                if (File.Exists(_fileNumber))
                    File.Delete(_fileNumber);
                ConfigInfo _ConfigTwo = new ConfigInfo()
                {
                    IsRegist = false,
                    IsTimeAuthorization = _Config.IsTimeAuthorization,
                    Days = _Config.Days,
                    Number = _Config.Number
                };
                JavaScriptSerializer jsonSerializerTwo = new JavaScriptSerializer();
                string r1 = jsonSerializer.Serialize(_ConfigTwo);
                string _miwen = _NetWorkHelper.Encrypt(r1, _NetWorkHelper.getMacAddr_Local());
                _NetWorkHelper.Write(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "电子签名.txt"), _miwen);
            }
            if (_Config.IsTimeAuthorization)
            {
                //TimeCompare(_Config.Days);
            }
            else
            {
                NumberCompare(_Config.Number);
            }
        }

        private void NumberCompare(int _Number)
        {
            string _file = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Number.txt");
            if (File.Exists(_file))
            {
                //解密注册时间
                string _content = _NetWorkHelper.ReadTo(_file);
                int _result = int.Parse(_NetWorkHelper._jiemi(_NetWorkHelper.getMacAddr_Local(), _content));
                if (_result >= _Number)
                {
                    ExpiredTips _ExpiredTips = new ExpiredTips();
                    _ExpiredTips.tbTime.Content = "已经超过授权次数，请购买授权码";
                    _ExpiredTips.tbBox.Text = _NetWorkHelper.getMacAddr_Local();
                    _ExpiredTips.ShowDialog();
                    if (_ExpiredTips.DialogResult == true)
                    {
                        App.Current.Shutdown();
                    }
                }
                else
                {
                    string _miwen = _NetWorkHelper._jiami(_NetWorkHelper.getMacAddr_Local(), (_result + 1).ToString());
                    _NetWorkHelper.Write(_file, _miwen);
                }
            }
            else
            {
                string _miwen = _NetWorkHelper._jiami(_NetWorkHelper.getMacAddr_Local(), "1");
                _NetWorkHelper.Write(_file, _miwen);

            }
        }
    }
}
