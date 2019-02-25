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
    /// IconTriggerButton.xaml 的交互逻辑
    /// </summary>
    public partial class IconTriggerButton : CheckControl
    {
        public IconTriggerButton()
        {
            InitializeComponent();

        }

        /// <summary>
        /// 显示的文字
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private void IconButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {

                if (!IsChecked)
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
                else
                {
                    Color color = ((SolidColorBrush)ThemeBrush).Color;
                    color.A = 150;
                    Background = new SolidColorBrush(color);
                }
            }


            e.Handled = false;
        }
        private void IconButton_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released && e.RightButton == MouseButtonState.Released)
            {
                if (!IsChecked)
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
                else
                {
                    Color color = ((SolidColorBrush)ThemeBrush).Color;
                    color.A = 200;
                    Background = new SolidColorBrush(color);
                }
            }
            e.Handled = false;
        }
        private void IconButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!IsChecked)
            {
                Background = Brushes.Transparent;
            }
            else
            {
                Background = ThemeBrush;
            }
            e.Handled = false;
        }
        private void IconButton_MouseUp(object sender, MouseButtonEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Released)
            {

                if (!IsChecked)
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
                else
                {
                    Color color = ((SolidColorBrush)ThemeBrush).Color;
                    color.A = 120;
                    Background = new SolidColorBrush(color);
                }

            }
            e.Handled = false;
        }

        public static readonly DependencyProperty IsSpecialColorProperty =
            DependencyProperty.Register("IsSpecialColor", typeof(bool), typeof(IconTriggerButton), new PropertyMetadata(false));
        //以下为静态成员
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(IconTriggerButton), new PropertyMetadata(""));

        protected override void OnColorStyleChanged()
        {
            if (ColorStyle == WindowColorStyle.Dark)
            {
                TextBlockIcon.Foreground = Brushes.White;
            }
            else
            {
                TextBlockIcon.Foreground = Brushes.Black;
            }
            if (IsChecked)
            {
                Background = ThemeBrush;
            }
            else
            {
                Background = Brushes.Transparent;
            }
        }
        protected override void OnChecked()
        {
            OnColorStyleChanged();
        }
    }
}
