using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace tty.Model
{
    public class CheckControl : UControl
    {
        public CheckControl()
        {
            Click += CheckControl_Click;
        }

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }
        public bool CanAutoCheck
        {
            get { return (bool)GetValue(CanAutoCheckProperty); }
            set { SetValue(CanAutoCheckProperty, value); }
        }

        protected virtual void OnChecked() { }

        public static readonly DependencyProperty IsCheckedProperty =
        DependencyProperty.Register("IsChecked", typeof(bool), typeof(CheckControl), new PropertyMetadata(false, new PropertyChangedCallback((d, e) =>
        {
            dynamic b = d;
            b.OnChecked();
        })));

        public static readonly DependencyProperty CanAutoCheckProperty =
            DependencyProperty.Register("CanAutoCheck", typeof(bool), typeof(CheckControl), new PropertyMetadata(true));

        private void CheckControl_Click(object sender, RoutedEventArgs e)
        {
            if (CanAutoCheck)
            {
                IsChecked = !IsChecked;
            }
            e.Handled = false;
        }
    }

}
