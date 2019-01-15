using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tty.com.Model
{
    public class Time_Msg
    {
        public Time_Msg()
        {
        }

        public string time { get; set; } = "";
        public MsgUni[] content { get; set; } = new MsgUni[0];
    }
}
