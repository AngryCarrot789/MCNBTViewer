using System.Threading.Tasks;
using System.Windows.Input;
using MCNBTViewer.Core.NBT;

namespace MCNBTViewer.Core.Explorer.Items {
    public abstract class BaseNBTArrayViewModel : BaseNBTViewModel {
        public ICommand EditCommand { get; }

        protected BaseNBTArrayViewModel(string name, NBTType type) : base(name, type) {
            this.EditCommand = new AsyncRelayCommand(this.EditAction);
        }

        public async Task EditAction() {
            bool isInCompound = this.Parent is NBTCompoundViewModel;

            string newName;
            NBTBase nbt;
            switch (this.NBTType) {
                case NBTType.ByteArray: { (string, NBTTagByteArray)? x = IoC.TagDialogService.CreateTagByteArray(isInCompound, this.Name, (NBTTagByteArray) this.ToNBT()); if (!x.HasValue) return; newName = x.Value.Item1; nbt = x.Value.Item2; } break;
                case NBTType.IntArray:  { (string, NBTTagIntArray)? x = IoC.TagDialogService.CreateTagIntArray(isInCompound, this.Name, (NBTTagIntArray) this.ToNBT());    if (!x.HasValue) return; newName = x.Value.Item1; nbt = x.Value.Item2; } break;
                case NBTType.LongArray: { (string, NBTTagLongArray)? x = IoC.TagDialogService.CreateTagLongArray(isInCompound, this.Name, (NBTTagLongArray) this.ToNBT()); if (!x.HasValue) return; newName = x.Value.Item1; nbt = x.Value.Item2; } break;
                default: return;
            }

            if (this.Parent is NBTCompoundViewModel parent) {
                BaseNBTViewModel existing = parent.FindChildByName(newName);
                if (existing != null && existing != this) {
                    await IoC.MessageDialogs.ShowMessageAsync("Already exists", $"A tag with the name '{newName}' already exists as {existing.NBTType}");
                    return;
                }

                this.Name = newName;
            }

            this.SetData(nbt);
        }

        protected abstract void SetData(NBTBase nbt);
    }
}