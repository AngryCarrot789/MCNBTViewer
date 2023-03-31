using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using MCNBTViewer.Core.AdvancedContextService;
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

        public NBTPrimitiveViewModel(string name, NBTType type) : base(name, type) {
            this.CopyValueCommand = new RelayCommand(async () => await this.CopyValueAction());
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