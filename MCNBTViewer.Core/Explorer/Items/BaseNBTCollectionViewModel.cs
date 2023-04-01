using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MCNBTViewer.Core.AdvancedContextService;
using MCNBTViewer.Core.NBT;

namespace MCNBTViewer.Core.Explorer.Items {
    public abstract class BaseNBTCollectionViewModel : BaseNBTViewModel {
        public static bool IgnoreMainExplorerSelectionChange {
            get => IoC.MainExplorer.IgnoreSelectionChanged;
            set => IoC.MainExplorer.IgnoreSelectionChanged = value;
        }

        public ObservableCollection<BaseNBTViewModel> Children { get; }

        private bool isEmpty;
        public bool IsEmpty {
            get => this.isEmpty;
            set => this.RaisePropertyChanged(ref this.isEmpty, value);
        }

        public RelayCommand SortByTypeCommand { get; }
        public RelayCommand SortByNameCommand { get; }
        public RelayCommand SortByBothCommand { get; }

        public AsyncRelayCommand PasteNBTBinaryDataCommand { get; }

        public RelayCommand<int> CreateTagCommand { get; }

        protected BaseNBTCollectionViewModel(string name, NBTType type) : base(name, type) {
            this.Children = new ObservableCollection<BaseNBTViewModel>();
            this.Children.CollectionChanged += this.OnChildrenChanged;
            this.CreateTagCommand = new RelayCommand<int>(async (x) => await this.NewTagAction(x));
            this.SortByTypeCommand = new RelayCommand(() => {
                IgnoreMainExplorerSelectionChange = true;
                List<BaseNBTViewModel> list = this.Children.OrderByDescending((a) => a.NBTType).ToList();
                this.ApplySort(list);
                IgnoreMainExplorerSelectionChange = false;
            });

            this.SortByNameCommand = new RelayCommand(() => {
                IgnoreMainExplorerSelectionChange = true;
                List<BaseNBTViewModel> list = this.Children.OrderBy((a) => a.Name ?? "").ToList();
                this.ApplySort(list);
                IgnoreMainExplorerSelectionChange = false;
            });

            this.SortByBothCommand = new RelayCommand(() => {
                IgnoreMainExplorerSelectionChange = true;
                List<BaseNBTViewModel> list = new List<BaseNBTViewModel>(this.Children);
                list.Sort((a, b) => {
                    int cmp = a.NBTType.Compare4(b.NBTType);
                    if (cmp != 0) {
                        return cmp; // reverse order; compound on top, list below, then rest
                    }

                    return string.Compare(a.Name ?? "", b.Name ?? "", StringComparison.CurrentCulture);
                });

                this.ApplySort(list);
                IgnoreMainExplorerSelectionChange = false;
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

                string tagName;
                NBTBase nbt;
                using (MemoryStream stream = new MemoryStream(bytes)) {
                    bool result;
                    try {
                        result = NBTBase.ReadTag(CompressedStreamTools.CreateInput(stream), 0, out tagName, out nbt);
                    }
                    catch (Exception e) {
                        await IoC.MessageDialogs.ShowMessageAsync("Invalid clipboard", "Failed to read NBT from clipboard");
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

        protected virtual Task<bool> CanAddTagOfType(int type) {
            return Task.FromResult(true);
        }

        protected virtual async Task NewTagAction(int type) {
            // await IoC.MessageDialogs.ShowMessageAsync("Cannot create child", "Children cannot be added to this tag");

            bool isCompound = this is NBTCompoundViewModel;
            string name;
            NBTBase nbt;
            switch ((NBTType) type) {
                case NBTType.Byte      :  { (string, NBTTagByte)? x = IoC.TagDialogService.CreateTagByte(isCompound);            if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; } break;
                case NBTType.Short     :  { (string, NBTTagShort)? x = IoC.TagDialogService.CreateTagShort(isCompound);          if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; } break;
                case NBTType.Int    :  { (string, NBTTagInt)? x = IoC.TagDialogService.CreateTagInt(isCompound);              if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; } break;
                case NBTType.Long      :  { (string, NBTTagLong)? x = IoC.TagDialogService.CreateTagLong(isCompound);            if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; } break;
                case NBTType.Float     :  { (string, NBTTagFloat)? x = IoC.TagDialogService.CreateTagFloat(isCompound);          if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; } break;
                case NBTType.Double    :  { (string, NBTTagDouble)? x = IoC.TagDialogService.CreateTagDouble(isCompound);        if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; } break;
                case NBTType.String   :  { (string, NBTTagString)? x = IoC.TagDialogService.CreateTagString(isCompound);        if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; } break;
                case NBTType.ByteArray   :  { (string, NBTTagByteArray)? x = IoC.TagDialogService.CreateTagByteArray(isCompound);  if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; } break;
                case NBTType.IntArray: { (string, NBTTagIntArray)? x = IoC.TagDialogService.CreateTagIntArray(isCompound);    if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; } break;
                case NBTType.Compound : {
                    if (isCompound) {
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
                    if (isCompound) {
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

            if (isCompound) {
                if (string.IsNullOrEmpty(name)) {
                    await IoC.MessageDialogs.ShowMessageAsync("Tag name is empty", "The tag name cannot be an empty string");
                    return;
                }
                else if (this.Children.Any(x => x.Name == name)) {
                    await IoC.MessageDialogs.ShowMessageAsync("Tag already exists", "A tag already exists with the name " + name + ": " + this.Children.FirstOrDefault(x => x.Name == name));
                    return;
                }
            }

            this.Children.Add(CreateFrom(name, nbt));
        }

        public abstract Task PasteNBTBinaryDataAction(string name, NBTBase nbt);

        protected void ApplySort(List<BaseNBTViewModel> sorted) {
            for (int index = 0; index < sorted.Count; index++) {
                this.Children.Move(this.Children.IndexOf(sorted[index]), index);
            }
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

            this.IsEmpty = this.Children.Count == 0;
        }

        public bool RemoveChild(BaseNBTViewModel nbt) {
            bool result = this.Children.Remove(nbt);
            nbt.Parent = null;
            return result;
        }

        public override IEnumerable<IBaseContextEntry> GetContextEntries() {
            yield return new ContextEntry("New", null, this.GetNewItemsEntries());
            yield return ContextEntrySeparator.Instance;
            foreach (IBaseContextEntry entry in this.GetSortingContextEntries()) {
                yield return entry;
            }

            yield return ContextEntrySeparator.Instance;
            foreach (IBaseContextEntry entry in base.GetContextEntries()) {
                yield return entry;
            }
        }

        public IEnumerable<IBaseContextEntry> GetSortingContextEntries() {
            yield return new ContextEntry("Sort By Type", this.SortByTypeCommand);
            yield return new ContextEntry("Sort By Name", this.SortByNameCommand);
            yield return new ContextEntry("Sort By Both", this.SortByBothCommand) {
                ToolTip = "This is what NBTExplorer sorts by; compound, list, array, primitive and then finally by name"
            };

            yield return new LazyASFContextEntry("Expand hierarchy", () => {
                IoC.MainExplorer.TreeView.Behaviour.ExpandItemHierarchy(this);
            },  () => this.Children.Count > 0);
        }

        public IEnumerable<IBaseContextEntry> GetNewItemsEntries() {
            // yield return new ContextEntry("End", this.CreateTagCommand, 0);
            yield return new ContextEntry("Byte", this.CreateTagCommand, 1);
            yield return new ContextEntry("Short", this.CreateTagCommand, 2);
            yield return new ContextEntry("Int", this.CreateTagCommand, 3);
            yield return new ContextEntry("Long", this.CreateTagCommand, 4);
            yield return new ContextEntry("Float", this.CreateTagCommand, 5);
            yield return new ContextEntry("Double", this.CreateTagCommand, 6);
            yield return new ContextEntry("String", this.CreateTagCommand, 8);
            yield return new ContextEntry("Byte Array", this.CreateTagCommand, 7);
            yield return new ContextEntry("Int Array", this.CreateTagCommand, 11);
            yield return new ContextEntry("List", this.CreateTagCommand, 9);
            yield return new ContextEntry("Compound", this.CreateTagCommand, 10);
        }

        public BaseNBTViewModel FindChildByName(string name) {
            return this.Children.FirstOrDefault(x => x.Name == name);
        }
    }
}