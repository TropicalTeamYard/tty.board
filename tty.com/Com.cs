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

        public string Cache { get; } = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\tty.board";
        private string userpath => Cache + @"\User.json";

        public API API = new API();
        public Dispatcher Dispatcher { get; }
        public bool IsLoaded { get; private set; }
        public User User { get; private set; } = new Data.User();

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
                    //载入文件
                    if (File.Exists(userpath))
                    {
                        try
                        {
                            User = JsonConvert.DeserializeObject<Data.User>(File.ReadAllText(userpath));
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
                        msg = "登录成功";
                    }
                    IsLoaded = true;

                    Thread.Sleep(500);

                    Dispatcher.Invoke(() =>
                    {
                        InitializeCompleted?.Invoke(this, new MessageEventArgs(true, msg, data));
                    });
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

                }
                else
                {
                    await Task.Delay(1000);
                    Dispatcher.Invoke(() =>
                    {

                        LoginCompleted?.Invoke(this, new MessageEventArgs(false, result.msg));
                    });
                }

            }
            catch (Exception ex)
            {
                await Task.Delay(1000);
                Dispatcher.Invoke(() =>
                {
                    LoginCompleted?.Invoke(this, new MessageEventArgs(false, $"登录操作失败 {ex.Message}"));
                });
            }
        }
    }
}
