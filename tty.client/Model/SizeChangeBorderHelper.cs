using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using XCore.Windows;

namespace tty.Model
{
    /// <summary>
    /// 窗体大小改变的辅助类.
    /// </summary>
    public class SizeChangeBorderHelper
    {
        public SizeChangeBorderHelper(Window window, params Border[] borders)
        {
            Window = window;
            Borders = borders;
            timer = new DispatcherTimer(DispatcherPriority.Normal)
            {
                Interval = TimeSpan.FromMilliseconds(50),
                IsEnabled = false,

            };
            timer.Tick += Timer_Tick;
            foreach (var item in borders)
            {
                item.MouseMove += BorderChangeSize_MouseMove;
                item.MouseDown += BorderChangeSize_MouseDown;
                item.MouseLeave += BorderChangeSize_MouseLeave;
            }
        }
        DispatcherTimer timer;
        Border isOperatingBorder;
        Point firstPoint;
        Rect area;
        public Border[] Borders { get; private set; }
        public Window Window { get; private set; }
        public bool IsInSizeChanging { get; private set; } = false;
        public double MinWidth { get; set; } = 800;
        public double MinHeight { get; set; } = 420;
        public event EventHandler SizeChanged;

        private async void Timer_Tick(object sender, EventArgs e)
        {
            await Window.Dispatcher.InvokeAsync(() =>
            {
                if (Mouse.LeftButton == MouseButtonState.Released)
                {
                    IsInSizeChanging = false;
                    timer.IsEnabled = false;
                    Mouse.OverrideCursor = null;
                    return;
                }
                else
                {
                    DirectionType directionType = GetDirectionType(isOperatingBorder).Item1;
                    Point point = WindowsTool.GetPositionFull();
                    if (directionType == DirectionType.Top || directionType == DirectionType.TopLeft || directionType == DirectionType.TopRight)
                    {
                        SetWindowSize(0, point);
                    }
                    if (directionType == DirectionType.Buttom || directionType == DirectionType.ButtomLeft || directionType == DirectionType.ButtomRight)
                    {
                        SetWindowSize(3, point);
                    }
                    if (directionType == DirectionType.Left || directionType == DirectionType.TopLeft || directionType == DirectionType.ButtomLeft)
                    {
                        SetWindowSize(1, point);
                    }
                    if (directionType == DirectionType.Right || directionType == DirectionType.TopRight || directionType == DirectionType.ButtomRight)
                    {
                        SetWindowSize(2, point);
                    }
                    SizeChanged?.Invoke(this, new EventArgs());
                }
            });

        }

        private void BorderChangeSize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            IsInSizeChanging = true;
            timer.IsEnabled = true;
            isOperatingBorder = (Border)sender;
            firstPoint = WindowsTool.GetPositionFull();
            area = new Rect(Window.Left, Window.Top, Window.Width, Window.Height);
        }
        private void BorderChangeSize_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released && e.RightButton == MouseButtonState.Released)
            {
                Mouse.OverrideCursor = GetDirectionType((Border)sender).Item2;
            }
            else if (!IsInSizeChanging)
            {
                Mouse.OverrideCursor = null;
            }
        }
        private void BorderChangeSize_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!IsInSizeChanging)
            {
                Mouse.OverrideCursor = null;
            }
        }

        /// <summary>
        /// tools to <see cref="BorderChangeSize_MouseMove(object, MouseEventArgs)"/>
        /// </summary>
        /// <param name="border"></param>
        /// <returns></returns>
        Tuple<DirectionType, Cursor> GetDirectionType(Border border)
        {
            if (border == Borders[0])
            {
                return new Tuple<DirectionType, Cursor>(DirectionType.Top, Cursors.SizeNS);
            }
            else if (border == Borders[1])
            {
                return new Tuple<DirectionType, Cursor>(DirectionType.TopLeft, Cursors.SizeNWSE);
            }
            else if (border == Borders[2])
            {
                return new Tuple<DirectionType, Cursor>(DirectionType.TopRight, Cursors.SizeNESW);
            }
            else if (border == Borders[3])
            {
                return new Tuple<DirectionType, Cursor>(DirectionType.Left, Cursors.SizeWE);
            }
            else if (border == Borders[4])
            {
                return new Tuple<DirectionType, Cursor>(DirectionType.Right, Cursors.SizeWE);
            }
            else if (border == Borders[5])
            {
                return new Tuple<DirectionType, Cursor>(DirectionType.Buttom, Cursors.SizeNS);
            }
            else if (border == Borders[6])
            {
                return new Tuple<DirectionType, Cursor>(DirectionType.ButtomLeft, Cursors.SizeNESW);
            }
            else
            {
                return new Tuple<DirectionType, Cursor>(DirectionType.ButtomRight, Cursors.SizeNWSE);
            }
        }
        /// <summary>
        /// 设置WindowSize的一部分
        /// </summary>
        /// <param name="v">0:Top,1:Left,2:Right,3:Buttom</param>
        private void SetWindowSize(double v, Point point)
        {
            if (v == 0)
            {
                double height = area.Height - (point.Y - firstPoint.Y);
                if (height >= MinHeight)
                {
                    Window.Height = height;
                }
                else
                {
                    Window.Height = MinHeight;
                }
                Window.Top = area.Bottom - Window.Height;
            }
            else if (v == 3)
            {
                double height = area.Height + (point.Y - firstPoint.Y);
                if (height >= MinHeight)
                {
                    Window.Height = height;
                }
                else
                {
                    Window.Height = MinHeight;
                }
            }
            else if (v == 1)
            {
                double width = area.Width - (point.X - firstPoint.X);
                if (width >= MinWidth)
                {
                    Window.Width = width;

                }
                else
                {
                    Window.Width = MinWidth;
                }
                Window.Left = area.Right - Window.Width;
            }
            else if (v == 2)
            {
                double width = area.Width + (point.X - firstPoint.X);
                if (width >= MinWidth)
                {
                    Window.Width = width;

                }
                else
                {
                    Window.Width = MinWidth;
                }
            }
        } 
    }
}
