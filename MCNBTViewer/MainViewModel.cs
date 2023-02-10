using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using MCNBTViewer.NBT.Explorer;
using MCNBTViewer.NBT.Explorer.Items;
using MCNBTViewer.NBT.Structure;
using REghZy.MVVM.ViewModels;

namespace MCNBTViewer {
    public class MainViewModel : BaseViewModel {
        public NBTExplorerViewModel Explorer { get; }

        public ICommand OpenFileCommand { get; }

        public MainViewModel() {
            this.Explorer = new NBTExplorerViewModel();
            const string debugPath = "C:\\Users\\kettl\\Desktop\\BergBergDk.dat";
            if (File.Exists(debugPath)) {
                NBTTagCompound compound = CompressedStreamTools.ReadCompressed(debugPath, out string name);
                this.Explorer.LoadedDataFiles.Add(new NBTDataFileViewModel(name, compound));
            }
            else {
                this.Explorer.LoadedDataFiles.Add(new NBTDataFileViewModel(CreateRoot()));
            }
        }

        public static NBTCompoundViewModel CreateRoot() {
            NBTTagCompound root = new NBTTagCompound();
            NBTTagList list = new NBTTagList();
            list.tags.Add(new NBTTagByte(23));
            list.tags.Add(new NBTTagFloat(0.357f));
            list.tags.Add(new NBTTagString("23"));
            root.Put("listA", list);

            NBTTagCompound inner = new NBTTagCompound();
            NBTTagCompound inner2 = new NBTTagCompound();
            inner2.Put("lol", new NBTTagString("lololol"));
            inner2.Put("234y57894t", new NBTTagInt());
            inner.Put("inner2", inner2);
            root.Put("inner", inner);
            return BaseNBTViewModel.CreateFrom(null, root);
        }
    }
}