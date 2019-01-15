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
        }

        public event RoutedEventHandler GoToLogin;

        private bool isInputValid = false;
        private void CheckInput()
        {
            var flag = true;
            if (tbxUser.Text == "")
            {
                flag = false;
                tbxUser.Background = new SolidColorBrush(Color.FromArgb(0x22, 0xff, 0xff, 0xff));
            }
            else if (CheckUtil.Nickname(tbxUser.Text))
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

            if (pwb2.Password == "")
            {
                flag = false;
                pwb2.Background = new SolidColorBrush(Color.FromArgb(0x22, 0xff, 0xff, 0xff));
            }
            else if (CheckUtil.Password(pwb2.Password) && pwb1.Password == pwb2.Password)
            {
                pwb2.Background = new SolidColorBrush(Color.FromArgb(0x22, 0xff, 0xff, 0xff));
            }
            else
            {
                flag = false;
                pwb2.Background = new SolidColorBrush(Color.FromRgb(0xff, 0x66, 0x00));
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
        private void Pwb2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckInput();
        }
        private void GoToLogin_Click(object sender, RoutedEventArgs e)
        {
            GoToLogin?.Invoke(sender, e);
        }


    }
}
