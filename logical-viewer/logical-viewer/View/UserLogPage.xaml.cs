using LogicalViewer.ViewModel;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.ApplicationModel.DataTransfer;

namespace LogicalViewer.View
{
    public sealed partial class UserLogPage : Page
    {
        public UserLogVM ViewModel { get; set; }

        public UserLogPage()
        {
            this.InitializeComponent();
        }

        private void CopyableTextBlock_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var elt = sender as TextBlock;
            if (elt is null)
            {
                return;
            }

            var flyout = elt.ContextFlyout as MenuFlyout;
            if (flyout is null)
            {
                flyout = new();
                elt.ContextFlyout = flyout;
            }

            var copyCommand = new StandardUICommand(StandardUICommandKind.Copy);
            copyCommand.ExecuteRequested += (_, _) =>
            {
                var dataPackage = new DataPackage() { RequestedOperation = DataPackageOperation.Copy };
                dataPackage.SetText(elt.Text);
                Clipboard.SetContent(dataPackage);
            };

            var copyFlyout = new MenuFlyoutItem() { Command = copyCommand };
            flyout.Items.Add(copyFlyout);
        }
    }
}
