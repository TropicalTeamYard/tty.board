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
    /// GroupItemView.xaml 的交互逻辑
    /// </summary>
    public partial class GroupItemView : CheckControl
    {
        public GroupItemView()
        {
            InitializeComponent();
        }

        public string FIcon
        {
            get { return (string)GetValue(FIconProperty); }
            set { SetValue(FIconProperty, value); }
        }
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public int Indent
        {
            get { return (int)GetValue(IndentProperty); }
            set { SetValue(IndentProperty, value); }
        }
        public string Date
        {
            get { return (string)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }
        public string ExtraData { get; set; }
        public bool IsAdd
        {
            get { return (bool)GetValue(IsAddProperty); }
            set { SetValue(IsAddProperty, value); }
        }

        public static readonly DependencyProperty FIconProperty =
            DependencyProperty.Register("FIcon", typeof(string), typeof(GroupItemView), new PropertyMetadata("\xe7b8"));
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(GroupItemView), new PropertyMetadata(""));
        public static readonly DependencyProperty IndentProperty =
            DependencyProperty.Register("Indent", typeof(int), typeof(GroupItemView), new PropertyMetadata(0,(d,e)=> 
            {
                GroupItemView b = (GroupItemView)d;
                b.Column1.Width = new GridLength(40 * (int)e.NewValue);
                if ((int)e.NewValue == 0)
                {
                    b.TextBlock1.Text = "添加组";
                }
                else
                {
                    b.TextBlock1.Text = "添加页面";
                }
            }));
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(string), typeof(GroupItemView), new PropertyMetadata("2018年6月28日"));
        public static readonly DependencyProperty IsAddProperty =
            DependencyProperty.Register("IsAdd", typeof(bool), typeof(GroupItemView), new PropertyMetadata(false, (d, e) =>
            {
                GroupItemView b = (GroupItemView)d;
                if ((bool)e.NewValue)
                {
                    b.Border1.Visibility = Visibility.Visible;
                    b.Grid1.Visibility = Visibility.Collapsed;
                }
                else
                {
                    b.Border1.Visibility = Visibility.Collapsed;
                    b.Grid1.Visibility = Visibility.Visible;
                }
            }));

        protected override void OnChecked()
        {
            OnColorStyleChanged();
        }
        protected override void OnColorStyleChanged()
        {
            if (IsChecked)
            {
                Background = ThemeBrush;
            }
            else
            {
                Background = Brushes.Transparent;
            }
        }
        public void ReSet()
        {
            OnChecked();
        }

        private void GroupItemView_MouseDown(object sender, MouseButtonEventArgs e)
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
        private void GroupItemView_MouseMove(object sender, MouseEventArgs e)
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
            IconButtonDelete.Text = "\xe74d";
            e.Handled = false;
        }
        private void GroupItemView_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!IsChecked)
            {
                Background = Brushes.Transparent;
            }
            else
            {
                Background = ThemeBrush;
            }
            IconButtonDelete.Text = "";
            e.Handled = false;
        }
        private void GroupItemView_MouseUp(object sender, MouseButtonEventArgs e)
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

        private void IconButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }
    }
}
