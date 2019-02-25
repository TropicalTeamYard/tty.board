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
using tty.Windows;

namespace tty.Pages
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : Page
    {
        public readonly NavigationHelper RightHelper;


        public MainPage()
        {
            InitializeComponent();
            App.Core.Window.SetTitleBar(GridTitle);


        }
        private void RichTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            
        }
        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && tbx1.Text!="")
            {
                AnalyseSentence(tbx1.Text);
                
                tbx1.Text = "";
            }
        }
        private void AppendLine(object arg, Brush foreground)
        {
            rtx1.Document.Blocks.Add(new Paragraph(new Run(arg.ToString()) { Foreground = foreground}));
        }

        private void AnalyseSentence(string text)
        {
            //清空缓存区
            if (text == "#cls" || text == "#clear")
            {
                rtx1.Document.Blocks.Clear();
            }
            else
            {
                AppendLine($"> {text}", Brushes.White);
                if (text.StartsWith("#"))
                {
                    if (text == "#time")
                    {
                        AppendLine(DateTime.Now, Brushes.Green);
                    }
                    else if (text == "#help")
                    {
                        AppendLine("#cls/#clear\t\t\t\t\t\t清除缓存区.", Brushes.SkyBlue);
                        AppendLine("#time\t\t\t\t\t\t\t显示系统时间.", Brushes.Green);
                    }
                    else
                    {
                        AppendLine("未知的命令,你可以输入'#help'查看支持的命令", Brushes.OrangeRed);
                    }
                }
                else
                {
                    AppendLine(text, Brushes.DarkGray);
                }
            }
        }
    }

   


}
