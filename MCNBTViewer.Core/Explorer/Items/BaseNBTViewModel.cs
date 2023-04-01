using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using MCNBTViewer.Core.AdvancedContextService;
using MCNBTViewer.Core.NBT;
using MCNBTViewer.Core.Utils;
using MCNBTViewer.Core.Views.Dialogs.UserInputs;

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

        public IEnumerable<(BaseNBTViewModel, string)> ParentAndNameChain {
            get {
                foreach (BaseNBTViewModel item in this.ParentChain) {
                    if (item.Parent is NBTListViewModel tagList) {
                        yield return (item, new StringBuilder().Append('[').Append(tagList.Children.IndexOf(item)).Append(']').ToString());
                    }
                    else if (string.IsNullOrEmpty(item.Name)) {
                        yield return (item, "<unnamed>");
                    }
                    else {
                        yield return (item, item.Name);
                    }
                }
            }
        }

        public List<string> PathChain => this.ParentAndNameChain.Select(x => x.Item2).ToList();

        public string Path => string.Join("/", this.PathChain);

        public RelayCommand RemoveFromParentCommand { get; }

        public ICommand CopyKeyNameCommand { get; }

        public ICommand CopyBinaryToClipboardCommand { get; }

        public ICommand RenameCommand { get; }

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
                            NBTBase.WriteTag(CompressedStreamTools.CreateOutput(stream), this.Name, nbt);
                            IoC.Clipboard.SetBinaryTag("TAG_NBT", stream.ToArray());
                        }
                        catch (Exception e) {
                            await IoC.MessageDialogs.ShowMessageAsync("Failed to write NBT", "Failed to write NBT to clipboard: " + e.Message);
                        }
                    }
                }
            });

            this.RenameCommand = new RelayCommand(this.RenameAction, () => this.Parent is NBTCompoundViewModel);
        }

        private void RenameAction() {
            if (!(this.Parent is NBTCompoundViewModel compound)) {
                return;
            }

            string newName = IoC.UserInput.ShowSingleInputDialog("Rename tag", "Input a new name for this element", this.Name ?? "", InputValidator.FromFunc(input => {
                BaseNBTViewModel first = compound.Children.FirstOrDefault(item => item.Name == input);
                if (first != null) {
                    return "A tag already exists with that name: " + first;
                }

                return null;
            }));

            if (newName != null) {
                this.Name = newName;
            }
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

        public override string ToString() {
            return this.NBTType.ToString();
        }

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
            yield return new ContextEntry("Copy Name", this.CopyKeyNameCommand);
            if (this is NBTPrimitiveViewModel item) {
                yield return new ContextEntry("Copy Value", item.CopyValueCommand);
            }
            else if (this is NBTIntArrayViewModel intArray) {
                yield return new LazyASFContextEntry("Copy Int Values (CSV)", async () => {
                    await ClipboardUtils.SetClipboardOrShowErrorDialog(intArray.Data == null ? "" : string.Join(",", intArray.Data));
                });
            }
            else if (this is NBTByteArrayViewModel byteArray) {
                yield return new LazyASFContextEntry("Copy Byte Values (CSV)", async () => {
                    await ClipboardUtils.SetClipboardOrShowErrorDialog(byteArray.Data == null ? "" : string.Join(",", byteArray.Data));
                });
            }

            yield return new ContextEntry("Copy (Binary)", this.CopyBinaryToClipboardCommand);
            yield return ContextEntrySeparator.Instance;
            yield return new ContextEntry("Delete Tag", this.RemoveFromParentCommand) {
                ToolTip = "Removes this NBT entry from its parent"
            };
        }
    }
}