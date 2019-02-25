using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
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
            bool flag = true;
            try
            {
                if (File.Exists(windowpath))
                {
                    WindowData = JsonConvert.DeserializeObject<WindowData>(
                        File.ReadAllText(windowpath)
                        );
                }
            }
            catch (Exception)
            {
                flag = false;
            }

            if (flag)
            {
                Window.IsFullScreen = WindowData.IsFullScreen;
                Window.IsMaxmize = WindowData.IsMaxSize;
                Window.WindowLocation = WindowData.WindowLocation;
                Window.WindowSize = WindowData.WindowSize;
            }
        }
        public void SaveWindowSettings()
        {
            WindowData.IsFullScreen = Window.IsFullScreen;
            WindowData.IsMaxSize = Window.IsMaxmize;
            WindowData.WindowLocation = Window.WindowLocation;
            WindowData.WindowSize = Window.WindowSize;

            try
            {
                File.WriteAllText(windowpath, JsonConvert.SerializeObject(WindowData));
            }
            catch (Exception)
            {
            }
        }
        //public void SaveUserSettings()
        //{
        //    Core.Com.Finish();
        //}

        //public Com Com { get; set; }
        public string Cache { get; } = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\tty.client";
        private string windowpath => Cache + @"\window.json";
        public static App Core => (App)Current;
        /// <summary>
        /// 获取程序的主窗体.
        /// </summary>
        public MainWindow Window => (MainWindow)Current.MainWindow;
        public WindowData WindowData { get; set; } = new WindowData();
    }

    public class WindowData
    {
        public bool IsFullScreen { get; set; } = false;
        public bool IsMaxSize { get; set; } = false;
        public Point WindowLocation { get; set; } = new Point(0.2, 0.2);
        public Size WindowSize { get; set; } = new Size(0.6, 0.6);
    }
}
