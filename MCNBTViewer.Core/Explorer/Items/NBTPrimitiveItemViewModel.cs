using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MCNBTViewer.Core.NBT;
using MCNBTViewer.Core.Utils;

namespace MCNBTViewer.Core.Explorer.Items {
    /// <summary>
    /// A view model for tags end, byte, short, int, long, float, double and string
    /// </summary>
    public class NBTPrimitiveViewModel : BaseNBTViewModel {
        private string data;
        public string Data {
            get => this.data;
            set => this.RaisePropertyChanged(ref this.data, value);
        }

        public ICommand CopyValueCommand { get; }

        public ICommand EditValueCommand { get; }

        public NBTPrimitiveViewModel(string name, NBTType type) : base(name, type) {
            this.CopyValueCommand = new RelayCommand(async () => await this.CopyValueAction());
            this.EditValueCommand = new AsyncRelayCommand(this.EditValueAction, () => this.Parent != null);
        }

        private async Task EditValueAction() {
            if (this.Parent == null) {
                return;
            }

            bool isInCompound = this.Parent is NBTCompoundViewModel;

            string name;
            NBTBase nbt;
            switch (this.NBTType) {
                case NBTType.Byte:  { (string, NBTTagByte)? x = IoC.TagDialogService.CreateTagByte(isInCompound, this.Name, (NBTTagByte) this.ToNBT());       if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; } break;
                case NBTType.Short:  { (string, NBTTagShort)? x = IoC.TagDialogService.CreateTagShort(isInCompound, this.Name, (NBTTagShort) this.ToNBT());    if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; } break;
                case NBTType.Int:  { (string, NBTTagInt)? x = IoC.TagDialogService.CreateTagInt(isInCompound, this.Name, (NBTTagInt) this.ToNBT());          if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; } break;
                case NBTType.Long:  { (string, NBTTagLong)? x = IoC.TagDialogService.CreateTagLong(isInCompound, this.Name, (NBTTagLong) this.ToNBT());       if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; } break;
                case NBTType.Float:  { (string, NBTTagFloat)? x = IoC.TagDialogService.CreateTagFloat(isInCompound, this.Name, (NBTTagFloat) this.ToNBT());    if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; } break;
                case NBTType.Double:  { (string, NBTTagDouble)? x = IoC.TagDialogService.CreateTagDouble(isInCompound, this.Name, (NBTTagDouble) this.ToNBT()); if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; } break;
                case NBTType.String:  { (string, NBTTagString)? x = IoC.TagDialogService.CreateTagString(isInCompound, this.Name, (NBTTagString) this.ToNBT()); if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; } break;
                default: return;
            }

            if (this.Parent is NBTCompoundViewModel parent) {
                BaseNBTViewModel existing = parent.FindChildByName(name);
                if (existing != null && existing != this) {
                    await IoC.MessageDialogs.ShowMessageAsync("Already exists", $"A tag with the name '{name}' already exists: {existing.NBTType}");
                    return;
                }

                this.Name = name;
            }

            this.Data = nbt.ToString();
        }

        private async Task CopyValueAction() {
            if (this.Data != null) {
                await ClipboardUtils.SetClipboardOrShowErrorDialog(this.Data);
            }
        }

        public override NBTBase ToNBT() {
            switch (this.NBTType) {
                case NBTType.End: return new NBTTagEnd();
                case NBTType.Byte: return new NBTTagByte(byte.Parse(this.data));
                case NBTType.Short: return new NBTTagShort(short.Parse(this.data));
                case NBTType.Int: return new NBTTagInt(int.Parse(this.data));
                case NBTType.Long: return new NBTTagLong(long.Parse(this.data));
                case NBTType.Float: return new NBTTagFloat(float.Parse(this.data));
                case NBTType.Double: return new NBTTagDouble(double.Parse(this.data));
                case NBTType.String: return new NBTTagString(this.data);
            }

            throw new Exception($"Unsupported. This = {this.GetType()}, NBT Type = {this.NBTType}");
        }
    }
}