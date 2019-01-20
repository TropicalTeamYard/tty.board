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
using Microsoft.Win32;

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

            imgPortrait.SetBinding(Model.ImageTriggerButtonRound.ImageSourceProperty, new Binding() { Source = App.Core.Com.User.Current, Path = new PropertyPath("Portrait") });
            tbkNickName.SetBinding(TextBlock.TextProperty, new Binding() { Source = App.Core.Com.User.Current, Path = new PropertyPath("nickname") });
            tbkUserName.SetBinding(TextBlock.TextProperty, new Binding() { Source = App.Core.Com.User.Current, Path = new PropertyPath("username") });
            SetBinding(UserPage.UserStateProperty, new Binding() { Source = App.Core.Com.User.Current, Path = new PropertyPath("userstate") });

            App.Core.Com.LoginCompleted += Com_LoginCompleted;
            App.Core.Com.ChangeNickNameCompleted += Com_ChangeNickNameCompleted;
            App.Core.Com.ChangePwCompleted += Com_ChangePwCompleted;
            App.Core.Com.ChangePortraitCompleted += Com_ChangePortraitCompleted;
            Console.WriteLine("---Window ---UserPage ---SetBinding");
        }
        private void OnUserStateChanged()
        {
            if (UserState == UserState.PasswordError)
            {
                gridRepair.Visibility = Visibility.Visible;
                ibarRepairOpen.IsEnabled = true;
                tbkRepair.Text = "需要修复账户";
                gridChangeInfoFlyout.Visibility = Visibility.Collapsed;
                gridChangePortraitFlyout.Visibility = Visibility.Collapsed;
                //禁用其他操作
                ibarChangeInfoOpen.IsEnabled = false;
                ibarChangePwOpen.IsEnabled = false;

            }
            else
            {
                gridRepair.Visibility = Visibility.Collapsed;
                ibarChangeInfoOpen.IsEnabled = true;
                ibarChangePwOpen.IsEnabled = true;
            }
        }
        public UserState UserState
        {
            get { return (UserState)GetValue(UserStateProperty); }
            set { SetValue(UserStateProperty, value); }
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

            App.Core.Com.LoginAsync(App.Core.Com.User.Current.username,pwb1.Password);
        }
        private void IconButtonRepairBack_Click(object sender, RoutedEventArgs e)
        {
            gridRepairFlyout.Visibility = Visibility.Collapsed;
        }
        private void Pwb1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (pwb1.Password == "")
            {
                ibarRepair.IsEnabled = false;
                pwb1.Background = new SolidColorBrush(Color.FromArgb(0x22, 0xff, 0xff, 0xff));
            }
            else if (CheckUtil.Password(pwb1.Password))
            {
                ibarRepair.IsEnabled = true;
                pwb1.Background = new SolidColorBrush(Color.FromArgb(0x22, 0xff, 0xff, 0xff));

            }
            else
            {
                ibarRepair.IsEnabled = false;
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
        private void IconBarChangeInfoOpen_Click(object sender, RoutedEventArgs e)
        {
            gridChangeInfoFlyout.Visibility = Visibility.Visible;
            tbxNickName.Text = tbkNickName.Text;
            tbxEmail.Text = tbkEmail.Text;
            ibarChangeInfo.IsEnabled = false;
        }
        private void Com_ChangeNickNameCompleted(object sender, MessageEventArgs e)
        {
            App.Core.Window.SendMessage(e.Msg);
        }
        private void CheckInputChangeInfo()
        {
            bool isInputValid = true;

            if (tbxNickName.Text == "")
            {
                tbxNickName.Background = UserBrushes.MediumWhite;
                isInputValid = false;
            }
            else if (CheckUtil.Nickname(tbkNickName.Text))
            {
                tbxNickName.Background = UserBrushes.MediumWhite;
            }
            else
            {
                isInputValid = false;
                tbxNickName.Background = UserBrushes.Warning;
            }

            if (isInputValid)
            {
                ibarChangeInfo.IsEnabled = true;
            }
            else
            {
                ibarChangeInfo.IsEnabled = false;
            }
        }
        private void tbxNickName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckInputChangeInfo();
        }
        private void BtnChangeInfo_Click(object sender, RoutedEventArgs e)
        {
            App.Core.Com.ChangeNickNameAsync(tbxNickName.Text);
            gridChangeInfoFlyout.Visibility = Visibility.Collapsed;
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
            App.Core.Com.ExitLogin();
            App.Core.Window.NavigateTo(typeof(StartPage));
        }
        #endregion
        #region 修改密码
        private void Com_ChangePwCompleted(object sender, MessageEventArgs e)
        {
            if (e.Status == true)
            {
                App.Core.Window.NavigateTo(typeof(StartPage));
            }
            else
            {
                ibarChangePw.Text = "修改密码";
                pwb_1.Background = UserBrushes.Warning;
            }

            App.Core.Window.SendMessage(e.Msg);
        }
        private void CheckInputChangePw()
        {
            bool flag = true;

            if (pwb_1.Password == "")
            {
                flag = false;
                pwb_1.Background = UserBrushes.MereWhite;
            }
            else if (CheckUtil.Password(pwb_1.Password))
            {
                pwb_1.Background = UserBrushes.MereWhite;
            }
            else
            {
                flag = false;
                pwb_1.Background = UserBrushes.Warning;
            }

            if (pwb_2.Password == "")
            {
                flag = false;
                pwb_2.Background = UserBrushes.MereWhite;
            }
            else if (CheckUtil.Password(pwb_2.Password))
            {
                pwb_2.Background = UserBrushes.MereWhite;
            }
            else
            {
                flag = false;
                pwb_2.Background = UserBrushes.Warning;
            }

            if (pwb_3.Password == "")
            {
                flag = false;
                pwb_3.Background = UserBrushes.MereWhite;
            }
            else if (CheckUtil.Password(pwb_3.Password) && pwb_2.Password == pwb_3.Password)
            {
                pwb_3.Background = UserBrushes.MereWhite;
            }
            else
            {
                flag = false;
                pwb_3.Background = UserBrushes.Warning;
            }

            if (flag)
            {
                ibarChangePw.IsEnabled = true;
            }
            else
            {
                ibarChangePw.IsEnabled = false;
            }
        }
        private void IconBarChangePwOpen_Click(object sender, RoutedEventArgs e)
        {
            gridChangePwFlyout.Visibility = Visibility.Visible;
        }
        private void IconButtonChangePwBack_Click(object sender, RoutedEventArgs e)
        {
            gridChangePwFlyout.Visibility = Visibility.Collapsed;
        }
        private void pwbChangPw_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckInputChangePw();
        }
        private void IbarChangePw_Click(object sender, RoutedEventArgs e)
        {
            ibarChangePw.IsEnabled = false;
            ibarChangePw.Text = "正在修改";
            App.Core.Com.ChangePwAsync(pwb_1.Password, pwb_2.Password);
        }
        #endregion
        #region 修改头像
        private void Com_ChangePortraitCompleted(object sender, MessageEventArgs e)
        {
            App.Core.Window.SendMessage(e.Msg);
        }
        private void IconButtonChangePortraitBack_Click(object sender, RoutedEventArgs e)
        {
            gridChangePortraitFlyout.Visibility = Visibility.Collapsed;
        }
        private void IconBarPickPic_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = ".jpg|*.jpg|.png|*.png|.jpeg|*.jpeg";
            if (dialog.ShowDialog() == false) return;
            string _fileName = dialog.FileName;
            //初始化图片
            BitmapImage tempImage = new BitmapImage();
            tempImage.BeginInit();
            tempImage.UriSource = new Uri(_fileName, UriKind.RelativeOrAbsolute);
            tempImage.EndInit();

            imgd.Source = tempImage;
            imgd.SelectArea = null;
            imgd.Visibility = Visibility.Visible;
            ibarPortraitSubmit.IsEnabled = true;
        }
        private void ImgPortrait_Click(object sender, RoutedEventArgs e)
        {
            if (UserState == UserState.Success)
            {
                gridChangePortraitFlyout.Visibility = Visibility.Visible;
            }
        }
        private void IconBarPortraitSubmit_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage bitmapImage = ToolUtil.ReSizeImage(imgd.ClipSource, new Size(218, 218));
            Console.WriteLine($"---Window ---UserPage ---CutPic:width={bitmapImage.PixelWidth};height={bitmapImage.PixelHeight}");
            App.Core.Com.ChangePortraitAsync(bitmapImage);
            gridChangePortraitFlyout.Visibility = Visibility.Collapsed;
        }
        #endregion


    }
}
