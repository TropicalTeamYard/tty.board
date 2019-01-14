using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Forms = System.Windows.Forms;

namespace XCore.Windows
{
    public static class WindowsTool
    {
        public static Point GetPositionFull()
        {
            Point abserbPoint = new Point(Forms.Cursor.Position.X, Forms.Cursor.Position.Y);
            Size screenSize = new Size(Forms.Screen.PrimaryScreen.Bounds.Width, Forms.Screen.PrimaryScreen.Bounds.Height);
            Vector dpi = new Vector(screenSize.Width / SystemParameters.PrimaryScreenWidth, screenSize.Height / SystemParameters.PrimaryScreenHeight);



            return new Point(abserbPoint.X / dpi.X, abserbPoint.Y / dpi.Y);

        }
    }

}
