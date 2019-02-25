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
    /// IconTextBox.xaml 的交互逻辑
    /// </summary>
    public partial class IconTextBox : UControl
    {
        public IconTextBox()
        {
            InitializeComponent();
        }

        private void IconButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            Text = "";
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Text = TextBox1.Text;
            if (TextBox1.Text == "")
            {
                TextBlock1.Visibility = Visibility.Visible;
            }
            else
            {
                TextBlock1.Visibility = Visibility.Collapsed;
            }
            TextChanged?.Invoke(this, e);
        }


        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public bool IsIconShow
        {
            get { return (bool)GetValue(IsIconShowProperty); }
            set { SetValue(IsIconShowProperty, value); }
        }
        public string Hint
        {
            get { return (string)GetValue(HintProperty); }
            set { SetValue(HintProperty, value); }
        }
        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(IconTextBox), new PropertyMetadata(""));
        public static readonly DependencyProperty IsIconShowProperty =
            DependencyProperty.Register("IsIconShow", typeof(bool), typeof(IconTextBox), new PropertyMetadata(true, (d, e) => { ((IconTextBox)d).OnIsIconShowChanged(); }));
        public static readonly DependencyProperty HintProperty =
            DependencyProperty.Register("Hint", typeof(string), typeof(IconTextBox), new PropertyMetadata(""));
        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(IconTextBox), new PropertyMetadata(TextWrapping.NoWrap));

        private void OnIsIconShowChanged()
        {
            if (IsIconShow)
            {
                IconButton1.Visibility = Visibility.Visible;
            }
            else
            {
                IconButton1.Visibility = Visibility.Collapsed;
            }
        }
        protected override void OnColorStyleChanged()
        {
            if (ColorStyle == WindowColorStyle.Dark)
            {
                Background = Brushes.Black;
                TextBox1.CaretBrush = Brushes.White;
                TextBox1.Foreground = Brushes.White;
                Border1.BorderBrush = Brushes.Gray;
                TextBlock1.Foreground = Brushes.DarkGray;
            }
            else
            {
                Background = Brushes.White;
                TextBox1.CaretBrush = Brushes.Black;
                TextBox1.Foreground = Brushes.Black;
                Border1.BorderBrush = Brushes.Gray;
                TextBlock1.Foreground = Brushes.DarkGray;
            }
            IconButton1.ColorStyle = ColorStyle;
        }

        public event TextChangedEventHandler TextChanged;
    }
}
