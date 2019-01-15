using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using tty.com.Data;
using tty.com.Model;
using tty.com.Util;

namespace tty.com
{
    /// <summary>
    /// 交互集.
    /// </summary>
    public class Com
    {
        public Com(Dispatcher dispatcher)
        {
            Dispatcher = dispatcher;
        }

        private string userpath => Cache + @"\User.json";
        private string msgpath => Cache + @"\Msg.json";
        private DispatcherTimer Timer { get; } = new DispatcherTimer()
        {
            Interval = TimeSpan.FromSeconds(10),
            IsEnabled = false
        };

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (User.Current != null && User.Current.userstate == UserState.Success)
            {
                UpdateUserInfoAsync();
                UpdateMsgAsync();
            }
            UpdateSharedInfoAsync();
            Console.WriteLine("---Com ---Timer ---Raised");
        }
        private void Restart()
        {
            Dispatcher.Invoke(() =>
            {
                int data = 0;
                string msg = "";

                //说明没有登录
                if (User.Current == null || User.Current.username == "" || User.Current.username == null)
                {
                    data = 0;
                    msg = "";
                }
                else if (User.Current.credit == null || User.Current.credit == "")
                {
                    data = 1;
                    msg = "";
                }
                else
                {
                    data = 2;
                    AutoLoginAsync();
                    msg = "";
                }

                Console.WriteLine($"---Com ---Initialize ---{msg}");
                InitializeCompleted?.Invoke(this, new MessageEventArgs(true, msg, data));
            });
        }
        private void AddSharedInfoAsync(UserInfo userInfo)
        {
            #region 提交信息
            bool flag = false;
            if (User.Users != null)
            {
                var selected = from item in User.Users
                               where item.username == userInfo.username
                               select item;
                if (selected.Count() > 0)
                {
                    var user = selected.First();
                    user.CopyFrom(userInfo);
                    user.IsLoaded = true;
                    flag = true;
                }
            }
            if (!flag)
            {
                List<UserInfo> users = User.Users == null ? new List<UserInfo>() : User.Users.ToList();
                userInfo.IsLoaded = true;
                users.Add(userInfo);

                User.Users = users.ToArray();
            }
            #endregion

        }
        private void pullmsg(MsgUni msg)
        {
            //var select = from item in User.Users where item.username == msg.username select item;
            //if (select != null && select.Count() > 0)
            //{
            //var user = select.First();

            //msg.nickname = user.nickname;
            //msg.portrait = user.Portrait;

            for (int i = 0; i < Msg.Msgs.Count; i++)
            {
                var cu = Msg.Msgs[i];
                if (cu.id == msg.id)
                {
                    Msg.Msgs[i] = msg;
                    return;
                }
                //倒序插入。
                else if (cu.id < msg.id)
                {
                    Msg.Msgs.Insert(i, msg);
                    return;
                }
            }
            Msg.Msgs.Add(msg);
            //}

            Console.WriteLine($"---Com ---pullmsg  ---wheremsgid:{msg.id}");
        }
        //private void pulluserInfo(UserInfo user)
        //{
        //    Dispatcher.Invoke(() => {
        //        var selected = from item in Msg.Msgs where item.username == user.username select item;

        //        foreach (var msg in selected)
        //        {
        //            msg.nickname = user.nickname;
        //            try
        //            {
        //                msg.portrait = ToolUtil.BytesToBitmapImage(ToolUtil.HexToBytes(user.portrait));
        //            }
        //            catch (Exception)
        //            {
        //            }
        //        }

        //        Console.WriteLine($"---Com ---pulluserInfo ---whereusername:{user.username}");
        //    });
        //}

        public string Cache { get; } = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\tty.board";
        public API API = new API();
        public Dispatcher Dispatcher { get; }
        public bool IsLoaded { get; private set; }
        public User User { get; private set; } = new User();
        public Msg Msg { get; private set; } = new Msg();

        public event EventHandler<MessageEventArgs> InitializeCompleted;
        public event EventHandler<MessageEventArgs> LoginCompleted;
        public event EventHandler<MessageEventArgs> RegisterCompleted;
        public event EventHandler<MessageEventArgs> ChangeNickNameCompleted;
        public event EventHandler<MessageEventArgs> AddMsgCompleted;

