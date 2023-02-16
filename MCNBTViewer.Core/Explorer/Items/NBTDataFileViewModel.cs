using System.Collections.Generic;
using MCNBTViewer.Core.NBT;

namespace MCNBTViewer.Core.Explorer.Items {
    public class NBTDataFileViewModel : NBTCompoundViewModel {
        private string fileFilePath;
        public string FilePath {
            get => this.fileFilePath;
            set => this.RaisePropertyChanged(ref this.fileFilePath, value);
        }

        public NBTDataFileViewModel(string name) : base(name) {

        }

        public NBTDataFileViewModel(string name, NBTTagCompound nbt) : this(name) {
            foreach (KeyValuePair<string, NBTBase> pair in nbt.map) {
                this.Children.Add(CreateFrom(pair.Key, pair.Value));
            }
        }

        public NBTDataFileViewModel(BaseNBTCollectionViewModel nbt, bool deepCopy = false) : this(nbt.Name) {
            foreach (BaseNBTViewModel pair in nbt.Children) {
                this.Children.Add(deepCopy ? CreateFrom(pair.Name, pair.ToNBT()) : pair);
            }
        }
    }
}