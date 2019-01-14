using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tty.com.Model;

namespace tty.com.Data
{
    public class User
    {
        public UserInfo Current { get; set; }
        public UserInfo[] Users { get; set; }
    }
}
