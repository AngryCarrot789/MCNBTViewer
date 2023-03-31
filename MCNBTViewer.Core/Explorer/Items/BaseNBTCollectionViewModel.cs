using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using FramePFX.Core;
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

        protected BaseNBTCollectionViewModel(string name, NBTType type) : base(name, type) {
            this.Children = new ObservableCollection<BaseNBTViewModel>();
            this.Children.CollectionChanged += this.OnChildrenChanged;

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
    }
}