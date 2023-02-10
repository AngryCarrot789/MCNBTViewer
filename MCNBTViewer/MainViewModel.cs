using System.Collections.Generic;
using System.IO;
using MCNBTViewer.NBT.Explorer;
using MCNBTViewer.NBT.Explorer.Items;
using MCNBTViewer.NBT.Structure;
using REghZy.MVVM.ViewModels;

namespace MCNBTViewer {
    public class MainViewModel : BaseViewModel {
        public NBTExplorerViewModel Explorer { get; }

        public MainViewModel() {
            this.Explorer = new NBTExplorerViewModel();
            string debugPath = "C:\\Users\\kettl\\Desktop\\BergBergDk.dat";
            if (File.Exists(debugPath)) {
                this.Explorer.Root = CreateTag(CompressedStreamTools.ReadCompressed(debugPath));
            }
            else {
                this.Explorer.Root = CreateRoot();
            }

            if (this.Explorer.Root.Children.Count > 0) {
                this.Explorer.OnTreeSelectItem(this.Explorer.Root.Children[0]);
            }
        }

        public static NBTCompoundViewModel CreateTag(NBTTagCompound nbt) {
            NBTCompoundViewModel tag = new NBTCompoundViewModel() {Name = nbt.name};
            foreach (KeyValuePair<string, NBTBase> pair in nbt.tagMap) {
                tag.Children.Add(CreateTag(pair.Value));
            }
            return tag;
        }

        public static NBTListViewModel CreateTag(NBTTagList nbt) {
            NBTListViewModel tag = new NBTListViewModel() {Name = nbt.name};
            foreach (NBTBase t in nbt.list) {
                tag.Children.Add(CreateTag(t));
            }
            return tag;
        }

        public static BaseNBTViewModel CreateTag(NBTBase nbt) {
            switch (nbt) {
                case NBTTagCompound compound: return CreateTag(compound);
                case NBTTagList list: return CreateTag(list);
                case NBTTagByteArray byteArray: return new NBTByteArrayViewModel() {Name = byteArray.name, Data = byteArray.data};
                case NBTTagIntArray array: return new NBTIntArrayViewModel() {Name = array.name, Data = array.data};
                default: return new NBTPrimitiveViewModel(nbt.Type) {Name = nbt.name};
            }
        }

        public static NBTCompoundViewModel CreateRoot() {
            NBTTagCompound root = new NBTTagCompound();
            NBTTagList list = new NBTTagList();
            list.list.Add(new NBTTagByte("My byte 1", 23));
            list.list.Add(new NBTTagFloat("My float 2", 0.357f));
            list.list.Add(new NBTTagString("My string 3", "23"));
            root.Put("listA", list);

            NBTTagCompound inner = new NBTTagCompound();
            NBTTagCompound inner2 = new NBTTagCompound();
            inner2.Put("lol", new NBTTagString("lololol"));
            inner2.Put("234y57894t", new NBTTagInt("24"));
            inner.Put("inner2", inner2);
            root.Put("inner", inner);
            return CreateTag(root);
        }
    }
}