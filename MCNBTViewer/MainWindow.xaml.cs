using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MCNBTViewer.Controls;
using MCNBTViewer.Core;
using MCNBTViewer.Core.Explorer;
using MCNBTViewer.Core.Explorer.Items;
using MCNBTViewer.Views;

namespace MCNBTViewer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WindowEx {
        public MainViewModel ViewModel => this.DataContext as MainViewModel;

        public MainWindow() {
            this.InitializeComponent();
            this.DataContext = new MainViewModel();
            this.ViewModel.Explorer.ExplorerListHandle = new MainListImpl(this.MainListBox);
            IoC.BroadcastShortcutActivity = (x) => {
                this.ShortcutIndicatorBlock.Text = x ?? "";
            };
        }

        public class MainListImpl : IMainList {
            public readonly ExtendedListBox listBox;

            public MainListImpl(ExtendedListBox listBox) {
                this.listBox = listBox;
            }

            public IEnumerable<BaseNBTViewModel> GetSelectedTags() {
                foreach (object obj in this.listBox.ItemContainerGenerator.Items) {
                    if (this.listBox.IsItemItsOwnContainer(obj)) {
                        yield return this.listBox.ItemContainerGenerator.ItemFromContainer((DependencyObject) obj) as BaseNBTViewModel;
                    }
                    else {
                        yield return obj as BaseNBTViewModel;
                    }
                }
            }
        }

        private async void OnTreeViewDrop(object sender, DragEventArgs e) {
            MainViewModel vm = this.ViewModel;
            if (vm == null) {
                return;
            }

            if (e.Data.GetData(DataFormats.FileDrop) is string[] files) {
                await vm.ParseFilesAction(files);
            }
            else {
                await IoC.MessageDialogs.ShowMessageAsync("Unknown drop", "Unknown dropped data. You can only drop files here!");
            }
        }

        private void ToggleButtonCheckChanged(object sender, RoutedEventArgs e) {
            if (sender is MenuItem button) {
                this.Topmost = button.IsChecked;
            }
        }
    }
}
