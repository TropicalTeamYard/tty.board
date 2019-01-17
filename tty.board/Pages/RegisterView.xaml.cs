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
using tty.com.Util;
using tty.com;
using tty.Model;

namespace tty.Pages
{
    /// <summary>
    /// RegisterView.xaml 的交互逻辑
    /// </summary>
    public partial class RegisterView : UserControl
    {
        public RegisterView()
        {
            InitializeComponent();
            App.Core.Com.RegisterCompleted += Com_RegisterCompleted;
        }

        public event RoutedEventHandler GoToLogin;
        public event RoutedEventHandler ToStart;

        #region 输入检查
        private void CheckInput()
        {
            var flag = true;
            if (tbxUser.Text == "")
            {
                flag = false;
                tbxUser.Background = UserBrushes.MereWhite;
            }
            else if (CheckUtil.Nickname(tbxUser.Text))
            {
                tbxUser.Background = UserBrushes.MereWhite;
            }
            else
            {
                flag = false;
                tbxUser.Background = UserBrushes.Warning;
            }

            if (pwb1.Password == "")
            {
                flag = false;
                pwb1.Background = UserBrushes.MereWhite;
            }
            else if (CheckUtil.Password(pwb1.Password))
            {
                pwb1.Background = UserBrushes.MereWhite;
            }
            else
            {
                flag = false;
                pwb1.Background = UserBrushes.Warning;
            }
            if (pwb2.Password == "")
            {
                flag = false;
                pwb2.Background = UserBrushes.MereWhite;
            }
            else if (CheckUtil.Password(pwb2.Password) && pwb1.Password == pwb2.Password)
            {
                pwb2.Background = UserBrushes.MereWhite;
            }
            else
            {
                flag = false;
                pwb2.Background = UserBrushes.Warning;
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

        }
        private void TbxUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckInput();
        }
        private void Pwb1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckInput();
        }
        private void Pwb2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckInput();
        }
        #endregion

        private void GoToLogin_Click(object sender, RoutedEventArgs e)
        {
            GoToLogin?.Invoke(sender, e);
        }

        #region 注册
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            gridMain.Visibility = Visibility.Collapsed;
            gridWait.Visibility = Visibility.Visible;

            App.Core.Com.RegisterAsync(tbxUser.Text, pwb1.Password);
        }
        private void Com_RegisterCompleted(object sender, MessageEventArgs e)
        {
            gridWait.Visibility = Visibility.Collapsed;
            if (e.Status)
            {
                var username = (string)e.Data;

                tbkUserName.Text = username;
                tbkNickName.Text = tbxUser.Text;

                gridCompleted.Visibility = Visibility.Visible;
            }
            else
            {
                gridMain.Visibility = Visibility.Visible;
            }

            App.Core.Window.SendMessage(e.Msg);
        }
        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            ToStart?.Invoke(sender, e);
        }
        #endregion


    }
}
