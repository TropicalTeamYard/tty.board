using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using tty.Model;

namespace tty.Windows
{
    public class NavigationHelper
    {
        public NavigationHelper(params KeyValuePair<CheckControl, FrameworkElement>[] pair)
        {
            items = new Dictionary<CheckControl, FrameworkElement>();
            foreach (var item in pair)
            {
                items.Add(item.Key,item.Value);
                item.Key.Click += Key_Click;
            }
            
        }

        private void Key_Click(object sender, RoutedEventArgs e)
        {
            CheckControl c = (CheckControl)sender;
            c.IsChecked = !c.IsChecked;
            foreach (var item in items)
            {
                if (item.Key == c)
                {
                    if (c.IsChecked)
                    {
                        item.Value.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        item.Value.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    item.Key.IsChecked = false;
                    item.Value.Visibility = Visibility.Collapsed;
                }
            }
        }
        private Dictionary<CheckControl, FrameworkElement> items;
    }
}
