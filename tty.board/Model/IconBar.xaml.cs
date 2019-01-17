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
    /// IconBar.xaml 的交互逻辑
    /// </summary>
    public partial class IconBar : CheckControl
    {
        public IconBar()
        {
            InitializeComponent();
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public FontFamily IconFontFamily
        {
            get { return (FontFamily)GetValue(IconFontFamilyProperty); }
            set { SetValue(IconFontFamilyProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(IconBar), new PropertyMetadata(""));
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(string), typeof(IconBar), new PropertyMetadata("\xe700"));
        public static readonly DependencyProperty IconFontFamilyProperty =
            DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(IconBar), new PropertyMetadata(new FontFamily("Microsoft YaHei UI")));

        private void IconBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (IsOpenHover)
                {
                    Color color = OnHoveringColor; color.A = (byte)(0.4 * 0xff);
                    GridBackground.Background = new SolidColorBrush(color);
                }
            }
        }
        private void IconBar_MouseEnter(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released && e.RightButton == MouseButtonState.Released)
            {
                if (IsOpenHover)
                {
                    Color color = OnHoveringColor; color.A = (byte)(0.2 * 0xff);
                    GridBackground.Background = new SolidColorBrush(color);
                }
            }
        }
        private void IconBar_MouseLeave(object sender, MouseEventArgs e)
        {
            if (IsOpenHover)
            {
                GridBackground.Background = Brushes.Transparent;
            }
        }
        private void IconBar_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (IsOpenHover)
            {
                Color color = OnHoveringColor; color.A = (byte)(0.2 * 0xff);
                GridBackground.Background = new SolidColorBrush(color);
            }
        }
        private void IconBar_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsEnabled)
            {
                gridMain.Background = Brushes.Transparent;
            }
            else
            {
                gridMain.Background = Brushes.DarkGray;
            }
        }

        protected override void OnChecked()
        {
            if (IsChecked)
            {
                Background = ThemeBrush;
            }
            else
            {
                Background = Brushes.Transparent;
            }
            base.OnChecked();
        }
    }
}
