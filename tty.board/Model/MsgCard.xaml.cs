using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using tty.com.Model;

namespace tty.Model
{
    /// <summary>
    /// MsgCard.xaml 的交互逻辑
    /// </summary>
    public partial class MsgCard : UserControl
    {
        public MsgCard()
        {
            InitializeComponent();

            try
            {
                ibtnDelete.SetBinding(IsEnabledProperty, new Binding() { Source = App.Core.Com, Path = new PropertyPath("IsMsgDeleteEnabled") });
            }
            catch (Exception)
            {
            }
        }
        public string Time
        {
            get { return (string)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public object Comments
        {
            get { return (object)GetValue(CommentsProperty); }
            set { SetValue(CommentsProperty, value); }
        }
        public int ID
        {
            get { return (int)GetValue(IDProperty); }
            set { SetValue(IDProperty, value); }
        }
        public bool IsShown
        {
            get { return (bool)GetValue(IsShownProperty); }
            set { SetValue(IsShownProperty, value); }
        }

        private void OnUserInfoChanged()
        {
            if (!string.IsNullOrEmpty(UserName))
            {
                UserInfo userInfo = App.Core.Com.GetUserInfo(UserName);

                if (userInfo!=null)
                {
                    tbkNickName.SetBinding(TextBlock.TextProperty, new Binding() { Source = userInfo, Path = new PropertyPath("nickname") });
                    itbPortrait.SetBinding(ImageTriggerButtonRound.ImageSourceProperty, new Binding() { Source = userInfo, Path = new PropertyPath("Portrait") });
                    tbkUserName.Text = userInfo.username;

                }
            }
        }
        private void OnCommentsChanged()
        {
            var comments = (MsgComment[])Comments;
            if (comments == null || comments.Length == 0)
            {
                tbkShortInfo.Text = "当前还没有评论.";
                tbkComment.Visibility = Visibility.Collapsed;
            }
            else
            {
                UserInfo userInfo = App.Core.Com.GetUserInfo(UserName);
                tbkShortInfo.Text = $"供{comments.Length}条评论";

                StringBuilder stringBuilder = new StringBuilder();
                foreach (var item in comments)
                {
                    stringBuilder.Append($"{(userInfo==null?"": userInfo.nickname ?? "null")} : {(item.content ?? "null")}   -{item.time ?? "null"}");
                    stringBuilder.AppendLine();
                }

                tbkComment.Text = stringBuilder.ToString();
                tbkComment.Visibility = Visibility.Visible;
            }
        }
        private void OnIsShownChanged()
        {
            if (IsShown)
            {
                Visibility = Visibility.Visible;
            }
            else
            {
                Visibility = Visibility.Collapsed;
            }
        }

        private void MsgCard_MouseEnter(object sender, MouseEventArgs e)
        {
            ibtnDelete.Visibility = Visibility.Visible;
        }
        private void MsgCard_MouseLeave(object sender, MouseEventArgs e)
        {
            ibtnDelete.Visibility = Visibility.Collapsed;
        }
        private void IbtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ID != -1)
            {
                Deleted?.Invoke(this, new EventArgs());
            }
        }

        public string UserName  
        {
            get { return (string)GetValue(UserNameProperty); }
            set { SetValue(UserNameProperty, value); }
        }
        public event EventHandler Deleted;
        public static readonly DependencyProperty UserNameProperty =
            DependencyProperty.Register("UserName", typeof(string), typeof(MsgCard), new PropertyMetadata(null,(d,e)=> 
            {
                ((MsgCard)d).OnUserInfoChanged();
            }));
        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(string), typeof(MsgCard), new PropertyMetadata("1970/1/1 08:00:00"));
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(MsgCard), new PropertyMetadata(""));
        public static readonly DependencyProperty CommentsProperty =
            DependencyProperty.Register("Comments", typeof(object), typeof(MsgCard), new PropertyMetadata(new MsgComment[0],(d,e)=> 
            {
                ((MsgCard)d).OnCommentsChanged();
            }));
        public static readonly DependencyProperty IDProperty =
            DependencyProperty.Register("ID", typeof(int), typeof(MsgCard), new PropertyMetadata(-1));
        public static readonly DependencyProperty IsShownProperty =
            DependencyProperty.Register("IsShown", typeof(bool), typeof(MsgCard), new PropertyMetadata(true,(d,e)=> 
            {
                ((MsgCard)d).OnIsShownChanged();
            }));
    }
}
