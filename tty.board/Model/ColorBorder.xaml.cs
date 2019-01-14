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
    /// ColorBorder.xaml 的交互逻辑
    /// </summary>
    public partial class ColorBorder : UControl
    {
        public ColorBorder()
        {
            InitializeComponent();
        }
        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(ColorBorder), new PropertyMetadata(false,(d,e)=>
            {
                ColorBorder b = (ColorBorder)d;
                if ((bool)e.NewValue)
                {
                    b.TextBlock1.Visibility = Visibility.Visible;
                    b.Border1.Visibility = Visibility.Visible;
                }
                else
                {
                    b.TextBlock1.Visibility = Visibility.Collapsed;
                    b.Border1.Visibility = Visibility.Collapsed;
                }
            }));




        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(ColorBorder), new PropertyMetadata(Colors.Black,(d,e)=> 
            {
                ColorBorder b = (ColorBorder)d;
                b.Background = new SolidColorBrush(b.Color);

                if (b.Color.R + b.Color.G + b .Color.B>=510)
                {
                    b.TextBlock1.Foreground = Brushes.Black;
                    b.Border1.BorderBrush = Brushes.Black;
                }
                else
                {
                    b.TextBlock1.Foreground = Brushes.White;
                    b.Border1.BorderBrush = Brushes.White;
                }
            }));

        private void CheckControl_MouseMove(object sender, MouseEventArgs e)
        {
            Border1.Visibility = Visibility.Visible;
        }

        private void CheckControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!IsChecked)
            {
                Border1.Visibility = Visibility.Collapsed;
            }
        }
    }
}
