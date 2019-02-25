using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace tty.Model
{
    public class UControl : UserControl
    {
        public UControl()
        {
            MouseDown += UControl_MouseDown;
            MouseLeave += UControl_MouseLeave;
            MouseUp += UControl_MouseUp;
            //MouseMove += UControl_MouseMove;
            //MouseEnter += UControl_MouseEnter;
        }

        //private void UControl_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    if (e.LeftButton == MouseButtonState.Released && e.RightButton == MouseButtonState.Released)
        //    {
        //        if (IsOpenHover)
        //        {
        //            BackgroundRecord = Background;
        //            Color color = OnHoveringColor; color.A = (byte)(0.2 * 0xff);
        //            Background = new SolidColorBrush(color);
        //        }
        //    }
        //    e.Handled = false;
        //}

        private void UControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                IsInClick = true;
                //if (IsOpenHover)
                //{
                //    //BackgroundRecord = Background;
                //    Color color = OnHoveringColor; color.A = (byte)(0.4 * 0xff);
                //    Background = new SolidColorBrush(color);
                //}
            }
            e.Handled = false;
        }
        private void UControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!e.Handled)
            {
                //处于单击状态.
                if (Mark && IsInClick)
                {
                    RaiseEvent(new RoutedEventArgs(ClickEvent));
                }
                IsInClick = false;
                //if (IsOpenHover)
                //{
                //    //BackgroundRecord = Background;
                //    Color color = OnHoveringColor; color.A = (byte)(0.2 * 0xff);
                //    Background = new SolidColorBrush(color);
                //}

                e.Handled = false;
            }
        }
        private void UControl_MouseLeave(object sender, MouseEventArgs e)
        {
            IsInClick = false;
            //if (IsOpenHover)
            //{
            //    Background = BackgroundRecord;
            //}
            e.Handled = false;
        }

        /// <summary>
        /// 记录控件是否处于点击状态.
        /// </summary>
        bool IsInClick { get; set; } = false;
        public bool Mark { get; set; } = true;
        Brush BackgroundRecord { get; set; }

        public event RoutedEventHandler Click
        {
            add => AddHandler(ClickEvent, value);
            remove => RemoveHandler(ClickEvent, value);
        }
        public WindowColorStyle ColorStyle
        {
            get { return (WindowColorStyle)GetValue(ColorStyleProperty); }
            set { SetValue(ColorStyleProperty, value); }
        }

        protected virtual void OnColorStyleChanged()
        {

        }

        public static readonly DependencyProperty ColorStyleProperty =
            DependencyProperty.Register("ColorStyle", typeof(WindowColorStyle), typeof(UControl), new PropertyMetadata(WindowColorStyle.Dark, new PropertyChangedCallback((d, e) =>
            {
                ((UControl)d).OnColorStyleChanged();
            })));
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(IconButton));

        public Brush ThemeBrush
        {
            get { return (Brush)GetValue(ThemeBrushProperty); }
            set { SetValue(ThemeBrushProperty, value); }
        }

        public static readonly DependencyProperty ThemeBrushProperty =
            DependencyProperty.Register("ThemeBrush", typeof(Brush), typeof(UControl), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(3, 131, 135))));

        public Color OnHoveringColor
        {
            get { return (Color)GetValue(OnHoveringColorProperty); }
            set { SetValue(OnHoveringColorProperty, value); }
        }
        public static readonly DependencyProperty OnHoveringColorProperty =
            DependencyProperty.Register("OnHoveringColor", typeof(Color), typeof(UControl), new PropertyMetadata(Colors.White));

        public bool IsOpenHover
        {
            get { return (bool)GetValue(IsOpenHoverProperty); }
            set { SetValue(IsOpenHoverProperty, value); }
        }

        public static readonly DependencyProperty IsOpenHoverProperty =
            DependencyProperty.Register("IsOpenHover", typeof(bool), typeof(UControl), new PropertyMetadata(false));

    }
}
