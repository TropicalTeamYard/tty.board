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
    /// IconTriggerButtonRound.xaml 的交互逻辑
    /// </summary>
    public partial class IconTriggerButtonRound : CheckControl
    {
        public IconTriggerButtonRound()
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
                        elp1.Fill = UserBrushes.MediumWhite;
                    }
                    else
                    {
                        elp1.Fill = UserBrushes.MediumBlack;
                    }
                }
                else
                {
                    Color color = ((SolidColorBrush)ThemeBrush).Color;
                    color.A = 150;
                    elp1.Fill = new SolidColorBrush(color);
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
                        elp1.Fill = UserBrushes.MereWhite;
                    }
                    else
                    {
                        elp1.Fill = UserBrushes.MereBlack;
                    }
                }
                else
                {
                    Color color = ((SolidColorBrush)ThemeBrush).Color;
                    color.A = 200;
                    elp1.Fill = new SolidColorBrush(color);
                }
            }
            e.Handled = false;
        }
        private void IconButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!IsChecked)
            {
                elp1.Fill = Brushes.Transparent;
            }
            else
            {
                elp1.Fill = ThemeBrush;
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
                        elp1.Fill = UserBrushes.MereWhite;
                    }
                    else
                    {
                        elp1.Fill = UserBrushes.MereBlack;
                    }
                }
                else
                {
                    Color color = ((SolidColorBrush)ThemeBrush).Color;
                    color.A = 120;
                    elp1.Fill = new SolidColorBrush(color);
                }

            }
            e.Handled = false;
        }

        public static readonly DependencyProperty IsSpecialColorProperty =
            DependencyProperty.Register("IsSpecialColor", typeof(bool), typeof(IconTriggerButtonRound), new PropertyMetadata(false));
        //以下为静态成员
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(IconTriggerButtonRound), new PropertyMetadata(""));

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
                elp1.Fill = ThemeBrush;
            }
            else
            {
                elp1.Fill = Brushes.Transparent;
            }
        }
        protected override void OnChecked()
        {
            OnColorStyleChanged();
        }
    }
}
