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
using tty.Windows;

namespace tty.Pages
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : Page
    {
        public readonly NavigationHelper RightHelper;
        public readonly NavigationHelper LeftHelper;

        public MainPage()
        {
            InitializeComponent();
            App.Core.Window.SetTitleBar(GridTitle);

            ibtnUser.SetBinding(Model.ImageTriggerButtonRound.ImageSourceProperty, new Binding() { Source = App.Core.Com.User.Current, Path = new PropertyPath("Portrait") });
            
            Console.WriteLine("---Window ---MainPage ---SetBindingPortrait");

            RightHelper = new NavigationHelper(
                new KeyValuePair<Model.CheckControl, FrameworkElement>(ibtnSettings, bdSettings),
                new KeyValuePair<Model.CheckControl, FrameworkElement>(ibtnUser, bdUser)
                );
            LeftHelper = new NavigationHelper(
                new KeyValuePair<Model.CheckControl, FrameworkElement>(ibtnMsg,bdMsg));

            userFrame.NavigateTo(typeof(UserPage));
            msgFrame.NavigateTo(typeof(MsgPage));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }

   


}
