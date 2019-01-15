using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tty.com.Model
{
    public struct User_MD5
    {
        public User_MD5(string userName, string mD5) : this()
        {
            username = userName;
            md5 = mD5;
        }

        public string username { get; }
        public string md5 { get; }
    }
}
