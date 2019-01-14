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
using tty.com.Model;
using tty.com;
using tty.com.Util;
using tty.Model;

namespace tty.Pages
{
    /// <summary>
    /// UserPage.xaml 的交互逻辑
    /// </summary>
    public partial class UserPage : Page
    {
        public UserPage()
        {
            InitializeComponent();

            imgPortrait.SetBinding(Model.ImageTriggerButtonRound.ImageSourceProperty, new Binding() { Source = App.Instance.Com.User.Current, Path = new PropertyPath("Portrait") });
            tbkNickName.SetBinding(TextBlock.TextProperty, new Binding() { Source = App.Instance.Com.User.Current, Path = new PropertyPath("nickname") });
            tbkUserName.SetBinding(TextBlock.TextProperty, new Binding() { Source = App.Instance.Com.User.Current, Path = new PropertyPath("username") });
            SetBinding(UserPage.UserStateProperty, new Binding() { Source = App.Instance.Com.User.Current, Path = new PropertyPath("userstate") });

            App.Instance.Com.LoginCompleted += Com_LoginCompleted;
            Console.WriteLine("---Window ---UserPage ---SetBinding");
        }


        #region 修复账号
        private void Com_LoginCompleted(object sender, MessageEventArgs e)
        {
            if (e.Status == false)
            {
                ibarRepairOpen.IsEnabled = true;
                tbkRepair.Text = "需要修复账户";
            }
            //其他的由绑定来完成。
        }
        private void IconBarRepairOpen_Click(object sender, RoutedEventArgs e)
        {
            gridRepairFlyout.Visibility = Visibility.Visible;
        }
        private void IbarRepair_Click(object sender, RoutedEventArgs e)
        {
            gridRepair.Visibility = Visibility.Collapsed;
            ibarRepairOpen.IsEnabled = false;
            tbkRepair.Text = "正在修复";

            App.Instance.Com.LoginAsync(App.Instance.Com.User.Current.username,pwb1.Password);
        }
        private void IconButtonRepairBack_Click(object sender, RoutedEventArgs e)
        {
            gridRepairFlyout.Visibility = Visibility.Collapsed;
        }
        private void OnUserStateChanged()
        {
            if (UserState == UserState.PasswordError)
            {
                gridRepair.Visibility = Visibility.Visible;
                ibarRepairOpen.IsEnabled = true;
                tbkRepair.Text = "需要修复账户";

                //禁用其他操作
                ibarChangeInfo.IsEnabled = false;
                ibarChangeInfo.ThemeBrush = Brushes.DarkGray;
            }
            else
            {
                gridRepair.Visibility = Visibility.Collapsed;

                ibarChangeInfo.IsEnabled = true;
                ibarChangeInfo.ThemeBrush = UserBrushes.BlueGreen;
            }
        }
        public UserState UserState
        {
            get { return (UserState)GetValue(UserStateProperty); }
            set { SetValue(UserStateProperty, value); }
        }
        private void Pwb1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (pwb1.Password == "")
            {
                ibarRepair.IsEnabled = false;
                ibarRepair.ThemeBrush = new SolidColorBrush(Color.FromRgb(0xaa, 0xaa, 0xaa));
                pwb1.Background = new SolidColorBrush(Color.FromArgb(0x22, 0xff, 0xff, 0xff));
            }
            else if (CheckUtil.Password(pwb1.Password))
            {
                ibarRepair.IsEnabled = true;
                ibarRepair.ThemeBrush = UserBrushes.BlueGreen;
                pwb1.Background = new SolidColorBrush(Color.FromArgb(0x22, 0xff, 0xff, 0xff));

            }
            else
            {
                ibarRepair.IsEnabled = false;
                ibarRepair.ThemeBrush = new SolidColorBrush(Color.FromRgb(0xaa, 0xaa, 0xaa));
                pwb1.Background = new SolidColorBrush(Color.FromRgb(0xff, 0x66, 0x00));
            }
        }
        public static readonly DependencyProperty UserStateProperty =
            DependencyProperty.Register("UserState", typeof(UserState), typeof(UserPage), new PropertyMetadata(UserState.Success, (d, e) => {
                ((UserPage)d).OnUserStateChanged();
            }));

        #endregion
        #region 修改基础信息
        private void IconButtonChangeInfoBack_Click(object sender, RoutedEventArgs e)
        {
            gridChangeInfoFlyout.Visibility = Visibility.Collapsed;
        }
        private void IconBarChangeInfo_Click(object sender, RoutedEventArgs e)
        {
            gridChangeInfoFlyout.Visibility = Visibility.Visible;
        }

        #endregion
        #region 退出登录
        private void IconButtonExitOpen_Click(object sender, RoutedEventArgs e)
        {
            gridExitLoginFlyout.Visibility = Visibility.Visible;
        }
        private void IconButtonExitBack_Click(object sender, RoutedEventArgs e)
        {
            gridExitLoginFlyout.Visibility = Visibility.Collapsed;
        }
        private void IconButtonExit_Click(object sender, RoutedEventArgs e)
        {
            App.Instance.Com.ExitLogin();
            App.Instance.Window.NavigateTo(typeof(StartPage));
        }
        #endregion

    }
}
