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

namespace tty.Model
{
    /// <summary>
    /// IconPasswordBox.xaml 的交互逻辑
    /// </summary>
    public partial class IconPasswordBox : UserControl
    {
        public IconPasswordBox()
        {
            InitializeComponent();
        }

        public string Password
        {
            get => PasswordBox1.Password;
            set => PasswordBox1.Password = value;
        }

        public string Hint
        {
            get { return (string)GetValue(HintProperty); }
            set { SetValue(HintProperty, value); }
        }

        private void SetPasswordVisibility(bool isVisiable)
        {
            if (isVisiable)
            {
                TextBlock1.Text = Password;
                PasswordBox1.Visibility = Visibility.Collapsed;
                TextBlock1.Visibility = Visibility.Visible;
            }
            else
            {
                PasswordBox1.Visibility = Visibility.Visible;
                TextBlock1.Visibility = Visibility.Collapsed;
            }
        }


        private void IconButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                SetPasswordVisibility(true);
            }
        }

        private void IconButton_MouseLeave(object sender, MouseEventArgs e)
        {
            SetPasswordVisibility(false);
        }

        private void IconButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SetPasswordVisibility(false);
        }

        private void PasswordBox1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PasswordBox1.Password == "")
            {
                TextBlock2.Visibility = Visibility.Visible;
            }
            else
            {
                TextBlock2.Visibility = Visibility.Collapsed;
            }
            PasswordChanged?.Invoke(this, e);
        }

        public static readonly DependencyProperty HintProperty =
            DependencyProperty.Register("Hint", typeof(string), typeof(IconPasswordBox), new PropertyMetadata(""));

        public event RoutedEventHandler PasswordChanged;
    }
}
