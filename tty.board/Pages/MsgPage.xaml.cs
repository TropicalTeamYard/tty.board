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

namespace tty.Pages
{
    /// <summary>
    /// MsgPage.xaml 的交互逻辑
    /// </summary>
    public partial class MsgPage : Page
    {
        public MsgPage()
        {
            InitializeComponent();

            listMsg.ItemsSource = App.Core.Com.Msg.Msgs;
            App.Core.Com.AddMsgCompleted += Com_AddMsgCompleted;
            App.Core.Com.RemoveMsgCompleted += Com_RemoveMsgCompleted;
        }
        private int IDToDelete = -1;

        #region 发送留言
        private void Com_AddMsgCompleted(object sender, com.MessageEventArgs e)
        {
            if (e.Status == true)
            {
                tbxSend.Text = "";
            }
            btnSend.Text = "添加留言";
            btnSend.IsEnabled = true;

            App.Core.Window.SendMessage(e.Msg);
        }
        private void TbxSend_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbxSend.Text == "")
            {
                btnSend.IsEnabled = false;
            }
            else
            {
                btnSend.IsEnabled = true;
            }
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            if (App.Core.Com.User.Current.userstate == com.Model.UserState.Success)
            {
                btnSend.IsEnabled = false;
                btnSend.Text = "正在发送";

                App.Core.Com.AddMsgAsync(tbxSend.Text);
            }
        }
        #endregion
        #region 删除留言
        private void Com_RemoveMsgCompleted(object sender, com.MessageEventArgs e)
        {
            App.Core.Window.SendMessage(e.Msg);
        }
        public void DeleteMsgOpen(int id)
        {
            IDToDelete = id;
            gridDeleteFlyout.Visibility = Visibility.Visible;
        }
        private void MsgCard_Deleted(object sender, EventArgs e)
        {
            if (App.Core.Com.User.Current.userstate == UserState.Success)
            {
                Model.MsgCard msgCard = (Model.MsgCard)sender;
                DeleteMsgOpen(msgCard.ID);
            }
        }
        private void IBarDeleteMsgCancel_Click(object sender, RoutedEventArgs e)
        {
            gridDeleteFlyout.Visibility = Visibility.Collapsed;
        }
        private void IBarDeleteMsgOk_Click(object sender, RoutedEventArgs e)
        {
            gridDeleteFlyout.Visibility = Visibility.Collapsed;
            App.Core.Com.RemoveMsgAsync(IDToDelete);
        }
        #endregion

    }
}
