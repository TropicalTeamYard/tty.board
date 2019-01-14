using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace tty.Model
{
    /// <summary>
    /// 提供预设的brush.
    /// </summary>
    public static class UserBrushes
    {
        /// <summary>
        /// 20%alpha黑色(0,0,0)
        /// </summary>
        public static Brush MereBlack => new SolidColorBrush(Color.FromArgb(51,0,0,0));
        /// <summary>
        /// 20%alpha白色(255,255,255)
        /// </summary>
        public static Brush MereWhite => new SolidColorBrush(Color.FromArgb(51, 255, 255, 255));
        /// <summary>
        /// 30%alpha黑色(0,0,0)
        /// </summary>
        public static Brush MediumBlack => new SolidColorBrush(Color.FromArgb(77, 0, 0, 0));
        /// <summary>
        /// 30%alpha白色(255,255,255)
        /// </summary>
        public static Brush MediumWhite => new SolidColorBrush(Color.FromArgb(77, 255, 255, 255));
        /// <summary>
        /// 淡红色
        /// </summary>
        public static Brush LightRed => new SolidColorBrush(Color.FromRgb(232, 162, 162));
        /// <summary>
        /// 略暗的红色
        /// </summary>
        public static Brush MereDarkRed => new SolidColorBrush(Color.FromRgb(232, 0, 0));
        /// <summary>
        /// 蓝绿色
        /// </summary>
        public static Brush BlueGreen => new SolidColorBrush(Color.FromRgb(3, 131, 135));
    }
}
