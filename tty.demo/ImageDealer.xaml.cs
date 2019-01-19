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

namespace tty.demo
{
    /// <summary>
    /// ImageDealer.xaml 的交互逻辑
    /// </summary>
    public partial class ImageDealer : UserControl
    {
        private const int MINSIZE = 30;
        private int movingElp = -1;
        public ImageDealer()
        {
            InitializeComponent();
        }
        
        private void SetRectangle()
        {
            Size canvasSize;
            Rect rect = GetRect(out canvasSize);

            //移动元素
            rtLeft.Width = rect.Left; rtLeft.Height = canvasSize.Height;
            rtRight.Width = canvasSize.Width - rect.Right; rtRight.Height = canvasSize.Height; rtRight.Margin = new Thickness(rect.Right, 0, 0, 0);
            rtTop.Width = rect.Width; rtTop.Height = rect.Top; rtTop.Margin = new Thickness(rect.Left, 0, 0, 0);
            rtBottom.Width = rect.Width; rtBottom.Height = canvasSize.Height - rect.Bottom; rtBottom.Margin = new Thickness(rect.Left, rect.Bottom, 0, 0);
            bdCenter.Width = rect.Width; bdCenter.Height = rect.Height; bdCenter.Margin = new Thickness(rect.Left, rect.Top, 0, 0);
            elpLeftTop.Margin = new Thickness(rect.Left, rect.Top, 0, 0);
            elpRightTop.Margin = new Thickness(rect.Right, rect.Top, 0, 0);
            elpLeftBottom.Margin = new Thickness(rect.Left, rect.Bottom, 0, 0);
            elpRightBottom.Margin = new Thickness(rect.Right, rect.Bottom, 0, 0);
        }
        private Rect GetRect(out Size canvasSize)
        {
            canvasSize = new Size(Width - 40, Height - 40);
            Size picSize = new Size();
            if (Source != null)
            {
                picSize = new Size(Source.Width, Source.Height);
            }
            else
            {
                picSize = canvasSize;
            }
            Rect rect = new Rect();

            //默认最大截图方式。
            if (SelectArea == null)
            {
                if (ClipMode == ClipMode.Any)
                {
                    if ((picSize.Width / picSize.Height) >= (canvasSize.Width / canvasSize.Height))
                    {
                        var mheight = (picSize.Height / picSize.Width) * canvasSize.Width;
                        rect = new Rect(0, (canvasSize.Height - mheight) / 2, canvasSize.Width, mheight);
                    }
                    else
                    {
                        var mwidth = (picSize.Width / picSize.Height) * canvasSize.Height;
                        rect = new Rect((canvasSize.Width - mwidth) / 2, 0, mwidth, canvasSize.Height);
                    }
                }
                else
                {
                    if ((picSize.Width / picSize.Height) >= (canvasSize.Width / canvasSize.Height))
                    {
                        var mheight = (picSize.Height / picSize.Width) * canvasSize.Width;
                        rect = new Rect((canvasSize.Width - mheight) / 2, (canvasSize.Height - mheight) / 2, mheight, mheight);
                    }
                    else
                    {
                        var mwidth = (picSize.Width / picSize.Height) * canvasSize.Height;
                        rect = new Rect((canvasSize.Width - mwidth) / 2, (canvasSize.Height - mwidth) / 2, mwidth, mwidth);
                    }
                }
            }
            //非默认截图。
            else
            {
                rect = SelectArea.Value;
            }

            return rect;
        }
        private void Elp_HandleMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                int corner = movingElp;

                Point p = e.GetPosition(cv1);
                Rect limit = new Rect();
                Rect rect = GetRect(out Size canvasSize);
                Size picSize = new Size();
                if (Source != null)
                {
                    picSize = new Size(Source.Width, Source.Height);
                }
                else
                {
                    picSize = canvasSize;
                }

