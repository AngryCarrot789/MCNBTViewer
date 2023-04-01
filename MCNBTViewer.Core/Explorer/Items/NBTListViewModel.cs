using System.Collections.Specialized;
using System.Threading.Tasks;
using MCNBTViewer.Core.NBT;

namespace MCNBTViewer.Core.Explorer.Items {
    public class NBTListViewModel : BaseNBTCollectionViewModel {
        private NBTType targetType;
        public NBTType TargetType {
            get => this.targetType;
            set => RaisePropertyChangedIfChanged(ref this.targetType, value);
        }

        public NBTListViewModel(string name = null) : base(name, NBTType.List) {

        }

        public override async Task PasteNBTBinaryDataAction(string name, NBTBase nbt) {
            if (this.TargetType != NBTType.End && nbt.Type != this.TargetType && this.Children.Count > 0) {
                await IoC.MessageDialogs.ShowMessageAsync("Invalid type", "This tag list expects items of type " + this.NBTType + ", not " + nbt.Type + ". Remove all exists items from the list and then paste it in, to switch the type");
                return;
            }

            this.Children.Add(CreateFrom(name, nbt));
        }

        public override NBTBase ToNBT() {
            NBTTagList list = new NBTTagList();
            foreach (BaseNBTViewModel item in this.Children) {
                list.tags.Add(item.ToNBT());
            }
            return list;
        }

        protected override void OnChildrenChanged(object sender, NotifyCollectionChangedEventArgs e) {
            base.OnChildrenChanged(sender, e);
            if (this.Children.Count < 1) {
                this.TargetType = NBTType.End;
                return;
            }

            this.TargetType = this.Children[0].NBTType;
        }
    }
}