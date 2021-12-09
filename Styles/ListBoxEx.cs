using Clipboards.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Clipboards.Styles
{
    public class ListBoxEx : ListBox
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ListBoxItemEx();
        }
    }

    public class ListBoxItemEx : System.Windows.Controls.ListBoxItem
    {
        protected override void OnSelected(System.Windows.RoutedEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is ListBoxItem))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            ListBoxItem item = (ListBoxItem)dep;

            if (item.IsSelected)
            {
                item.IsSelected = !item.IsSelected;
                //e.Handled = true;
            }
            base.OnSelected(e);
        }

        private TextBlock textbox;
        public Button button;
        private Image image;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            textbox = Template.FindName("tb", this) as TextBlock;
            textbox.MouseLeftButtonDown += Textbox_MouseLeftButtonDown;
            button = Template.FindName("closeBtn", this) as Button;
            button.Click += Button_Click;
            image = Template.FindName("img", this) as Image;
            image.MouseLeftButtonDown += Textbox_MouseLeftButtonDown;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var hashCode = textbox.Text.GetHashCode();
            var item = MainVM.Instance.ClipboardsItems.FirstOrDefault(c => c.HashCode == hashCode);
            MainVM.Instance.ClipboardsItems.Remove(item);
        }

        private void Textbox_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                HotKeyHelper.IsIgnorance = true;
                if (!string.IsNullOrEmpty(textbox.Text))
                {
                    Clipboard.SetText(textbox.Text);
                }
                else
                {
                    Clipboard.SetImage((BitmapSource)image.Source);
                }
            }
            catch(Exception ex)
            {
                LogHelper.Instance._logger.Error(ex);
            }
        }


    }
}
