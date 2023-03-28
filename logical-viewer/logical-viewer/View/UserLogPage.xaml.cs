using LogicalViewer.ViewModel;
using Microsoft.UI.Xaml.Controls;

namespace LogicalViewer.View
{
    public sealed partial class UserLogPage : Page
    {
        public UserLogVM ViewModel { get; set; }

        public UserLogPage()
        {
            this.InitializeComponent();
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            DetailFrame.Content = e.ClickedItem;
        }
    }
}
