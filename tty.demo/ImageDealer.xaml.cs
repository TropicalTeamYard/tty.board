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
    public partial class ImageDealer : UserControl
    {
        public static readonly RoutedEvent OnCutImagingEventHandler = EventManager.RegisterRoutedEvent("OnCutImaging", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ImageDealer));
        #region 私有字段
        /// <summary>
        /// 截图控件
        /// </summary>
        private ImageDealerUnsafe _ImageDealerControl = new ImageDealerUnsafe();
        /// <summary>
        /// 图片源
        /// </summary>
        private BitmapImage _BitSource;
        private int _ChangeMargin = 1;
        #endregion
        #region 公共字段
        /// <summary>
        /// 图片源
        /// </summary>
        public BitmapImage BitSource
        {
            get { return this._BitSource; }
            set
            {
                this._BitSource = value;
                this._ImageDealerControl.BitSource = value;
                LocateInit();
            }
        }
        /// <summary>
        /// 截图事件
        /// </summary>
        public event RoutedEventHandler OnCutImaging
        {
            add { base.AddHandler(OnCutImagingEventHandler, value); }
            remove { base.RemoveHandler(OnCutImagingEventHandler, value); }
        }
        #endregion
        #region ==方法==
        public ImageDealer()
        {
            InitializeComponent();
            this._ImageDealerControl.OnCutImage += this.OnCutImage;
        }
        //外部截图
        public void CutImage()
        {
            if (this.IsLoaded == true || this._ImageDealerControl == null)
            {
                this._ImageDealerControl.CutImage();
            }
            else
            {
                throw new Exception("尚未创建视图时无法截图！");
            }
        }
        //截图控件位置初始化
        private void LocateInit()
        {
            double Margin = 1;
            if (this._BitSource != null)
            {
                double percent = 1;
                //根据最小倍率放大截图控件
                percent = (this._BitSource.PixelHeight * 1.0 / this.ActualHeight);
                percent = percent < (this._BitSource.PixelWidth * 1.0 / this.ActualWidth) ? (this._BitSource.PixelWidth * 1.0 / this.ActualWidth) : percent;
                this._ImageDealerControl.Width = this._BitSource.PixelWidth * 1.0 / percent;
                this._ImageDealerControl.Height = this._BitSource.PixelHeight * 1.0 / percent;
                //初始化截图方块
                this._ImageDealerControl.ImageArea.Width = this._ImageDealerControl.ImageArea.Height = 100 + _ChangeMargin;
                _ChangeMargin = -_ChangeMargin;
                this._ImageDealerControl.ImageArea.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);
                this._ImageDealerControl.ImageArea.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Center);
                this._ImageDealerControl.ImageArea.SetValue(MarginProperty, new Thickness(0));
                //截图控件相对父控件Margin
                this._ImageDealerControl.Width -= 2 * Margin;
                this._ImageDealerControl.Height -= 2 * Margin;
                this._ImageDealerControl.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);
                this._ImageDealerControl.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Center);
                this._ImageDealerControl.SetValue(MarginProperty, new Thickness(0));
            }
        }
        #endregion
        #region ==事件==
        private void OnCutImage(BitmapSource bit)
        {
            RaiseEvent(new RoutedEventArgs(OnCutImagingEventHandler, bit));
        }
        //加载完成
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.MainGrid.Children.Contains(this._ImageDealerControl) == false)
            {
                this.MainGrid.Children.Add(this._ImageDealerControl);
                this._ImageDealerControl.Width = this.ActualWidth;
                this._ImageDealerControl.Height = this.ActualHeight;
                this._ImageDealerControl.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);
                this._ImageDealerControl.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Center);
                this._ImageDealerControl.SetValue(MarginProperty, new Thickness(0));
            }
        }
        #endregion
    }
}
