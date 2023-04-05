using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MCNBTViewer.Core.AdvancedContextService;
using MCNBTViewer.Core.AdvancedContextService.Base;
using MCNBTViewer.Core.NBT;
using MCNBTViewer.Core.Utils;

namespace MCNBTViewer.Core.Explorer.Items {
    public abstract class BaseNBTCollectionViewModel : BaseNBTViewModel {
        public static bool IgnoreMainExplorerSelectionChange {
            get => IoC.MainExplorer.IgnoreSelectionChanged;
            set => IoC.MainExplorer.IgnoreSelectionChanged = value;
        }

        protected readonly EfficientObservableCollection<BaseNBTViewModel> children;
        public ReadOnlyObservableCollection<BaseNBTViewModel> Children { get; }

        public bool IsEmpty => this.Children.Count < 1;

        public RelayCommand SortByTypeCommand { get; }
        public RelayCommand SortByNameCommand { get; }
        public RelayCommand SortByBothCommand { get; }

        public AsyncRelayCommand PasteNBTBinaryDataCommand { get; }

        public RelayCommand<int> CreateTagCommand { get; }

        protected BaseNBTCollectionViewModel(string name, NBTType type) : base(name, type) {
            this.children = new EfficientObservableCollection<BaseNBTViewModel>();
            this.Children = new ReadOnlyObservableCollection<BaseNBTViewModel>(this.children);
            this.children.CollectionChanged += this.OnChildrenChanged;
            this.CreateTagCommand = new RelayCommand<int>(async (x) => await this.NewTagAction(x));
            this.SortByTypeCommand = new RelayCommand(() => {
                List<BaseNBTViewModel> list = this.Children.OrderByDescending((a) => a.NBTType).ToList();
                this.ApplySort(list);
            });

            this.SortByNameCommand = new RelayCommand(() => {
                List<BaseNBTViewModel> list = this.Children.OrderBy((a) => a.Name ?? "").ToList();
                this.ApplySort(list);
            });

            this.SortByBothCommand = new RelayCommand(() => {
                List<BaseNBTViewModel> list = new List<BaseNBTViewModel>(this.Children);
                list.Sort((a, b) => {
                    int cmp = a.NBTType.Compare4(b.NBTType);
                    return cmp != 0 ? cmp : string.Compare(a.Name ?? "", b.Name ?? "", StringComparison.CurrentCulture);
                });

                this.ApplySort(list);
            });

            this.PasteNBTBinaryDataCommand = new AsyncRelayCommand(async () => {
                if (IoC.Clipboard == null) {
                    await IoC.MessageDialogs.ShowMessageAsync("Clipboard unavailable", "Clipboard is unavailable. Cannot paste");
                    return;
                }

                byte[] bytes = IoC.Clipboard.GetBinaryTag("TAG_NBT");
                if (bytes == null || bytes.Length < 1) {
                    await IoC.MessageDialogs.ShowMessageAsync("Invalid clipboard", "Clipboard did not contain a copied tag");
                    return;
                }

                using (MemoryStream stream = new MemoryStream(bytes)) {
                    bool result;
                    string tagName;
                    NBTBase nbt;
                    try {
                        result = NBTBase.ReadTag(CompressedStreamTools.CreateInput(stream), 0, out tagName, out nbt);
                    }
                    catch (Exception e) {
                        await IoC.MessageDialogs.ShowMessageAsync("Invalid clipboard", "Failed to read NBT from clipboard: " + e.Message);
                        return;
                    }

                    if (!result) {
                        await IoC.MessageDialogs.ShowMessageAsync("Invalid clipboard", "Clipboard did not contain a valid tag? That's weird...");
                        return;
                    }

                    await this.PasteNBTBinaryDataAction(tagName, nbt);
                }
            });
        }

        public void AddChild(BaseNBTViewModel nbt) {
            this.children.Add(nbt);
        }

        public void AddChildren(IEnumerable<BaseNBTViewModel> nbt) {
            this.children.AddRange(nbt);
        }

        public void AddChildren(NBTTagCompound nbt) {
            this.children.AddRange(nbt.map.Select(x => CreateFrom(x.Key, x.Value)));
        }

        public void AddChildren(NBTTagList nbt) {
            this.children.AddRange(nbt.tags.Select(x => CreateFrom(null, x)));
        }

