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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using tty.com;
using tty.Model;
using tty.Pages;

namespace tty
{
    /// <summary>
    /// MainWindow.xaml 的外部逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            //XSerializer = new XCore.Component.XSerializer<MainWindow>(this, App.WindowSettingsPath, new Predicate<string>((s) =>
            //{
            //    string[] ps = { nameof(IsFullScreen), nameof(IsMaxmize), nameof(WindowLocation), nameof(WindowSize) };
            //    if (ps.Contains(s))
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}));
            
            //App.LoadWindowSettingsAsync();

            SizeChangeBorderHelper = new SizeChangeBorderHelper(this, BorderMoveTop, BorderMoveTopLeft, BorderMoveTopRight, BorderMoveLeft, BorderMoveRight, BorderMoveButtom, BorderMoveButtomLeft, BorderMoveButtomRight);
            SizeChangeBorderHelper.SizeChanged += (sender, e) =>
            {
                windowLocation = new Point(Left / ScreenSize.Width, Top / ScreenSize.Height);
                windowSize = new Size(Width / ScreenSize.Width, Height / ScreenSize.Height);
            };
            //if (App.Core.Com == null)
            //{
            //    App.Core.Com = new Com(Dispatcher);
            //}

            NavigateTo(typeof(MainPage));

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            App.Core.LoadWindowSettings();
        }

        #region UI设计
        #region 这是设置模块,与App.cs的load与save settings会进行交互.
        bool isFullScreen = false;
        bool isMaxmize = false;
        Point windowLocation = new Point(0.2, 0.2);
        Size windowSize = new Size(0.6, 0.6);

        public bool IsFullScreen
        {
            get => isFullScreen; set
            {
                isFullScreen = value;
                SetWindowBig();
            }
        }
        public bool IsMaxmize
        {
            get => isMaxmize; set
            {
                isMaxmize = value;
                SetWindowBig();
            }
        }
        public Point WindowLocation
        {
            get => windowLocation; set
            {
                if (!IsFullScreen && !IsMaxmize)
                {
                    Left = value.X * ScreenSize.Width;
                    Top = value.Y * ScreenSize.Height;
                }

                windowLocation = value;
            }
        }
        public Size WindowSize
        {
            get => windowSize; set
            {
                if (!IsFullScreen && !IsMaxmize)
                {
                    Width = value.Width * ScreenSize.Width;
                    Height = value.Height * ScreenSize.Height;
                }
                windowSize = value;
            }
        }
        #endregion
        #region 设置辅助方法.
        /// <summary>
        /// 设置边框大小以及颜色.
        /// </summary>
        /// <param name="isZero"></param>
        void SetColumnRow(bool isZero)
        {
            if (isZero)
            {
                ColumnLeft.Width = new GridLength(0); ColumnRight.Width = new GridLength(0); RowButton.Height = new GridLength(0);
                GridContent.BorderThickness = new Thickness(0);
            }
            else
            {
                ColumnLeft.Width = new GridLength(5); ColumnRight.Width = new GridLength(5); RowButton.Height = new GridLength(5);
                GridContent.BorderThickness = new Thickness(1);
            }
        }
        /// <summary>
        /// 当窗体发生大的变动(全屏,最大化,还原,退出全屏)的统一操作.
        /// </summary>
        void SetWindowBig()
        {
            //全屏
            if (IsFullScreen)
            {
                Left = 0; Top = 0; Width = ScreenSize.Width; Height = ScreenSize.Height;
                IconButtonFullScreen.Text = "\xe73f";
                IconButtonMaximize.Visibility = Visibility.Collapsed;
                BorderMoveTop.Visibility = Visibility.Collapsed;
                SetColumnRow(true);
            }
            else
            {
                IconButtonFullScreen.Text = "\xe740";
                IconButtonMaximize.Visibility = Visibility.Visible;
                //还原
                if (!IsMaxmize)
                {
                    BorderMoveTop.Visibility = Visibility.Visible;
                    IconButtonMaximize.Text = "\xe922";
                    Left = windowLocation.X * ScreenSize.Width; Top = windowLocation.Y * ScreenSize.Height; Width = windowSize.Width * ScreenSize.Width; Height = windowSize.Height * ScreenSize.Height;
                    SetColumnRow(false);
                }
                //最大化
                else
                {
                    BorderMoveTop.Visibility = Visibility.Collapsed;
                    IconButtonMaximize.Text = "\xe923";
                    Left = 0; Top = 0; Width = ScreenSize.Width; Height = SystemParameters.MaximizedPrimaryScreenHeight;
                    SetColumnRow(true);
                }
            }
        }

        #endregion
        public Size ScreenSize => new Size(SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight);
        /// <summary>
        /// 给窗体大小调整提供辅助类.
        /// </summary>
        public SizeChangeBorderHelper SizeChangeBorderHelper { get; set; }

        /// <summary>
        /// 窗体边框颜色.
        /// </summary>
        public Brush WindowBorderBrush
        {
            get { return (Brush)GetValue(WindowBorderBrushProperty); }
            set { SetValue(WindowBorderBrushProperty, value); }
        }

        #region 为外部的交互提供封装.
        MouseEventHandler WindowMoveEventHandler;
        public event EventHandler ColorStyleChanged;

        /// <summary>
        /// 添加某一控件的标题栏特性.
        /// </summary>
        /// <param name="control"></param>
        public void SetTitleBar(FrameworkElement control)
        {
            if (WindowMoveEventHandler == null)
            {
                WindowMoveEventHandler = (sender, e) =>
                {
                    //{solve}发现改变大小可能导致的bug,故在条件中加入[&&!SizeChangeBorderHelper.IsInSizeChanging]
                    if (e.LeftButton == MouseButtonState.Pressed && !IsFullScreen && !SizeChangeBorderHelper.IsInSizeChanging)
                    {
                        //这里用到最大化时拖拽缩放的特性.
                        if (IsMaxmize)
                        {
                            windowLocation.Y = 0;
                            //以下用于调整窗体的X位置.
                            double ex = e.GetPosition(GridContent).X;
                            double rwidth = windowSize.Width * ScreenSize.Width;
                            if (ex <= rwidth / 2)
                            {
                                windowLocation.X = 0;
                            }
                            else if (ex >= ScreenSize.Width - rwidth / 2)
                            {
                                windowLocation.X = 1 - windowSize.Width;
                            }
                            else
                            {
                                windowLocation.X = (ex - rwidth / 2) / ScreenSize.Width;
                            }
                            IsMaxmize = false;
                        }
                        DragMove();
                        {
                            Point mpoint = new Point(e.GetPosition(this).X + Left, e.GetPosition(this).Y + Top);
                            if (mpoint.Y == 0)
                            {
                                IsMaxmize = true;
                                return;
                            }
                        }

                        windowLocation = new Point(Left / ScreenSize.Width, Top / ScreenSize.Height);
                    }
                };

            }
            //注册事件到匿名委托.
            control.MouseMove += WindowMoveEventHandler;
        }
        /// <summary>
        /// 移除某一控件的标题栏特性.
        /// </summary>
        /// <param name="control"></param>
        public void RemoveTitleBar(FrameworkElement control)
        {
            control.MouseMove -= WindowMoveEventHandler;
        }
        /// <summary>
        /// 将<see cref="contentFrame"/>转至某一<see cref="Page"/>
        /// </summary>
        public void NavigateTo(Type pageType)
        {
            Page page = (Page)Activator.CreateInstance(pageType);
            contentFrame.Content = page;
        }
        /// <summary>
        /// 序列化器,为<see cref="App"/>的<see cref="App.LoadWindowSettingsAsync"/>提供序列化器.
        /// </summary>
        //public XCore.Component.XSerializer<MainWindow> XSerializer;
        public void SetColorStyle(WindowColorStyle colorStyle)
        {
            IconButtonMaximize.ColorStyle = colorStyle;
            IconButtonFullScreen.ColorStyle = colorStyle;
            IconButtomMinimize.ColorStyle = colorStyle;
            IconButtomCloseWindow.ColorStyle = colorStyle;
            ColorStyleChanged?.Invoke(this, new EventArgs());
        }
        #endregion

        public static readonly DependencyProperty WindowBorderBrushProperty =
            DependencyProperty.Register("WindowBorderBrush", typeof(Brush), typeof(MainWindow), new PropertyMetadata(UserBrushes.BlueGreen));

        private void IconButtonCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            App.Core.SaveWindowSettings();
            //App.Core.SaveUserSettings();
            Close();
        }
        private void IconButtonFullScreen_Click(object sender, RoutedEventArgs e)
        {
            IsFullScreen = !IsFullScreen;
        }
        private void IconButtonMaximize_Click(object sender, RoutedEventArgs e)
        {
            IsMaxmize = !IsMaxmize;
        }
        private void IconButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        #endregion

        #region 发送消息
        public void SendMessage(string msg)
        {
            if (msg != null && msg!="")
            {
                TextBlockMsg.Text = msg;
                BorderMsg.Visibility = Visibility.Visible;
                DoubleAnimation opAnimaiton = new DoubleAnimation
                {
                    From = 1,
                    To = 0,
                    EasingFunction = new BackEase() { EasingMode = EasingMode.EaseIn },
                    Duration = new Duration(TimeSpan.FromSeconds(2)),
                };
                opAnimaiton.Completed += (s, e) => { BorderMsg.Visibility = Visibility.Collapsed; };

                BorderMsg.BeginAnimation(Border.OpacityProperty, opAnimaiton);
            }
        }
        #endregion

    }
}
