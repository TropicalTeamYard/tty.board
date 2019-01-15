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
        }

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
            btnSend.IsEnabled = false;
            btnSend.Text = "正在发送";

            App.Core.Com.AddMsgAsync(tbxSend.Text);
        }
        #endregion

    }
}
