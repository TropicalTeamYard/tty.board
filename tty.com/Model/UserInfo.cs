using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace tty.com.Model
{
    public class UserInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _username = "";
        private string _nickname = "";
        private string _portrait = "";
        private string _usertype = "COMMON";
        private string _email = "";
        private string _phone = "";
        private UserState _userstate = UserState.PasswordError;

        public string username { get => _username;set
            {
                _username = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(username)));
            }
        }
        public string nickname { get => _nickname; set
            {
                _nickname = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(nickname)));
            }
        }
        public string usertype { get => _usertype;set
            {
                _usertype = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(usertype)));
            }
        }
        public string email { get => _email; set
            {
                _email = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(email)));
            }
        }
        public string phone { get => _phone;set
            {
                _phone = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(phone)));
            }
        }
        public UserState userstate { get => _userstate;set
            {
                _userstate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(userstate)));
            }
        }

        public string credit { get; set; }


        public void CopyFrom(UserInfo other)
        {
            username = other.username;
            nickname = other.nickname;
            usertype = other.usertype;
            email = other.email;
            phone = other.phone;
            
        }
    }

    public enum UserState
    {
        PasswordError,
        Success,
    }
}
