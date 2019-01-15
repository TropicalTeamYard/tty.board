using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace tty.com.Model
{
    public class MsgUni : INotifyPropertyChanged
    {
        public MsgUni()
        {
        }
        public MsgUni(string username, string time, string content, byte[] pic = null)
        {
            this.username = username;
            this.time = time;
            this.content = content;
            this.pic = pic;
        }

        private string _username = "";
        private string _time = "1970-01-01 08:00:00";
        private string _content = "";
        private string _nickname = "昵称";
        private BitmapImage _portrait = null;

        public int id { get; set; }
        public string username
        {
            get => _username;
            set
            {
                _username = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(username)));
            }
        }
        public string time
        {
            get => _time;
            set
            {
                _time = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(time)));
            }
        }
        public int istop { get; set; } = 0;
        public int islocked { get; set; } = 0;
        public string content
        {
            get => _content;
            set
            {
                _content = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(content)));
            }
        }
        public string nickname
        {
            get => _nickname;
            set
            {
                _nickname = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(nickname)));
            }
        }
        [JsonIgnore]
        public BitmapImage portrait
        {
            get => _portrait;
            set
            {
                _portrait = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(portrait)));
            }
        }

        /// <summary>
        /// 留言标记，0表示正常，2表示删除。
        /// </summary>
        public int mark { get; set; }

        // TODO 写入数据库
        // [SqlElement]
        // [SqlBinding("body")]
        public byte[] pic { get; set; }
        public MsgComment[] comments { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
