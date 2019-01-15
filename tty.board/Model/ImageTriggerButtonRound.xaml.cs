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
    /// ImageTriggerButtonRound.xaml 的交互逻辑
    /// </summary>
    public partial class ImageTriggerButtonRound : CheckControl
    {
        public ImageTriggerButtonRound()
        {
            InitializeComponent();

        }

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ImageTriggerButtonRound), new PropertyMetadata(null,(d,e)=> 
            {
                ((ImageTriggerButtonRound)d).OnImageSourceChanged();
            }));

        private void OnImageSourceChanged()
        {
            if (ImageSource == null)
            {
                TextBlockIcon.Visibility = Visibility.Visible;
            }
            else
            {
                TextBlockIcon.Visibility = Visibility.Collapsed;
            }
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
            DependencyProperty.Register("IsSpecialColor", typeof(bool), typeof(ImageTriggerButtonRound), new PropertyMetadata(false));

        protected override void OnColorStyleChanged()
        {
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
