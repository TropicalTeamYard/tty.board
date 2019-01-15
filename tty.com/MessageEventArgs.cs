using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tty.com
{
    public class MessageEventArgs:EventArgs
    {
        public MessageEventArgs(bool status, string msg, object data = null)
        {
            Status = status;
            Msg = msg;
            Data = data;
        }

        public bool Status { get; private set; }
        public string Msg { get; private set; }
        public object Data { get; private set; }
    }

    public enum MessageStatus
    {
        Success,
        Failed,
        Error,
    }
}
