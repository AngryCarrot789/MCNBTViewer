using MCNBTViewer.Explorer;
using REghZy.MVVM.ViewModels;

namespace MCNBTViewer {
    public class MainViewModel : BaseViewModel {
        public ExplorerViewModel Explorer { get; }

        public MainViewModel() {
            this.Explorer = new ExplorerViewModel();
            this.Explorer.Root = CreateRoot();
        }

        public static FolderItemViewModel CreateRoot() {
            FolderItemViewModel root = new FolderItemViewModel();

            FolderItemViewModel a = new FolderItemViewModel();
            a.Children.Add(new FileItemViewModel() {Name = "File 1 in A"});
            a.Children.Add(new FileItemViewModel() {Name = "File 2 in A"});
            FolderItemViewModel aIn = new FolderItemViewModel();
            aIn.Children.Add(new FileItemViewModel() { Name = "File 1 in Inner A" });
            aIn.Children.Add(new FileItemViewModel() { Name = "File 2 in Inner A" });
            a.Children.Add(aIn);

            root.Children.Add(a);

            FolderItemViewModel b = new FolderItemViewModel();
            b.Children.Add(new FileItemViewModel() {Name = "File 1 in B"});
            b.Children.Add(new FileItemViewModel() {Name = "File 2 in B"});
            b.Children.Add(new FileItemViewModel() {Name = "File 3 in B"});

            root.Children.Add(b);

            root.Children.Add(new FileItemViewModel() {Name = "File 1 in root"});

            return root;
        }
    }
}