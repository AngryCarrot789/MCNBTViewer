using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MCNBTViewer.Core.Actions;
using MCNBTViewer.Core.AdvancedContextService;
using MCNBTViewer.Core.AdvancedContextService.Base;
using MCNBTViewer.Core.NBT;
using MCNBTViewer.Core.Utils;
using MCNBTViewer.Core.Views.Dialogs.UserInputs;
using Action = MCNBTViewer.Core.Actions.Action;

namespace MCNBTViewer.Core.Explorer.Items {
    public abstract class BaseNBTViewModel : BaseViewModel, IContextProvider {
        public NBTType NBTType { get; }

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

        public ICommand CopyNameCommand { get; }

        public ICommand CopyBinaryToClipboardCommand { get; }

        public ICommand EditNameCommand { get; }

        protected BaseNBTViewModel(string name, NBTType type) {
            this.Name = name;
            this.NBTType = type;
            this.RemoveFromParentCommand = new RelayCommand(this.RemoveFromParentAction, () => this.Parent != null && !(this is NBTDataFileViewModel));
            this.CopyNameCommand = new RelayCommand(async () => {
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

            this.EditNameCommand = new RelayCommand(this.RenameAction, () => this.Parent is NBTCompoundViewModel);
        }

        public void RenameAction() {
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
            tag.AddChildren(nbt);
            return tag;
        }

        public static NBTListViewModel CreateFrom(string name, NBTTagList nbt) {
            NBTListViewModel tag = new NBTListViewModel(name);
            tag.AddChildren(nbt);
            return tag;
        }

        public static BaseNBTViewModel CreateFrom(string name, NBTBase nbt) {
            switch (nbt) {
                case NBTTagCompound compound: return CreateFrom(name, compound);
                case NBTTagList list: return CreateFrom(name, list);
                case NBTTagLongArray la: return new NBTLongArrayViewModel(name) {Data = la.data};
                case NBTTagIntArray ia: return new NBTIntArrayViewModel(name) {Data = ia.data};
                case NBTTagByteArray ba: return new NBTByteArrayViewModel(name) {Data = ba.data};
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

        public virtual List<IContextEntry> GetContext(List<IContextEntry> list) {
            list.Add(new CommandContextEntry("Edit Name", this.EditNameCommand));
            list.Add(new CommandContextEntry("Copy Name", this.CopyNameCommand));

            if (this is NBTPrimitiveViewModel a) {
                list.Add(new CommandContextEntry("Edit Value", a.EditGeneralCommand));
                list.Add(new CommandContextEntry("Copy Value", a.CopyValueCommand));
            }
            else if (this is BaseNBTArrayViewModel) {
                list.Add(new ActionContextEntry(this, "Copy array (CSV)", "nbt.value.copy.array.csv"));
            }

            list.Add(new CommandContextEntry("Copy to clipboard (binary)", this.CopyBinaryToClipboardCommand));
            if (this is BaseNBTCollectionViewModel c) {
                list.Add(new CommandContextEntry("Paste from clipboard (binary)", c.PasteNBTBinaryDataCommand));
            }

            list.Add(new CommandContextEntry("Remove tag", this.RemoveFromParentCommand));
            return list;
        }

        static BaseNBTViewModel() {
            ActionUtils.Register<CopyNBTNameAction>(ActionType.NBTCopyName);
            ActionUtils.Register<CopyNBTValueAction>(ActionType.NBTCopyValue);
            ActionUtils.Register<EditNBTNameAction>(ActionType.NBTEditGeneral);
        }

        private class EditNBTNameAction : Action {
            public EditNBTNameAction() : base("Edit Name", "Renames this tag's name") {

            }

            public override async Task<bool> Execute(ActionEvent e) {
                if (e.TryGetContext(out NBTPrimitiveViewModel primitive)) {
                    await primitive.EditAction();
                }
                else if (e.TryGetContext(out BaseNBTArrayViewModel array)) {
                    await array.EditAction();
                }
                else if (e.TryGetContext(out BaseNBTViewModel nbt)) {
                    string newName = IoC.UserInput.ShowSingleInputDialog("Rename tag", "Input a new name for this element", nbt.Name ?? "", InputValidator.FromFunc(input => {
                        if (nbt.Parent is NBTCompoundViewModel compound) {
                            BaseNBTViewModel first = compound.Children.FirstOrDefault(item => item.Name == input);
                            if (first != null) {
                                return "A tag already exists with that name: " + first;
                            }
                        }

                        return null;
                    }));

                    if (newName != null) {
                        nbt.Name = newName;
                    }
                }
                else {
                    return false;
                }

                return true;
            }
        }

        private class CopyNBTNameAction : Action {
            public CopyNBTNameAction() : base("Copy Name", "Copies this tag's name, if it has one, to the clipboard") {

            }

            public override async Task<bool> Execute(ActionEvent e) {
                if (e.TryGetContext(out BaseNBTViewModel nbt) && !string.IsNullOrEmpty(nbt.Name)) {
                    await ClipboardUtils.SetClipboardOrShowErrorDialog(nbt.Name);
                    return true;
                }

                return false;
            }
        }

        private class CopyNBTValueAction : Action {
            public CopyNBTValueAction() : base("Copy value", "Copies this tag's value, if it has one, to the clipboard") {

            }

            public override async Task<bool> Execute(ActionEvent e) {
                if (e.TryGetContext(out NBTPrimitiveViewModel primitive)) {
                    if (!string.IsNullOrEmpty(primitive.Data)) {
                        await ClipboardUtils.SetClipboardOrShowErrorDialog(primitive.Data);
                    }
                }
                else if (e.TryGetContext(out NBTLongArrayViewModel lvm)) {
                    if (lvm.Data != null) {
                        await ClipboardUtils.SetClipboardOrShowErrorDialog(string.Join(",", lvm.Data));
                    }
                }
                else if (e.TryGetContext(out NBTIntArrayViewModel ivm)) {
                    if (ivm.Data != null) {
                        await ClipboardUtils.SetClipboardOrShowErrorDialog(string.Join(",", ivm.Data));
                    }
                }
                else if (e.TryGetContext(out NBTByteArrayViewModel bvm)) {
                    if (bvm.Data != null) {
                        await ClipboardUtils.SetClipboardOrShowErrorDialog(string.Join(",", bvm.Data));
                    }
                }
                else {
                    return false;
                }

                return true;
            }
        }
    }
}