        /// <summary>
        /// 初始化
        /// </summary>
        public async void InitializeAsync()
        {
            await Task.Run(() =>
            {
                if (!IsLoaded)
                {
                    Timer.Tick += Timer_Tick;
                    Timer.IsEnabled = true;
                    //载入文件
                    if (File.Exists(userpath))
                    {
                        try
                        {
                            var str = File.ReadAllText(userpath);
                            Dispatcher.Invoke(() =>
                            {
                                User = JsonConvert.DeserializeObject<User>(str);
                            });
                        }
                        catch (Exception)
                        {
                        }

                        try
                        {
                            var str = File.ReadAllText(msgpath);
                            Dispatcher.Invoke(() =>
                            {
                                Msg = JsonConvert.DeserializeObject<Msg>(str);
                            });
                        }
                        catch (Exception)
                        {
                        }
                    }
                    int data = 0;
                    string msg = "";

                    //说明没有登录
                    if (User.Current == null || User.Current.username == "" || User.Current.username == null)
                    {
                        data = 0;
                        msg = "你还没有登录";
                    }
                    else if (User.Current.credit == null || User.Current.credit == "")
                    {
                        data = 1;
                        msg = "凭证已过期，请重新登录";
                    }
                    else
                    {
                        data = 2;
                        AutoLoginAsync();
                        msg = "";
                    }
                    IsLoaded = true;

                    Thread.Sleep(500);

                    Console.WriteLine($"---Com ---Initialize ---{msg}");

                    Dispatcher.Invoke(() =>
                    {
                        InitializeCompleted?.Invoke(this, new MessageEventArgs(true, msg, data));
                    });
                }
                else
                {
                    Restart();
                }
            });
        }
        public void Finish()
        {
            try
            {
                Directory.CreateDirectory(Cache);
            }
            catch (Exception)
            {
            }

            string data = JsonConvert.SerializeObject(User);
            File.WriteAllText(userpath, data);
            string data2 = JsonConvert.SerializeObject(Msg);
            File.WriteAllText(msgpath, data2);
        }
        public async void LoginAsync(string username, string password)
        {

            ResponceModel<UserInfo> result = null;
            // 请求数据。
            await Task.Run(() =>
            {
                try
                {
                    var postdata = $"method=login&username={username}&password={ToolUtil.MD5Encrypt32(password)}&devicetype=pc";
                    result = JsonConvert.DeserializeObject<ResponceModel<UserInfo>>(
                            HttpUtil.post(API[APIKey.User], postdata)
                            );


                }
                catch (Exception ex)
                {
                    Task.Delay(1000);
                    Dispatcher.Invoke(() =>
                    {
                        LoginCompleted?.Invoke(this, new MessageEventArgs(false, $"登录操作失败 {ex.Message}"));
                    });
                    Console.WriteLine($"---Com ---Login ---{ex.Message}");
                }
            });

            if (result != null)
            {
                if (result.code == 200)
                {
                    Dispatcher.Invoke(() =>
                    {
                        if (User.Current == null)
                        {
                            User.Current = new UserInfo();
                        }
                        User.Current.username = result.data.username;
                        User.Current.nickname = result.data.nickname;
                        User.Current.credit = result.data.credit;
                        User.Current.userstate = UserState.Success;

                        LoginCompleted?.Invoke(this, new MessageEventArgs(true, result.msg));
                    });

                    UpdateUserInfoAsync();
                }
                else
                {
                    await Task.Delay(1000);
                    Dispatcher.Invoke(() =>
                    {
                        LoginCompleted?.Invoke(this, new MessageEventArgs(false, result.msg));
                    });
                }

                Console.WriteLine($"---Com ---Login ---{result.msg}");

            }
        }
        public async void RegisterAsync(string nickname, string password)
        {
            ResponceModel<UserInfo> result = null;
            await Task.Run(() =>
            {
                try
                {
                    var postdata = $"method=register2&nickname={nickname}&password={ToolUtil.MD5Encrypt32(password)}";
                    result = JsonConvert.DeserializeObject<ResponceModel<UserInfo>>(
                            HttpUtil.post(API[APIKey.User], postdata)
                                );
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        RegisterCompleted?.Invoke(this, new MessageEventArgs(false, $"注册失败 {ex.Message}"));
                    });
                    Console.WriteLine($"---Com ---Register ---{ex.Message}");
                }
            });

            if (result != null)
            {
                Dispatcher.Invoke(() =>
                {
                    if (result.code == 200)
                    {
                        RegisterCompleted?.Invoke(this, new MessageEventArgs(true, result.msg, result.data.username));
                    }
                    else
                    {
                        RegisterCompleted?.Invoke(this, new MessageEventArgs(false, result.msg));
                    }
                });

                Console.WriteLine($"---Com ---Register ---{result.msg}");
            }
        }
        public async void AutoLoginAsync()
        {
            ResponceModel<UserInfo> result = null;

            if (User.Current.credit != null && User.Current.credit != "")
            {
                await Task.Run(() =>
                {
                    try
                    {
                        var postdata = $"method=autologin&credit={User.Current.credit}";
                        result = JsonConvert.DeserializeObject<ResponceModel<UserInfo>>(
                                HttpUtil.post(API[APIKey.User], postdata)
                                );
                    }
                    catch (Exception ex)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            LoginCompleted?.Invoke(this, new MessageEventArgs(false, $"自动登录失败 {ex.Message}"));
                        });
                    }
                });


                if (result != null)
                {
                    Dispatcher.Invoke(() =>
                    {
                        if (result.code == 200)
                        {
                            User.Current.username = result.data.username;
                            User.Current.nickname = result.data.nickname;
                            User.Current.credit = result.data.credit;
                            User.Current.userstate = UserState.Success;
                            LoginCompleted?.Invoke(this, new MessageEventArgs(true, result.msg));

                        }
                        else
                        {
                            User.Current.userstate = UserState.PasswordError;
                            LoginCompleted?.Invoke(this, new MessageEventArgs(false, result.msg));
                        }

                        Console.WriteLine($"---Com ---AutoLogin ---{result.msg}");
                    });
                }
            }
            else
            {
                Console.WriteLine($"---Com ---AutoLogin ---用户凭证不存在");
            }

        }
        public async void UpdateUserInfoAsync()
        {
            ResponceModel<UserInfo> result = null;

            await Task.Run(() =>
            {
                try
                {
                    if (User.Current!= null && User.Current.credit != null)
                    {
                        var postdata = $"type=base&credit={User.Current.credit}";
                        result = JsonConvert.DeserializeObject<ResponceModel<UserInfo>>(HttpUtil.post(API[APIKey.GetInfo], postdata));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"---Com ---UpdateUserInfo ---{ex.Message}");

                    return;
                }
            });
            if (result != null)
            {
                Dispatcher.Invoke(() =>
                {
                    if (result.code == 200)
                    {
                        // TODO 正在修改
                        User.Current.nickname = result.data.nickname;
                        User.Current.portrait = result.data.portrait;
                        User.Current.email = result.data.email;
                        User.Current.phone = result.data.phone;

                        AddSharedInfoAsync(User.Current);

                        User.Current.userstate = UserState.Success;
                    }
                    else
                    {
                        User.Current.userstate = UserState.PasswordError;
                    }
                });

                Console.WriteLine($"---Com ---UpdateUserInfo ---{result.msg}");
            }

        }
        public async void ChangeNickName(string nickname)
        {
            ResponceModel result = null;
            await Task.Run(() =>
            {
                try
                {
                    var postdata = $"method=changenickname&credit={User.Current.credit}&nickname={nickname}";
                    result = JsonConvert.DeserializeObject<ResponceModel>(
                        HttpUtil.post(API[APIKey.User], postdata)
                            );
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        ChangeNickNameCompleted?.Invoke(this, new MessageEventArgs(false, result.msg));
                    });
                    Console.WriteLine($"---Com ---ChangeNickName ---{ex.Message}");
                }
            });

            if (result != null)
            {
                Dispatcher.Invoke(() =>
                {
                    if (result.code == 200)
                    {
                        User.Current.userstate = UserState.Success;
                        User.Current.nickname = nickname;
                        ChangeNickNameCompleted?.Invoke(this, new MessageEventArgs(true, result.msg));
                    }
                    else
                    {
                        User.Current.userstate = UserState.PasswordError;
                        ChangeNickNameCompleted?.Invoke(this, new MessageEventArgs(false, result.msg));
                    }
                });
                Console.WriteLine($"---COM ---ChangeNickName ---{result.msg}");
            }
        }
        public async Task GetSharedUserInfoAsync(IEnumerable<string> users)
        {
            ResponceModel<UserInfo[]> result = null;

            await Task.Run(() =>
            {
                try
                {
                    var postdata = $"type=user&query={JsonConvert.SerializeObject(users.ToArray())}";
                    result = JsonConvert.DeserializeObject<ResponceModel<UserInfo[]>>(
                        HttpUtil.post(API[APIKey.Shared], postdata)
                        );
                }
                catch (Exception)
                {
                }
            });

            if (result != null)
            {
                Dispatcher.Invoke(() =>
                {
                    if (result.code == 200)
                    {
                        foreach (var re in result.data)
                        {
                            var seusers = from item in User.Users where item.username == re.username select item;
                            if (seusers.Count() > 0)
                            {
                                var user = seusers.First();
                                user.CopyFrom(re);
                            }
                        }
                    }
                });
            }

            Console.WriteLine($"---Com ---GetSharedInfo ---{result.msg}");
        }
        /// <summary>
        /// 更新共享用户信息。
        /// </summary>
        public async void UpdateSharedInfoAsync()
        {
            if (User.Users != null)
            {
                IEnumerable<string> needupdate = from item in User.Users where item.IsLoaded == false select item.username;
                IEnumerable<string> added = null;

                ResponceModel<User_MD5[]> result = null;
                await Task.Run(() =>
                {
                    try
                    {
                        var nk = from item in User.Users where item.IsLoaded == true select item;
                        var postdata = $"type=usermd5&query={JsonConvert.SerializeObject(nk.ToArray().Select((m) => m.username))}";
                        result = JsonConvert.DeserializeObject<ResponceModel<User_MD5[]>>(
                                HttpUtil.post(API[APIKey.Shared], postdata)
                            );

                        if (result.code == 200)
                        {
                            added = from item in nk where (item.md5 != result.data.TakeWhile((m) => m.username == item.username).First().username) select item.username;
                        }

                        Console.WriteLine($"---Com ---UpdateShareInfo ---{result.msg}");
                    }
                    catch (Exception)
                    {

                    }
                });


                try
                {
                    if (added != null)
                    {
                        needupdate = needupdate.Concat(added);
                    }

                    await GetSharedUserInfoAsync(needupdate);
                }
                catch (Exception)
                {
                }
            }
        }
        public async void AddMsgAsync(string content, BitmapImage pic = null)
        {
            ResponceModel<MsgUni> result = null;

            await Task.Run(() =>
            {
                try
                {
                    var postdata = $"method=add&credit={User.Current.credit}&content={content}";
                    result = JsonConvert.DeserializeObject<ResponceModel<MsgUni>>(HttpUtil.post(API[APIKey.MsgBoard], postdata));
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        AddMsgCompleted?.Invoke(this, new MessageEventArgs(false, $"添加留言失败{ex.Message}"));
                    });
                }
            });
            if (result != null)
            {
                if (result.code == 200)
                {
                    var selected = from item in User.Users where item.username == result.data.username select item.username;

                    if (selected.Count() == 0)
                    {
                        await GetSharedUserInfoAsync(new string[] { selected.First() });
                    }

                    Dispatcher.Invoke(() =>
                    {
                        pullmsg(result.data);
                        AddMsgCompleted?.Invoke(this, new MessageEventArgs(true, result.msg));
                    });

                }
                else
                {
                    Dispatcher.Invoke(() =>
                    {
                        AddMsgCompleted?.Invoke(this, new MessageEventArgs(false, result.msg));
                    });
                }
            }

        }
        public async void UpdateMsgAsync()
        {
            ResponceModel<Time_Msg> result = null;

            await Task.Run(() =>
            {
                try
                {
                    var postdata = $"method=update&credit={User.Current.credit}&time={Msg.UpdateTime.ToString()}";
                    result = JsonConvert.DeserializeObject<ResponceModel<Time_Msg>>(
                        HttpUtil.post(API[APIKey.MsgBoard], postdata)
                        );
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"---Com ---UpdateMsg ---{ex.Message}");
                }
            });

            if (result != null)
            {

                if (result.code == 200)
                {
                    Msg.UpdateTime = DateTime.Parse(result.data.time);


                    if (result.data.content.Length > 0)
                    {
                        var users = (from item in result.data.content select item.username).Distinct();
                        var except = users.Except(User.Users.Select((m) => m.username));
                        if (except.Count() > 0)
                        {
                            Dispatcher.Invoke(() =>
                            {
                                var list = User.Users.ToList();
                                foreach (var item in except)
                                {
                                    list.Add(new UserInfo() { username = item });
                                }
                                User.Users = list.ToArray();
                            });

                            await GetSharedUserInfoAsync(except);

                        }


                        foreach (var item in result.data.content)
                        {
                            Dispatcher.Invoke(() =>
                            {
                                pullmsg(item);
                            });
                        }
                    }
                }

                Console.WriteLine($"---Com ---UpdateMsg ---{result.msg}");
            }
        }
        public UserInfo GetUserInfo(string username)
        {
            if (User.Users != null)
            {
                var selected = from item in User.Users where item.username == username select item;
                if (selected.Count() > 0)
                {
                    return selected.First();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public void ExitLogin()
        {
            User.Current.credit = null;
        }
    }
}