                //计算鼠标移动的限制区域
                if (ClipMode == ClipMode.Any)
                {
                    if (corner == 0)
                    {
                        movingElp = 0;
                        if ((picSize.Width / picSize.Height) >= (canvasSize.Width / canvasSize.Height))
                        {
                            var mheight = (picSize.Height / picSize.Width) * canvasSize.Width;
                            limit = new Rect(0, (canvasSize.Height - mheight) / 2, rect.Right - MINSIZE, rect.Bottom - MINSIZE - (canvasSize.Height - mheight) / 2);
                        }
                        else
                        {
                            var mwidth = (picSize.Width / picSize.Height) * canvasSize.Height;
                            limit = new Rect((canvasSize.Width - mwidth) / 2, 0, rect.Right - MINSIZE - (canvasSize.Width - mwidth) / 2, rect.Bottom - MINSIZE);
                        }
                    }
                    else if (corner == 1)
                    {
                        movingElp = 1;
                        if ((picSize.Width / picSize.Height) >= (canvasSize.Width / canvasSize.Height))
                        {
                            var mheight = (picSize.Height / picSize.Width) * canvasSize.Width;
                            limit = new Rect(rect.Left + MINSIZE,(canvasSize.Height - mheight) /2, canvasSize.Width - (rect.Left + MINSIZE),rect.Bottom - MINSIZE - ((canvasSize.Height - mheight) / 2));
                        }
                        else
                        {
                            var mwidth = (picSize.Width / picSize.Height) * canvasSize.Height;
                            limit = new Rect(rect.Left + MINSIZE, 0, rect.Right - (rect.Left + MINSIZE), rect.Bottom - MINSIZE);
                        }

                    }
                    else if (corner == 2)
                    {
                        movingElp = 2;
                        if ((picSize.Width / picSize.Height) >= (canvasSize.Width / canvasSize.Height))
                        {
                            var mheight = (picSize.Height / picSize.Width) * canvasSize.Width;
                            limit = new Rect(0, rect.Top + MINSIZE, rect.Right - MINSIZE, (canvasSize.Height - mheight) / 2 + mheight - (rect.Top + MINSIZE));
                        }
                        else
                        {
                            var mwidth = (picSize.Width / picSize.Height) * canvasSize.Height;
                            limit = new Rect((canvasSize.Width - mwidth) / 2, rect.Top + MINSIZE, rect.Right - MINSIZE - ((canvasSize.Width - mwidth) / 2), canvasSize.Height - (rect.Top + MINSIZE));
                        }
                    }
                    else if (corner == 3)
                    {
                        movingElp = 3;
                        if ((picSize.Width / picSize.Height) >= (canvasSize.Width / canvasSize.Height))
                        {
                            var mheight = (picSize.Height / picSize.Width) * canvasSize.Width;
                            limit = new Rect(rect.Left + MINSIZE,rect.Top + MINSIZE,canvasSize.Width - (rect.Left + MINSIZE), (canvasSize.Height - mheight) /2 + mheight - (rect.Top + MINSIZE));
                        }
                        else
                        {
                            var mwidth = (picSize.Width / picSize.Height) * canvasSize.Height;
                            limit = new Rect(rect.Left + MINSIZE, rect.Top + MINSIZE, (canvasSize.Width - mwidth) / 2 + mwidth - (rect.Left + MINSIZE), canvasSize.Height - (rect.Top + MINSIZE));
                        }
                    }
                }

                //调整响应的坐标
                Point re = p;
                if (p.X < limit.Left) re.X = limit.Left;
                if (p.Y < limit.Top) re.Y = limit.Top;
                if (p.X > limit.Right) re.X = limit.Right;
                if (p.Y > limit.Bottom) re.Y = limit.Bottom;

                //应用移动
                if (ClipMode == ClipMode.Any)
                {
                    //左上角
                    if (corner == 0)
                    {
                        SelectArea = new Rect(re.X, re.Y, rect.Right - re.X, rect.Bottom - re.Y);
                    }
                    else if (corner == 1)
                    {
                        SelectArea = new Rect(rect.Left, re.Y, re.X - rect.Left, rect.Bottom - re.Y);
                    }
                    else if (corner == 2)
                    {
                        SelectArea = new Rect(re.X, rect.Top, rect.Right - re.X, re.Y - rect.Top);
                    }
                    else if(corner == 3)
                    {
                        SelectArea = new Rect(rect.Left, rect.Top, re.X - rect.Left, re.Y - rect.Top);
                    }
                }
            }

        }

        public BitmapSource Source
        {
            get { return (BitmapSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        public Rect? SelectArea
        {
            get { return (Rect?)GetValue(SelectAreaRelativeProperty); }
            set { SetValue(SelectAreaRelativeProperty, value); }
        }
        public ClipMode ClipMode
        {
            get { return (ClipMode)GetValue(ClipModeProperty); }
            set { SetValue(ClipModeProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(BitmapSource), typeof(ImageDealer), new PropertyMetadata(null, (d, e) => { ((ImageDealer)d).SetRectangle(); }));
        public static readonly DependencyProperty SelectAreaRelativeProperty =
            DependencyProperty.Register("SelectAreaRelative", typeof(Rect?), typeof(ImageDealer), new PropertyMetadata(null, (d, e) => { ((ImageDealer)d).SetRectangle(); }));
        public static readonly DependencyProperty ClipModeProperty =
            DependencyProperty.Register("ClipMode", typeof(ClipMode), typeof(ImageDealer), new PropertyMetadata(ClipMode.Any, (d, e) => { ((ImageDealer)d).SetRectangle(); }));

        private void ImageDealer_Loaded(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.MouseMove += Window_MouseMove;
            window.MouseUp += Window_MouseUp;
        }

        //--------****---------
        //为了扩大鼠标操作控件，移动事件转移到其宿主的Window对象。
        //---------------------
        //private void ImageDealer_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (movingElp == 0)
        //    {
        //        Elp_HandleMove(elpLeftTop, e);
        //    }
        //    else if (movingElp == 2)
        //    {
        //        Elp_HandleMove(elpLeftBottom, e);
        //    }

        //}
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            Elp_HandleMove( e);
        }
        //private void ImageDealer_MouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    movingElp = -1;
        //}
        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            movingElp = -1;
        }
        private void Elp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (sender == elpLeftTop)
                    movingElp = 0;
                else if (sender == elpRightTop)
                    movingElp = 1;
                else if (sender == elpLeftBottom)
                    movingElp = 2;
                else
                    movingElp = 3;
            }
        }


    }

    public enum ClipMode
    {
        Any,
        Square,
        //H16_9,
        //H4_3,
    }
}
