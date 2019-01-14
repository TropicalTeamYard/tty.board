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
using tty.com;
using tty.com.Util;

namespace tty.Pages
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            App.Instance.Com.LoginCompleted += Com_LoginCompleted;
        }

        #region 返回
        public event RoutedEventHandler Back;
        private void IconButtonRound_Click(object sender, RoutedEventArgs e)
        {
            Back?.Invoke(sender, e);
        }
        #endregion
        #region 输入检查
        private bool isInputValid = false;
        private void CheckInput()
        {
            var flag = true;
            if (tbxUser.Text == "")
            {
                flag = false;
                tbxUser.Background = new SolidColorBrush( Color.FromArgb(0x22, 0xff, 0xff, 0xff));
            }
            else if (CheckUtil.Username(tbxUser.Text))
            {
                tbxUser.Background = new SolidColorBrush(Color.FromArgb(0x22, 0xff, 0xff, 0xff));
            }
            else
            {
                flag = false;
                tbxUser.Background = new SolidColorBrush(Color.FromRgb(0xff, 0x66, 0x33));
            }

            if (pwb1.Password == "")
            {
                flag = false;
                pwb1.Background = new SolidColorBrush(Color.FromArgb(0x22, 0xff, 0xff, 0xff));
            }
            else if (CheckUtil.Password(pwb1.Password))
            {
                pwb1.Background = new SolidColorBrush(Color.FromArgb(0x22, 0xff, 0xff, 0xff));
            }
            else
            {
                flag = false;
                pwb1.Background = new SolidColorBrush(Color.FromRgb(0xff, 0x66, 0x00));
            }

            if (flag)
            {
                gridLogin.Background = Brushes.DeepSkyBlue;
                btnLogin.IsEnabled = true;
            }
            else
            {
                gridLogin.Background = new SolidColorBrush(Color.FromRgb(0xaa, 0xaa, 0xaa));
                btnLogin.IsEnabled = false;
            }

            isInputValid = flag;
        }
        private void TbxUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckInput();
        }
        private void Pwb1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckInput();
        }
        #endregion
        #region 登录
        private void Com_LoginCompleted(object sender, MessageEventArgs e)
        {
            if (e.Status == false)
            {
                gridMain.Visibility = Visibility.Visible;
                gridWait.Visibility = Visibility.Collapsed;
            }
            else
            {
                App.Instance.Window.NavigateTo(typeof(MainPage));
            }

            App.Instance.Window.SendMessage(e.Msg);
        }
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            gridMain.Visibility = Visibility.Collapsed;
            gridWait.Visibility = Visibility.Visible;

            App.Instance.Com.LoginAsync(tbxUser.Text, pwb1.Password);
        }
        #endregion

    }
}
