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
    /// UButton.xaml 的交互逻辑
    /// </summary>
    public partial class UButton : UControl
    {
        public UButton()
        {
            InitializeComponent();
        }


        public object InnerContent
        {
            get { return (object)GetValue(InnerContentProperty); }
            set { SetValue(InnerContentProperty, value); }
        }

        public static readonly DependencyProperty InnerContentProperty =
            DependencyProperty.Register("InnerContent", typeof(object), typeof(UButton), new PropertyMetadata(null));

        private void UButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (ColorStyle == WindowColorStyle.Dark)
                {
                    Background = UserBrushes.MediumWhite;
                }
                else
                {
                    Background = UserBrushes.MediumBlack;
                }
            }
            e.Handled = false;
        }
        private void UButton_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released && e.RightButton == MouseButtonState.Released)
            {
                if (ColorStyle == WindowColorStyle.Dark)
                {
                    Background = UserBrushes.MereWhite;
                }
                else
                {
                    Background = UserBrushes.MereBlack;
                }
            }
            e.Handled = false;
        }
        private void UButton_MouseLeave(object sender, MouseEventArgs e)
        {
            Background = Brushes.Transparent;
            e.Handled = false;
        }
        private void UButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                if (ColorStyle == WindowColorStyle.Dark)
                {
                    Background = UserBrushes.MereWhite;
                }
                else
                {
                    Background = UserBrushes.MereBlack;
                }
            }
            e.Handled = false;
        }
    }
}
