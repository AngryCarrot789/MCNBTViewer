using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

        public RelayCommand PasteNBTBinaryDataCommand { get; }

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

            this.PasteNBTBinaryDataCommand = new RelayCommand(this.PasteNBTBinaryDataAction);
        }

        private async Task NewTagAction(int type) {
            string name;
            NBTBase nbt;
            switch (type) {
                case 1:  { (string, NBTTagByte)? x = IoC.TagDialogService.CreateTagByte(this is NBTCompoundViewModel);            if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; }break;
                case 2:  { (string, NBTTagShort)? x = IoC.TagDialogService.CreateTagShort(this is NBTCompoundViewModel);          if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; }break;
                case 3:  { (string, NBTTagInt)? x = IoC.TagDialogService.CreateTagInt(this is NBTCompoundViewModel);              if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; }break;
                case 4:  { (string, NBTTagLong)? x = IoC.TagDialogService.CreateTagLong(this is NBTCompoundViewModel);            if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; }break;
                case 5:  { (string, NBTTagFloat)? x = IoC.TagDialogService.CreateTagFloat(this is NBTCompoundViewModel);          if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; }break;
                case 6:  { (string, NBTTagDouble)? x = IoC.TagDialogService.CreateTagDouble(this is NBTCompoundViewModel);        if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; }break;
                case 8:  { (string, NBTTagByteArray)? x = IoC.TagDialogService.CreateTagByteArray(this is NBTCompoundViewModel);  if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; }break;
                case 7:  { (string, NBTTagString)? x = IoC.TagDialogService.CreateTagString(this is NBTCompoundViewModel);        if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; }break;
                case 11: { (string, NBTTagIntArray)? x = IoC.TagDialogService.CreateTagIntArray(this is NBTCompoundViewModel);    if (!x.HasValue) return; name = x.Value.Item1; nbt = x.Value.Item2; }break;
                default: return;
            }

            if (this is NBTCompoundViewModel && string.IsNullOrEmpty(name)) {
                await IoC.MessageDialogs.ShowMessageAsync("Tag name is empty", "The tag name cannot be an empty string");
                return;
            }

            if (this.Children.Any(x => x.Name == name)) {
                await IoC.MessageDialogs.ShowMessageAsync("Tag already exists", "A tag already exists with the name " + name + ": " + this.Children.FirstOrDefault(x => x.Name == name));
                return;
            }

            this.Children.Add(CreateFrom(name, nbt));
        }

        private void PasteNBTBinaryDataAction() {
            // this.CopyBinaryToClipboardCommand = new RelayCommand(async () => {
            //     if (IoC.Clipboard == null) {
            //         await IoC.MessageDialogs.ShowMessageAsync("No clipboard", "Clipboard is unavailable. Cannot copy the NBT to the clipboard");
            //     }
            //     else {
            //         using (MemoryStream stream = new MemoryStream()) {
            //             try {
            //                 NBTBase nbt = this.ToNBT();
            //                 NBTBase.WriteTag(new DataOutputStream(stream), this.Name, nbt);
            //                 IoC.Clipboard.SetBinaryTag("TAG_NBT", stream.ToArray());
            //             }
            //             catch (Exception e) {
            //                 await IoC.MessageDialogs.ShowMessageAsync("Failed to write NBT", "Failed to write NBT to clipboard: " + e.Message);
            //             }
            //         }
            //     }
            // });
        }

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

        protected override bool CanExpandTreeItem() {
            return this.Children.Count > 0;
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
            // yield return new ContextEntry("List", this.CreateTagCommand, 9);
            // yield return new ContextEntry("Compound", this.CreateTagCommand, 10);
        }
    }
}