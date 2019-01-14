using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using tty.com;

namespace tty
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {

        public void LoadWindowSettings()
        {

        }
        public void SaveWindowSettings()
        {

        }
        public void SaveUserSettings()
        {
            App.Instance.Com.Finish();
        }

        public static App Instance => (App)Current;
        /// <summary>
        /// 获取程序的主窗体.
        /// </summary>
        public MainWindow Window => (MainWindow)Current.MainWindow;
        public Com Com { get; set; }
    }
}
