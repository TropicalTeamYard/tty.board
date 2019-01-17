using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tty.demo
{
        /// <summary>

        /// 鼠标八大位置枚举

        /// </summary>

        public enum MouseLocationEnum

        {

            None,

            Left,

            Up,

            Right,

            Down,

            LeftUp,

            LeftDown,

            RightUp,

            RightDown

        }

        /// <summary>

        /// 

        /// </summary>

        public class RectangleAreaModel

        {

            public double X { get; set; }

            public double Y { get; set; }

            public double Width { get; set; }

            public double Height { get; set; }

        }

        /// <summary>

        /// 鼠标状态

        /// </summary>

        public enum MouseActionEx

        {

            /// <summary>

            /// 无

            /// </summary>

            None,

            /// <summary>

            /// 拖拽

            /// </summary>

            Drag,

            /// <summary>

            /// 拖动

            /// </summary>

            DragMove

        }



        public enum SculptureAction

        {

            Nomal,

            Video,

            Image,

        }



    }
