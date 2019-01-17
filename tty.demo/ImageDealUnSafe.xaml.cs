using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

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

    /// ImageDealerUnsafe.xaml 的交互逻辑

    /// </summary>

    public partial class ImageDealerUnsafe : UserControl

    {



        #region 公共字段

        //截图回调

        public delegate void CutImageDelegate(BitmapSource bit);

        public CutImageDelegate OnCutImage;

        //图片原

        private BitmapImage _BitSource;

        public BitmapImage BitSource

        {

            get { return this._BitSource; }

            set

            {

                this._BitSource = value;

                this.SoureceImage.Source = value;

            }

        }

        #endregion



        #region 依赖属性



        /// <summary>

        /// 边距

        /// </summary>

        public double MaxMargin = 2;



        //public Brush BorderBrush;



        #endregion



        #region ==私有字段==

        /// <summary>

        /// 鼠标样式

        /// </summary>

        private Cursor MouseCursor = Cursors.Arrow;

        /// <summary>

        /// 鼠标位置

        /// </summary>

        private MouseLocationEnum MouseLocation = MouseLocationEnum.None;

        /// <summary>

        /// 鼠标行为

        /// </summary>

        private MouseActionEx Action { get; set; }

        /// <summary>

        /// 边框粗细

        /// </summary>

        private double BorderWidth = 1;

        /// <summary>

        /// 拖拽前鼠标按下位置

        /// </summary>

        private Point MouseDownPoint;

        /// <summary>

        /// 拖拽前控件位置

        /// </summary>

        private Point MouseDownLocate;



        #endregion



        #region ==方法==

        public ImageDealerUnsafe()

        {

            InitializeComponent();

        }

        /// <summary>

        /// 计算区域圆点及宽高

        /// </summary>

        /// <param name="MouseButtonLocate">鼠标相对背景MainGrid位置</param>

        /// <param name="IsRectangle">是否正方形</param>

        /// <returns>NULL 或 具体值</returns>

        private RectangleAreaModel CalculatedArea(Point MouseButtonLocate, bool IsRectangle)

        {

            Point Locate = this.ImageArea.TransformToAncestor((UIElement)this.MainGrid).Transform(new Point(0, 0));

            //边框宽度

            double BorderWidth = this.BorderWidth;

            //整体宽度

            double RectWidth = this.ImageArea.ActualWidth;

            //整体高度

            double RectHeight = this.ImageArea.ActualHeight;



            //裁剪区域

            Point OriginalPoint = new Point(0, 0);//圆点坐标

            Point TheoryPoint = new Point(0, 0);  //理论坐标

            double TheoryWidth = 0;   //理论宽度

            double TheoryHeight = 0;  //理论高度

            switch (MouseLocation)

            {

                case MouseLocationEnum.Left:

                    {

                        this.Cursor = Cursors.SizeWE;



                        OriginalPoint = new Point(Locate.X + RectWidth - BorderWidth / 2, Locate.Y + RectHeight / 2);//右中部位置

                        TheoryWidth = OriginalPoint.X - MouseButtonLocate.X + BorderWidth;

                        TheoryHeight = IsRectangle == true ? TheoryWidth : RectHeight;



                        TheoryPoint = new Point(OriginalPoint.X + BorderWidth / 2 - TheoryWidth, OriginalPoint.Y - TheoryHeight / 2);

                    }

                    break;

                case MouseLocationEnum.LeftUp:

                    {

                        this.Cursor = Cursors.SizeNWSE;



                        OriginalPoint = new Point(Locate.X + RectWidth - BorderWidth / 2, Locate.Y + RectHeight - BorderWidth / 2);//右下部位置

                        TheoryWidth = OriginalPoint.X - MouseButtonLocate.X + BorderWidth;

                        TheoryHeight = IsRectangle == true ? TheoryWidth : OriginalPoint.Y - MouseButtonLocate.Y + BorderWidth;



                        TheoryPoint = new Point(OriginalPoint.X + BorderWidth / 2 - TheoryWidth, OriginalPoint.Y + BorderWidth / 2 - TheoryHeight);

                    }

                    break;

                case MouseLocationEnum.Up:

                    {

                        this.Cursor = Cursors.SizeNS;



                        OriginalPoint = new Point(Locate.X + RectWidth / 2, Locate.Y + RectHeight - BorderWidth / 2);//下中部位置

                        TheoryHeight = OriginalPoint.Y - MouseButtonLocate.Y + BorderWidth;

                        TheoryWidth = IsRectangle == true ? TheoryHeight : RectWidth;



                        TheoryPoint = new Point(OriginalPoint.X - TheoryWidth / 2, OriginalPoint.Y + BorderWidth / 2 - TheoryHeight);

                    }

                    break;

                case MouseLocationEnum.RightUp:

                    {

                        this.Cursor = Cursors.SizeNESW;



                        OriginalPoint = new Point(Locate.X + BorderWidth / 2, Locate.Y + RectHeight - BorderWidth / 2);//左下部位置

                        TheoryWidth = MouseButtonLocate.X - OriginalPoint.X + BorderWidth;

                        TheoryHeight = IsRectangle == true ? TheoryWidth : MouseButtonLocate.Y - OriginalPoint.Y + BorderWidth;



                        TheoryPoint = new Point(OriginalPoint.X - BorderWidth / 2, OriginalPoint.Y + BorderWidth / 2 - TheoryHeight);

                    }

                    break;

                case MouseLocationEnum.Right:

                    {

                        this.Cursor = Cursors.SizeWE;



                        OriginalPoint = new Point(Locate.X + BorderWidth / 2, Locate.Y + RectHeight / 2);//左中部位置

                        TheoryWidth = MouseButtonLocate.X - OriginalPoint.X + BorderWidth;

                        TheoryHeight = IsRectangle == true ? TheoryWidth : RectHeight;



                        TheoryPoint = new Point(OriginalPoint.X - BorderWidth / 2, OriginalPoint.Y - TheoryHeight / 2);

                    }

                    break;

                case MouseLocationEnum.RightDown:

                    {

                        this.Cursor = Cursors.SizeNWSE;



                        OriginalPoint = new Point(Locate.X + BorderWidth / 2, Locate.Y + BorderWidth / 2);//左上部位置

                        TheoryWidth = MouseButtonLocate.X - OriginalPoint.X + BorderWidth;

                        TheoryHeight = IsRectangle == true ? TheoryWidth : MouseButtonLocate.Y - OriginalPoint.Y + BorderWidth;



                        TheoryPoint = new Point(OriginalPoint.X - BorderWidth / 2, OriginalPoint.Y - BorderWidth / 2);

                    }

                    break;

                case MouseLocationEnum.Down:

                    {

                        this.Cursor = Cursors.SizeNS;



                        OriginalPoint = new Point(Locate.X + RectWidth / 2, Locate.Y + BorderWidth / 2);//上中部位置

                        TheoryHeight = MouseButtonLocate.Y - OriginalPoint.Y + BorderWidth;

                        TheoryWidth = IsRectangle == true ? TheoryHeight : RectWidth;



                        TheoryPoint = new Point(OriginalPoint.X - TheoryWidth / 2, OriginalPoint.Y - BorderWidth / 2);

                    }

                    break;

                case MouseLocationEnum.LeftDown:

                    {

                        this.Cursor = Cursors.SizeNESW;



                        OriginalPoint = new Point(Locate.X + RectWidth - BorderWidth / 2, Locate.Y + BorderWidth / 2);//右上部位置

                        TheoryWidth = OriginalPoint.X - MouseButtonLocate.X + BorderWidth;

                        TheoryHeight = IsRectangle == true ? TheoryWidth : OriginalPoint.Y - MouseButtonLocate.Y + BorderWidth;



                        TheoryPoint = new Point(OriginalPoint.X + BorderWidth / 2 - TheoryWidth, OriginalPoint.Y - BorderWidth / 2);

                    }

                    break;

                default:

                    return null;

            }

            return new RectangleAreaModel()

            {

                X = TheoryPoint.X,

                Y = TheoryPoint.Y,

                Width = TheoryWidth,

                Height = TheoryHeight

            };

        }



        /// <summary>

        /// 图片剪切

        /// </summary>

        public void CutImage()

        {

            if (this.BitSource != null)

            {

                try

                {

                    double ImageAreaWidth = this.ImageArea.ActualWidth;

                    double ImageAreaHeight = this.ImageArea.ActualHeight;

                    double GridWidth = this.MainGrid.ActualWidth;

                    double GridHeight = this.MainGrid.ActualHeight;



                    BitmapSource source = (BitmapSource)this.BitSource;

                    //计算比例

                    Point Locate = this.ImageArea.TransformToAncestor((UIElement)this.MainGrid).Transform(new Point(0, 0));

                    int dWidth = (int)((ImageAreaWidth * 1.0 / GridWidth) * source.PixelWidth);

                    int dHeight = (int)((ImageAreaHeight * 1.0 / GridHeight) * source.PixelHeight);

                    int dLeft = (int)((Locate.X * 1.0 / GridWidth) * source.PixelWidth);

                    int dTop = (int)((Locate.Y * 1.0 / GridHeight) * source.PixelHeight);

                    //像素区域

                    Int32Rect cutRect = new Int32Rect(dLeft, dTop, dWidth, dHeight);

                    //数组字节数

                    int stride = source.Format.BitsPerPixel * cutRect.Width / 8;

                    byte[] data = new byte[cutRect.Height * stride];

                    source.CopyPixels(cutRect, data, stride, 0);

                    //创建

                    BitmapSource bit = BitmapSource.Create(dWidth, dHeight, 0, 0, PixelFormats.Bgr32, null, data, stride);

                    //通知订阅

                    if (this.OnCutImage != null)

                    {

                        OnCutImage(bit);

                    }

                }

                catch

                {



                }

            }

        }

        /// <summary>

        /// 视图转图片

        /// </summary>

        /// <param name="vsual"></param>

        /// <param name="nLeft"></param>

        /// <param name="nTop"></param>

        /// <param name="nWidth"></param>

        /// <param name="nHeight"></param>

        /// <returns></returns>

        private RenderTargetBitmap RenderVisaulToBitmap(Visual vsual, int nLeft, int nTop, int nWidth, int nHeight)

        {

            var rtb = new RenderTargetBitmap(nWidth, nHeight, nLeft, nTop, PixelFormats.Default);

            rtb.Render(vsual);



            return rtb;

        }

        /// <summary>

        /// Bitmap转图片

        /// </summary>

        /// <param name="bmp"></param>

        /// <returns></returns>

        public BitmapSource ToBitmapSource(System.Drawing.Bitmap bmp)

        {

            BitmapSource returnSource;



            try

            {

                returnSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            }

            catch

            {

                returnSource = null;

            }



            return returnSource;



        }

        #endregion



        #region ==事件==



        //按下鼠标

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)

        {

            this.MouseLocation = MouseLocationEnum.None;

            if (e.OriginalSource.GetType() == typeof(Rectangle))

            {

                Rectangle Act = e.OriginalSource as Rectangle;

                switch (Act.Name)

                {

                    case "R_Left": MouseLocation = MouseLocationEnum.Left; break;

                    case "R_LeftUp": MouseLocation = MouseLocationEnum.LeftUp; break;

                    case "R_Up": MouseLocation = MouseLocationEnum.Up; break;

                    case "R_RightUp": MouseLocation = MouseLocationEnum.RightUp; break;

                    case "R_Right": MouseLocation = MouseLocationEnum.Right; break;

                    case "R_RightDown": MouseLocation = MouseLocationEnum.RightDown; break;

                    case "R_Down": MouseLocation = MouseLocationEnum.Down; break;

                    case "R_LeftDown": MouseLocation = MouseLocationEnum.LeftDown; break;

                    default: MouseLocation = MouseLocationEnum.None; break;

                }



                this.Action = MouseActionEx.Drag;

            }

            else

            {

                this.MouseDownPoint = Mouse.GetPosition(e.Source as FrameworkElement);//WPF方法

                this.MouseDownLocate = this.ImageArea.TransformToAncestor((UIElement)this.MainGrid).Transform(new Point(0, 0));

                if ((this.MouseDownLocate.X < this.MouseDownPoint.X && this.MouseDownPoint.X < this.MouseDownLocate.X + this.ImageArea.ActualWidth) &&

                    (this.MouseDownLocate.Y < this.MouseDownPoint.Y && this.MouseDownPoint.Y < this.MouseDownLocate.Y + this.ImageArea.ActualHeight)

                    )

                {

                    this.Action = MouseActionEx.DragMove;

                }

            }



        }

        //弹起鼠标

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)

        {

            this.Action = MouseActionEx.None;

            this.Cursor = Cursors.Arrow;

        }

        //移动鼠标

        private void UserControl_MouseMove(object sender, MouseEventArgs e)

        {

            //鼠标相对空间区域位置

            Point MousePoint = e.GetPosition((IInputElement)this.MainGrid);

            Point ImageLocate = this.ImageArea.TransformToAncestor((UIElement)this.MainGrid).Transform(new Point(0, 0));

            if (ImageLocate.X <= MousePoint.X && MousePoint.X <= ImageLocate.X + this.ImageArea.ActualWidth &&

                ImageLocate.Y <= MousePoint.Y && MousePoint.Y <= ImageLocate.Y + this.ImageArea.ActualHeight)

            {

                this.Cursor = Cursors.Hand;

            }

            else

            {

                this.Cursor = Cursors.Arrow;

            }

            //边框拉伸

            if (this.Action == MouseActionEx.Drag)

            {

                this.Cursor = this.MouseCursor;

                //剪辑图片区域宽高

                double ImageAreaWidth = this.ImageArea.ActualWidth;

                double ImageAreaHeight = this.ImageArea.ActualHeight;



                //裁剪区域理论位置

                RectangleAreaModel Model = this.CalculatedArea(MousePoint, true);

                if (Model != null)

                {

                    //不能超出边界区域

                    if (Model.X + Model.Width + MaxMargin > this.ActualWidth ||

                        Model.Y + Model.Height + MaxMargin > this.ActualHeight ||

                        Model.X < MaxMargin ||

                        Model.Y < MaxMargin

                        )

                    {

                        this.Cursor = Cursors.Arrow;

                        this.Action = MouseActionEx.None;

                        return;

                    }

                    this.ImageArea.Width = Model.Width;

                    this.ImageArea.Height = Model.Height;



                    this.ImageArea.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Left);

                    this.ImageArea.SetValue(VerticalAlignmentProperty, VerticalAlignment.Top);

                    this.ImageArea.SetValue(MarginProperty, new Thickness(Model.X, Model.Y, 0, 0));



                    CutImage();

                }

            }

            else if (this.Action == MouseActionEx.DragMove)//拖动

            {

                double Left = this.MouseDownLocate.X + (MousePoint.X - MouseDownPoint.X);

                double Top = this.MouseDownLocate.Y + (MousePoint.Y - MouseDownPoint.Y);

                //不能超出边界区域

                if (Left < MaxMargin ||

                    Top < MaxMargin ||

                    (Left + this.ImageArea.ActualWidth + MaxMargin) > this.ActualWidth ||

                    (Top + this.ImageArea.ActualHeight + MaxMargin) > this.ActualHeight)

                {

                    this.Cursor = Cursors.Arrow;

                    this.Action = MouseActionEx.None;

                    return;

                }

                this.ImageArea.Width = this.ImageArea.ActualWidth;

                this.ImageArea.Height = this.ImageArea.ActualHeight;



                this.ImageArea.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Left);

                this.ImageArea.SetValue(VerticalAlignmentProperty, VerticalAlignment.Top);

                this.ImageArea.SetValue(MarginProperty, new Thickness(Left, Top, 0, 0));



                CutImage();

            }

            else

            {

                //this.Cursor = Cursors.Arrow;

            }

        }

        //鼠标离开

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)

        {

            this.Action = MouseActionEx.None;



        }

        //加载完成后截图

        private void ImageArea_SizeChanged(object sender, SizeChangedEventArgs e)

        {

            this.CutImage();

        }



        #endregion

    }

}
