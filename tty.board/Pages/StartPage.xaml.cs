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

namespace tty.Pages
{
    /// <summary>
    /// StartPage.xaml 的交互逻辑
    /// </summary>
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();

            //设置标题栏，提供移动功能。
            App.Core.Window.SetTitleBar(GridTitle);
            App.Core.Com.InitializeCompleted += Com_InitializeCompleted;


        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            App.Core.Com.InitializeAsync();
        }

        private void Com_InitializeCompleted(object sender, MessageEventArgs e)
        {
            int data = (int)e.Data;

            gridStart.Visibility = Visibility.Visible;
            gridRing.Visibility = Visibility.Collapsed;
            if (data == 1)
            {
                ViewStart.Visibility = Visibility.Collapsed;
                ViewLogin.Visibility = Visibility.Visible;
                ViewLogin.tbxUser.Text = App.Core.Com.User.Current.username;
            }
            else if (data == 2)
            {
                App.Core.Window.NavigateTo(typeof(MainPage));
            }

            App.Core.Window.SendMessage(e.Msg);
        }

        private void UButton_Click(object sender, RoutedEventArgs e)
        {
            ViewStart.Visibility = Visibility.Collapsed;
            ViewLogin.Visibility = Visibility.Visible;
        }

        private void ViewLogin_Back(object sender, RoutedEventArgs e)
        {
            ViewStart.Visibility = Visibility.Visible;
            ViewLogin.Visibility = Visibility.Collapsed;
        }

        private void ViewLogin_GoToRegister(object sender, RoutedEventArgs e)
        {
            ViewLogin.Visibility = Visibility.Collapsed;
            ViewRegister.Visibility = Visibility.Visible;
        }

        private void ViewRegister_GoToLogin(object sender, RoutedEventArgs e)
        {
            ViewLogin.Visibility = Visibility.Visible;
            ViewRegister.Visibility = Visibility.Collapsed;
        }

        private void ViewRegister_ToStart(object sender, RoutedEventArgs e)
        {
            ViewRegister.Visibility = Visibility.Collapsed;
            ViewLogin.Visibility = Visibility.Visible;
            ViewLogin.tbxUser.Text = ViewRegister.tbkUserName.Text;
        }
    }
}