        protected virtual async Task NewTagAction(int type) {
            bool isInCompound = this is NBTCompoundViewModel;
            string name;
            NBTBase nbt;
            switch ((NBTType) type) {
                case NBTType.Byte      : { (string, NBTTagByte)? x = IoC.TagDialogService.CreateTagByte(isInCompound);            if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; } break;
                case NBTType.Short     : { (string, NBTTagShort)? x = IoC.TagDialogService.CreateTagShort(isInCompound);          if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; } break;
                case NBTType.Int       : { (string, NBTTagInt)? x = IoC.TagDialogService.CreateTagInt(isInCompound);              if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; } break;
                case NBTType.Long      : { (string, NBTTagLong)? x = IoC.TagDialogService.CreateTagLong(isInCompound);            if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; } break;
                case NBTType.Float     : { (string, NBTTagFloat)? x = IoC.TagDialogService.CreateTagFloat(isInCompound);          if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; } break;
                case NBTType.Double    : { (string, NBTTagDouble)? x = IoC.TagDialogService.CreateTagDouble(isInCompound);        if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; } break;
                case NBTType.String    : { (string, NBTTagString)? x = IoC.TagDialogService.CreateTagString(isInCompound);        if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; } break;
                case NBTType.ByteArray : { (string, NBTTagByteArray)? x = IoC.TagDialogService.CreateTagByteArray(isInCompound);  if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; } break;
                case NBTType.IntArray  : { (string, NBTTagIntArray)? x = IoC.TagDialogService.CreateTagIntArray(isInCompound);    if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; } break;
                case NBTType.LongArray : { (string, NBTTagLongArray)? x = IoC.TagDialogService.CreateTagLongArray(isInCompound);  if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; } break;
                case NBTType.Compound  : {
                    if (isInCompound) {
                        (string, NBTTagList)? x = IoC.TagDialogService.CreateTagList(true);
                        if (!x.HasValue)
                            return;
                        name = x.Value.Item1;
                        nbt = x.Value.Item2;
                    }
                    else {
                        name = null;
                        nbt = new NBTTagList();
                    }

                    break;
                }
                case NBTType.List: {
                    if (isInCompound) {
                        (string, NBTTagCompound)? x = IoC.TagDialogService.CreateTagCompound(true);
                        if (!x.HasValue)
                            return;
                        name = x.Value.Item1;
                        nbt = x.Value.Item2;
                    }
                    else {
                        name = null;
                        nbt = new NBTTagCompound();
                    }
                } break;
                default: return;
            }

            if (isInCompound) {
                if (string.IsNullOrEmpty(name)) {
                    await IoC.MessageDialogs.ShowMessageAsync("Tag name is empty", "The tag name cannot be an empty string");
                    return;
                }
                else if (this.Children.Any(x => x.Name == name)) {
                    await IoC.MessageDialogs.ShowMessageAsync("Tag already exists", "A tag already exists with the name " + name + ": " + this.Children.FirstOrDefault(x => x.Name == name));
                    return;
                }
            }

            this.children.Add(CreateFrom(name, nbt));
        }

        public abstract Task PasteNBTBinaryDataAction(string name, NBTBase nbt);

        protected void ApplySort(List<BaseNBTViewModel> sorted) {
            IgnoreMainExplorerSelectionChange = true;
            this.children.ClearAndAddRange(sorted);
            IgnoreMainExplorerSelectionChange = false;
            // for (int index = 0; index < sorted.Count; index++) {
            //     this.children.Move(this.children.IndexOf(sorted[index]), index);
            // }
        }

        protected virtual void OnChildrenChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.OldItems != null) {
                foreach (BaseNBTViewModel item in e.OldItems) {
                    item.OnRemovedFromFolder();
                    item.Parent = null;
                }
            }

            if (e.NewItems != null) {
                foreach (BaseNBTViewModel item in e.NewItems) {
                    item.Parent = this;
                    item.OnAddedToFolder();
                }
            }

            this.RaisePropertyChanged(nameof(this.IsEmpty));
        }

        public bool RemoveChild(BaseNBTViewModel nbt) {
            return this.children.Remove(nbt);
        }

        public override List<IContextEntry> GetContext(List<IContextEntry> list) {
            list.Add(GetNewItemContextEntry());
            list.Add(ContextEntrySeparator.Instance);
            list.Add(new CommandContextEntry("Sort By Type", this.SortByTypeCommand));
            list.Add(new CommandContextEntry("Sort By Name", this.SortByNameCommand));
            list.Add(new CommandContextEntry("Sort By Both", this.SortByBothCommand) {
                ToolTip = "This is what NBTExplorer sorts by; compound, list, array, primitive and then finally by name"
            });

            list.Add(ContextEntrySeparator.Instance);
            list.Add(new CommandContextEntry("Edit Name", this.EditNameCommand));
            list.Add(new CommandContextEntry("Copy Name", this.CopyNameCommand));
            list.Add(new CommandContextEntry("Copy to clipboard (binary)", this.CopyBinaryToClipboardCommand));
            list.Add(new CommandContextEntry("Paste from clipboard (binary)", this.PasteNBTBinaryDataCommand));
            list.Add(new CommandContextEntry("Remove tag", this.RemoveFromParentCommand));
            return list;
        }

        public virtual IContextEntry GetNewItemContextEntry() {
            return new ContextEntry(this, "New...", this.GetNewItemsEntries(new List<IContextEntry>()));
        }

        public List<IContextEntry> GetNewItemsEntries(List<IContextEntry> list) {
            list.Add(new CommandContextEntry("Byte", this.CreateTagCommand, (int) NBTType.Byte));
            list.Add(new CommandContextEntry("Short", this.CreateTagCommand, (int) NBTType.Short));
            list.Add(new CommandContextEntry("Int", this.CreateTagCommand, (int) NBTType.Int));
            list.Add(new CommandContextEntry("Long", this.CreateTagCommand, (int) NBTType.Long));
            list.Add(new CommandContextEntry("Float", this.CreateTagCommand, (int) NBTType.Float));
            list.Add(new CommandContextEntry("Double", this.CreateTagCommand, (int) NBTType.Double));
            list.Add(new CommandContextEntry("String", this.CreateTagCommand, (int) NBTType.String));
            list.Add(new CommandContextEntry("Byte Array",this.CreateTagCommand, (int) NBTType.ByteArray));
            list.Add(new CommandContextEntry("Int Array",this.CreateTagCommand, (int) NBTType.IntArray));
            list.Add(new CommandContextEntry("Long Array",this.CreateTagCommand, (int) NBTType.LongArray));
            list.Add(new CommandContextEntry("List", this.CreateTagCommand, (int) NBTType.List));
            list.Add(new CommandContextEntry("Compound", this.CreateTagCommand, (int) NBTType.Compound));
            return list;
        }

        public BaseNBTViewModel FindChildByName(string name) {
            return this.Children.FirstOrDefault(x => x.Name == name);
        }
    }
}