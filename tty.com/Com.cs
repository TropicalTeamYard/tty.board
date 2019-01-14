using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        private DispatcherTimer Timer { get; } = new DispatcherTimer()
        {
            Interval = TimeSpan.FromMinutes(10),
            IsEnabled = false
        };

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (User.Current.userstate == UserState.Success)
            {
                UpdateUserInfoAsync();
            }
            Console.WriteLine("---Com ---Timer ---Raised");
        }

        public string Cache { get; } = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\tty.board";
        public API API = new API();
        public Dispatcher Dispatcher { get; }
        public bool IsLoaded { get; private set; }
        public User User { get; private set; } = new User();

        public event EventHandler<MessageEventArgs> InitializeCompleted;
        public event EventHandler<MessageEventArgs> LoginCompleted;

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
        private void Restart()
        {
            Dispatcher.Invoke(() => {
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
        /// <summary>
        /// 启用自动定时刷新状态
        /// </summary>
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
        }

        public async void LoginAsync(string username, string password)
        {
            try
            {
                ResponceModel<UserInfo> result = null;
                // 请求数据。
                await Task.Run(() =>
                {
                    var postdata = $"method=login&username={username}&password={ToolUtil.MD5Encrypt32(password)}&devicetype=pc";
                    result = JsonConvert.DeserializeObject<ResponceModel<UserInfo>>(
                        HttpUtil.post(API[APIKey.User], postdata)
                        );
                });


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
            catch (Exception ex)
            {
                await Task.Delay(1000);
                Dispatcher.Invoke(() =>
                {
                    LoginCompleted?.Invoke(this, new MessageEventArgs(false, $"登录操作失败 {ex.Message}"));
                });
                Console.WriteLine($"---Com ---Login ---{ex.Message}");
            }
        }
        public async void AutoLoginAsync()
        {
            try
            {
                ResponceModel<UserInfo> result = null;

                if (User.Current.credit != null && User.Current.credit != "")
                {
                    await Task.Run(() =>
                    {
                        var postdata = $"method=autologin&credit={User.Current.credit}";
                        result = JsonConvert.DeserializeObject<ResponceModel<UserInfo>>(
                            HttpUtil.post(API[APIKey.User], postdata)
                            );
                    });


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
                else
                {
                    Console.WriteLine($"---Com ---AutoLogin ---用户凭证不存在");
                }
            }
            catch (Exception ex)
            {
                LoginCompleted?.Invoke(this, new MessageEventArgs(false, $"自动登录失败 {ex.Message}"));
            }
        }
        public async void UpdateUserInfoAsync()
        {
            try
            {
                ResponceModel<UserInfo> result = null;

                await Task.Run(() =>
                {
                    var postdata = $"type=base&credit={User.Current.credit}";
                    result = JsonConvert.DeserializeObject<ResponceModel<UserInfo>>(HttpUtil.post(API[APIKey.GetInfo], postdata));
                });


                Dispatcher.Invoke(() =>
                {
                    if (result.code == 200)
                    {

                        // TODO 正在修改
                        User.Current.nickname = result.data.nickname;
                        User.Current.portrait = result.data.portrait;
                        User.Current.email = result.data.email;
                        User.Current.phone = result.data.phone;

                        User.Current.userstate = UserState.Success;
                    }
                    else
                    {
                        User.Current.userstate = UserState.PasswordError;
                    }
                });

                Console.WriteLine($"---Com ---UpdateUserInfo ---{result.msg}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"---Com ---UpdateUserInfo ---{ex.Message}");
            }
        }
        public void ExitLogin()
        {
            User.Current.credit = null;
            //Finish();
        }
    }
}
