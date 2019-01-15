using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tty.com.Model;

namespace tty.com.Data
{
    public class Msg
    {
        public DateTime UpdateTime { get; set; }

        [JsonIgnore]
        public ObservableCollection<MsgUni> Msgs { get; set; } = new ObservableCollection<MsgUni>();
        public MsgUni[] msgs { get => Msgs.ToArray(); set
            {
                Msgs = new ObservableCollection<MsgUni>(value);
            }
        }
    }
}
