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
    /// IconButtonRound.xaml 的交互逻辑
    /// </summary>
    public partial class IconButtonRound : UControl
    {
        public IconButtonRound()
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
        public bool IsSpecialColor
        {
            get { return (bool)GetValue(IsSpecialColorProperty); }
            set { SetValue(IsSpecialColorProperty, value); }
        }


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
        }

        private void IconButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (!IsSpecialColor)
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
                    elp1.Fill = UserBrushes.LightRed;
                }
            }
            e.Handled = false;
        }
        private void IconButton_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released && e.RightButton == MouseButtonState.Released)
            {
                if (!IsSpecialColor)
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
                    elp1.Fill = UserBrushes.MereDarkRed;
                }
            }
            e.Handled = false;
        }
        private void IconButton_MouseLeave(object sender, MouseEventArgs e)
        {
            elp1.Fill = Brushes.Transparent;
            e.Handled = false;
        }
        private void IconButton_MouseUp(object sender, MouseButtonEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Released)
            {
                if (!IsSpecialColor)
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
                    elp1.Fill = UserBrushes.MereDarkRed;
                }
            }
            e.Handled = false;
        }

        //以下为静态成员
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(IconButtonRound), new PropertyMetadata(""));
        public static readonly DependencyProperty IsSpecialColorProperty =
            DependencyProperty.Register("IsSpecialColor", typeof(bool), typeof(IconButtonRound), new PropertyMetadata(false));
    }
}