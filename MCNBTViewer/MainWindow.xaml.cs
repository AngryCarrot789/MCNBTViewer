using System.Windows;

namespace MCNBTViewer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            this.InitializeComponent();
            this.DataContext = new MainViewModel();
        }
    }
}
