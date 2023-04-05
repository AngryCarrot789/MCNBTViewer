using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using MCNBTViewer.Core.AdvancedContextService;
using MCNBTViewer.Core.AdvancedContextService.Base;
using MCNBTViewer.Core.NBT;

namespace MCNBTViewer.Core.Explorer.Items {
    public class NBTListViewModel : BaseNBTCollectionViewModel {
        private NBTType targetType;
        public NBTType TargetType {
            get => this.targetType;
            set => this.RaisePropertyChangedIfChanged(ref this.targetType, value);
        }

        public NBTListViewModel(string name = null) : base(name, NBTType.List) {

        }

        public override async Task PasteNBTBinaryDataAction(string name, NBTBase nbt) {
            if (this.TargetType != NBTType.End && nbt.Type != this.TargetType && this.Children.Count > 0) {
                await IoC.MessageDialogs.ShowMessageAsync("Invalid type", "This tag list expects items of type " + this.TargetType + ", not " + nbt.Type + ". Remove all exists items from the list and then paste it in, to switch the type");
                return;
            }

            this.AddChild(CreateFrom(name, nbt));
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

        public override IContextEntry GetNewItemContextEntry() {
            object typeParam = (int) this.TargetType;
            switch (this.TargetType) {
                case NBTType.Byte:      return new CommandContextEntry("Add new Byte...", this.CreateTagCommand, typeParam);
                case NBTType.Short:     return new CommandContextEntry("Add new Short...", this.CreateTagCommand, typeParam);
                case NBTType.Int:       return new CommandContextEntry("Add new Int...", this.CreateTagCommand, typeParam);
                case NBTType.Long:      return new CommandContextEntry("Add new Long...", this.CreateTagCommand, typeParam);
                case NBTType.Float:     return new CommandContextEntry("Add new Float...", this.CreateTagCommand, typeParam);
                case NBTType.Double:    return new CommandContextEntry("Add new Double...", this.CreateTagCommand, typeParam);
                case NBTType.String:    return new CommandContextEntry("Add new String...", this.CreateTagCommand, typeParam);
                case NBTType.ByteArray: return new CommandContextEntry("Add new Byte Array...", this.CreateTagCommand, typeParam);
                case NBTType.IntArray:  return new CommandContextEntry("Add new Int Array...", this.CreateTagCommand, typeParam);
                case NBTType.LongArray: return new CommandContextEntry("Add new Long Array...", this.CreateTagCommand, typeParam);
                case NBTType.List:      return new CommandContextEntry("Add new List...", this.CreateTagCommand, typeParam);
                case NBTType.Compound:  return new CommandContextEntry("Add new Compound...", this.CreateTagCommand, typeParam);
                default: return new ContextEntry(this, "New...", this.GetNewItemsEntries(new List<IContextEntry>()));
            }
        }
    }
}