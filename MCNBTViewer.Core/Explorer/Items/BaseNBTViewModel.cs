using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using FramePFX.Core;
using MCNBTViewer.Core.AdvancedContextService;
using MCNBTViewer.Core.NBT;
using MCNBTViewer.Core.Utils;
using REghZy.Streams;

namespace MCNBTViewer.Core.Explorer.Items {
    public abstract class BaseNBTViewModel : BaseViewModel, IContextProvider {
        private NBTType nbtType;
        public NBTType NBTType {
            get => this.nbtType;
            set => this.RaisePropertyChanged(ref this.nbtType, value);
        }

        protected string name;
        public string Name {
            get => this.name;
            set => this.RaisePropertyChanged(ref this.name, value);
        }

        private BaseNBTCollectionViewModel parent;
        public BaseNBTCollectionViewModel Parent {
            get => this.parent;
            set => this.RaisePropertyChanged(ref this.parent, value);
        }

        /// <summary>
        /// Calculates the root parent of this NBT
        /// </summary>
        public BaseNBTCollectionViewModel RootParent {
            get {
                BaseNBTCollectionViewModel topLevel = this.Parent;
                for (BaseNBTCollectionViewModel next = topLevel; next != null; next = next.Parent)
                    topLevel = next;
                return topLevel;
            }
        }

        public List<BaseNBTViewModel> ParentChain {
            get {
                BaseNBTViewModel item = this;
                List<BaseNBTViewModel> list = new List<BaseNBTViewModel>();
                do {
                    list.Add(item);
                } while ((item = item.Parent) != null);
                list.Reverse();
                return list;
            }
        }

        public RelayCommand RemoveFromParentCommand { get; }

        public ICommand CopyKeyNameCommand { get; }

        public ICommand CopyBinaryToClipboardCommand { get; }

        protected BaseNBTViewModel(string name, NBTType type) {
            this.Name = name;
            this.NBTType = type;
            this.RemoveFromParentCommand = new RelayCommand(this.RemoveFromParentAction, () => this.Parent != null && !(this is NBTDataFileViewModel));
            this.CopyKeyNameCommand = new RelayCommand(async () => {
                await ClipboardUtils.SetClipboardOrShowErrorDialog(this.Name ?? "");
            }, () => !string.IsNullOrEmpty(this.Name));
            this.CopyBinaryToClipboardCommand = new RelayCommand(async () => {
                if (IoC.Clipboard == null) {
                    await IoC.MessageDialogs.ShowMessageAsync("No clipboard", "Clipboard is unavailable. Cannot copy the NBT to the clipboard");
                }
                else {
                    using (MemoryStream stream = new MemoryStream()) {
                        try {
                            NBTBase nbt = this.ToNBT();
                            NBTBase.WriteTag(new DataOutputStream(stream), this.Name, nbt);
                            IoC.Clipboard.SetBinaryTag("TAG_NBT", stream.ToArray());
                        }
                        catch (Exception e) {
                            await IoC.MessageDialogs.ShowMessageAsync("Failed to write NBT", "Failed to write NBT to clipboard: " + e.Message);
                        }
                    }
                }
            });
        }

        protected BaseNBTViewModel(NBTType type) : this(null, type) {

        }

        protected BaseNBTViewModel() : this(null, NBTType.End) {

        }

        public static NBTCompoundViewModel CreateFrom(string name, NBTTagCompound nbt) {
            NBTCompoundViewModel tag = new NBTCompoundViewModel(name);
            foreach (KeyValuePair<string, NBTBase> pair in nbt.map) {
                tag.Children.Add(CreateFrom(pair.Key, pair.Value));
            }

            return tag;
        }

        public static NBTListViewModel CreateFrom(string name, NBTTagList nbt) {
            NBTListViewModel tag = new NBTListViewModel(name);
            foreach (NBTBase t in nbt.tags) {
                tag.Children.Add(CreateFrom(null, t));
            }

            return tag;
        }

        public static BaseNBTViewModel CreateFrom(string name, NBTBase nbt) {
            switch (nbt) {
                case NBTTagCompound compound: return CreateFrom(name, compound);
                case NBTTagList list: return CreateFrom(name, list);
                case NBTTagByteArray ba: return new NBTByteArrayViewModel(name) {Data = ba.data};
                case NBTTagIntArray ia: return new NBTIntArrayViewModel(name) {Data = ia.data};
                case NBTTagByte b: return new NBTPrimitiveViewModel(name, NBTType.Byte) {Data = b.data.ToString()};
                case NBTTagShort s: return new NBTPrimitiveViewModel(name, NBTType.Short) {Data = s.data.ToString()};
                case NBTTagInt i: return new NBTPrimitiveViewModel(name, NBTType.Int) {Data = i.data.ToString()};
                case NBTTagLong l: return new NBTPrimitiveViewModel(name, NBTType.Long) {Data = l.data.ToString()};
                case NBTTagFloat f: return new NBTPrimitiveViewModel(name, NBTType.Float) {Data = f.data.ToString()};
                case NBTTagDouble d: return new NBTPrimitiveViewModel(name, NBTType.Double) {Data = d.data.ToString()};
                case NBTTagString str: return new NBTPrimitiveViewModel(name, NBTType.String) {Data = str.data};
                case NBTTagEnd _: return new NBTPrimitiveViewModel(name, NBTType.End);
                default: return new NBTPrimitiveViewModel(name, nbt.Type);
            }
        }

        public abstract NBTBase ToNBT();

        protected virtual bool CanExpandTreeItem() {
            return false;
        }

        protected virtual bool CanChangeName(string oldName, string newName) {
            return true;
        }

        public virtual void OnRemovedFromFolder() {

        }

        public virtual void OnAddedToFolder() {

        }

        private void RemoveFromParentAction() {
            if (this.Parent != null) {
                this.Parent.RemoveChild(this);
            }
        }

        public virtual IEnumerable<IBaseContextEntry> GetContextEntries() {
            yield return new ContextEntry("Copy Key Name", this.CopyKeyNameCommand);
            yield return new ContextEntry("Copy", this.CopyBinaryToClipboardCommand);
            yield return ContextEntrySeparator.Instance;
            yield return new ContextEntry("Delete Tag", this.RemoveFromParentCommand) {
                ToolTip = this.Parent != null ? "Removes this NBT entry from its parent" : "This Tag has no parent?"
            };
        }
    }
}