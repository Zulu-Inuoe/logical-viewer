using LogicalParse.DataModel;
using LogicalViewer.View;
using LogicalViewer.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.IO;
using Windows.Storage.Pickers;

namespace LogicalViewer
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private async void MenuBarOpen_Click(object sender, RoutedEventArgs e)
        {
            // Create a file picker
            var openPicker = new FileOpenPicker();

            // Retrieve the window handle (HWND) of the current WinUI 3 window.
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);

            // Initialize the file picker with the window handle (HWND).
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            // Set options for your file picker
            openPicker.ViewMode = PickerViewMode.List;
            openPicker.FileTypeFilter.Add(".log");
            openPicker.FileTypeFilter.Add(".zip");

            // Open the picker for the user to pick a file
            var file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                // todo parse
                var userLog = default(UserLog);
                try
                {
                    using (var stream = await file.OpenReadAsync())
                    using (var reader = new StreamReader(stream.AsStreamForRead()))
                    {
                        userLog = new LogicalParse.Parser().Parse(reader);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                    return;
                }
                
                var vm = new UserLogVM()
                {
                    Name = file.Name,
                    UserLog = userLog
                };


                var newItem = new TabViewItem
                {
                    Header = file.DisplayName,
                    IconSource = new SymbolIconSource() { Symbol = Symbol.Document },
                    Content = new Frame()
                    {
                        Content = new UserLogPage() { ViewModel = vm }
                    }
                };

                LogsTabView.TabItems.Add(newItem);
                if (LogsTabView.TabItems.Count == 1)
                {
                    LogsTabView.SelectedIndex = 0;
                }
            }
        }

        private void TabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            sender.TabItems.Remove(args.Tab);
        }

        private void CloseSelectedTabKeyboardAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            var InvokedTabView = (args.Element as TabView);

            // Only close the selected tab if it is closeable
            if (((TabViewItem)InvokedTabView.SelectedItem).IsClosable)
            {
                InvokedTabView.TabItems.Remove(InvokedTabView.SelectedItem);
            }

            args.Handled = true;
        }

        private void NavigateToNumberedTabKeyboardAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            var InvokedTabView = (args.Element as TabView);

            var tabToSelect =
                sender.Key switch
                {
                    Windows.System.VirtualKey.Number1 => 0,
                    Windows.System.VirtualKey.Number2 => 1,
                    Windows.System.VirtualKey.Number3 => 2,
                    Windows.System.VirtualKey.Number4 => 3,
                    Windows.System.VirtualKey.Number5 => 4,
                    Windows.System.VirtualKey.Number6 => 5,
                    Windows.System.VirtualKey.Number7 => 6,
                    Windows.System.VirtualKey.Number8 => 7,
                    Windows.System.VirtualKey.Number9 => 8,
                    Windows.System.VirtualKey.Number0 => InvokedTabView.TabItems.Count - 1,
                    _ => default(int?)
                };

            if (tabToSelect is int tabNum && tabNum < InvokedTabView.TabItems.Count)
            {
                InvokedTabView.SelectedIndex = tabNum;
            }

            args.Handled = true;
        }
    }
}
