using System.Windows;
using MCNBTViewer.Core;

namespace MCNBTViewer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainViewModel ViewModel => this.DataContext as MainViewModel;

        public MainWindow() {
            this.InitializeComponent();
            this.DataContext = new MainViewModel();
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
    }
}
