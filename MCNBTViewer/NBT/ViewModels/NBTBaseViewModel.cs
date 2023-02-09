using System;
using MCNBTViewer.NBT.Structure;
using REghZy.MVVM.ViewModels;

namespace MCNBTViewer.NBT.ViewModels {
    public abstract class NBTBaseViewModel : BaseViewModel {
        private NBTType nbtType;
        public NBTType NBTType {
            get => this.nbtType;
            set => RaisePropertyChanged(ref this.nbtType, value);
        }

        private string name;
        public string Name {
            get => this.name;
            set => this.RaisePropertyChanged(ref this.name, value);
        }

        public NBTBaseViewModel() {

        }


        public abstract NBTBase WriteToNBT();

        public abstract void ReadFromNBT(NBTBase nbt);

        // Possible problems in the XAML designer by using abstract classes?
        // public abstract NBTBase WriteToNBT() {
        //     throw new NotImplementedException("Cannot call WriteToNBT on the base view model class");
        // }
        // public abstract void ReadFromNBT(NBTBase nbt) {
        //     throw new NotImplementedException("Cannot call WriteToNBT on the base view model class");
        // }
    }
}