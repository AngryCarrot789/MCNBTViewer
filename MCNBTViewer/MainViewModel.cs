using System.Collections.Generic;
using System.IO;
using System.Threading;
using MCNBTViewer.Explorer;
using MCNBTViewer.NBT.Structure;
using REghZy.MVVM.ViewModels;

namespace MCNBTViewer {
    public class MainViewModel : BaseViewModel {
        public ExplorerViewModel Explorer { get; }

        public MainViewModel() {
            this.Explorer = new ExplorerViewModel();
            string debugPath = "C:\\Users\\kettl\\Desktop\\BergBergDk.dat";
            if (File.Exists(debugPath)) {
                this.Explorer.Root = CreateFolder(CompressedStreamTools.ReadCompressed(debugPath));
                this.Explorer.Root.Name = "Root Folder";
            }
            else {
                this.Explorer.Root = CreateRoot();
            }

            if (this.Explorer.Root.Children.Count > 0) {
                this.Explorer.SelectFileFromTree(this.Explorer.Root.Children[0]);
            }
        }

        public static FolderItemViewModel CreateFolder(NBTTagCompound nbt) {
            FolderItemViewModel folder = new FolderItemViewModel();
            foreach (KeyValuePair<string, NBTBase> pair in nbt.tagMap) {
                FileItemViewModel file = CreateFile(pair.Value);
                folder.Children.Add(file);
            }

            folder.Name = $"{nbt.tagName ?? "<unnamed>"}: {nbt.tagMap.Count} entries";
            return folder;
        }

        public static FolderItemViewModel CreateFolder(NBTTagList nbt) {
            FolderItemViewModel folder = new FolderItemViewModel();
            for (int index = 0; index < nbt.tagList.Count; index++) {
                NBTBase tag = nbt.tagList[index];
                FileItemViewModel file = CreateFile(tag);
                file.Name = tag.ToString();
                folder.Children.Add(file);
            }

            folder.Name = $"{nbt.tagName ?? "<unnamed>"}: {nbt.tagList.Count} entries";
            return folder;
        }

        public static FileItemViewModel CreateFile(NBTBase nbt) {
            if (nbt is NBTTagCompound) {
                return CreateFolder((NBTTagCompound) nbt);
            }
            else if (nbt is NBTTagList) {
                return CreateFolder((NBTTagList) nbt);
            }
            else {
                return new FileItemViewModel() {Name = $"{nbt.tagName}: {nbt}"};
            }
        }

        public static FolderItemViewModel CreateRoot() {
            FolderItemViewModel root = new FolderItemViewModel();

            FolderItemViewModel a = new FolderItemViewModel();
            a.Children.Add(new FileItemViewModel() { Name = "File 1 in A" });
            a.Children.Add(new FileItemViewModel() { Name = "File 2 in A" });
            FolderItemViewModel aIn = new FolderItemViewModel();
            aIn.Children.Add(new FileItemViewModel() { Name = "File 1 in Inner A" });
            aIn.Children.Add(new FileItemViewModel() { Name = "File 2 in Inner A" });
            a.Children.Add(aIn);

            root.Children.Add(a);

            FolderItemViewModel b = new FolderItemViewModel();
            b.Children.Add(new FileItemViewModel() { Name = "File 1 in B" });
            b.Children.Add(new FileItemViewModel() { Name = "File 2 in B" });
            b.Children.Add(new FileItemViewModel() { Name = "File 3 in B" });

            root.Children.Add(b);

            root.Children.Add(new FileItemViewModel() { Name = "File 1 in root" });

            return root;
        }
    }
}