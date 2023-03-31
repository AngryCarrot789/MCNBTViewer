using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MCNBTViewer.Core.Explorer.Items;
using MCNBTViewer.Core.NBT;

namespace MCNBTViewer.Core.Explorer {
    public class NBTExplorerViewModel : BaseViewModel {
        public bool IgnoreSelectionChanged { get; set; }

        public NBTDataFileViewModel RootDataFileForSelectedItem {
            get {
                BaseNBTViewModel item = this.SelectedFile;
                if (item is NBTDataFileViewModel model) {
                    return model;
                }
                else if (item != null && (item = item.RootParent) is NBTDataFileViewModel) {
                    return (NBTDataFileViewModel) item;
                }
                else {
                    return null;
                }
            }
        }

        public BaseNBTViewModel SelectedFile => (BaseNBTViewModel) this.TreeView.GetSelectedItem();

        private readonly ObservableCollection<NBTDataFileViewModel> loadedDataFiles;
        public ReadOnlyObservableCollection<NBTDataFileViewModel> LoadedDataFiles { get; }

        public NBTExplorerListViewModel ExplorerList { get; }

        public ITreeView TreeView { get; set; }

        public ICommand SortByTypeCommand { get; }
        public ICommand SortByNameCommand { get; }
        public ICommand SortByBothCommand { get; }

        public IMainList ExplorerListHandle { get; set; }

        public NBTExplorerViewModel() {
            this.loadedDataFiles = new ObservableCollection<NBTDataFileViewModel>();
            this.LoadedDataFiles = new ReadOnlyObservableCollection<NBTDataFileViewModel>(this.loadedDataFiles);
            this.ExplorerList = new NBTExplorerListViewModel(this);
            this.SortByTypeCommand = new RelayCommand(() => {
                BaseNBTViewModel selected = this.SelectedFile;
                if (selected != null && (selected is BaseNBTCollectionViewModel || (selected = selected.Parent) is BaseNBTCollectionViewModel)) {
                    BaseNBTCollectionViewModel collection = (BaseNBTCollectionViewModel) selected;
                    this.IgnoreSelectionChanged = true;
                    List<BaseNBTViewModel> list = collection.Children.OrderByDescending((a) => a.NBTType).ToList();
                    collection.Children.Clear();
                    foreach (BaseNBTViewModel model in list) {
                        collection.Children.Add(model);
                    }

                    this.IgnoreSelectionChanged = false;
                }
            });
            this.SortByNameCommand = new RelayCommand(() => {
                BaseNBTViewModel selected = this.SelectedFile;
                if (selected != null && (selected is BaseNBTCollectionViewModel || (selected = selected.Parent) is BaseNBTCollectionViewModel)) {
                    BaseNBTCollectionViewModel collection = (BaseNBTCollectionViewModel) selected;
                    this.IgnoreSelectionChanged = true;
                    List<BaseNBTViewModel> list = collection.Children.OrderBy((a) => a.Name ?? "").ToList();
                    collection.Children.Clear();
                    foreach (BaseNBTViewModel model in list) {
                        collection.Children.Add(model);
                    }

                    this.IgnoreSelectionChanged = false;
                }
            });
            this.SortByBothCommand = new RelayCommand(() => {
                BaseNBTViewModel selected = this.SelectedFile;
                if (selected != null && (selected is BaseNBTCollectionViewModel || (selected = selected.Parent) is BaseNBTCollectionViewModel)) {
                    BaseNBTCollectionViewModel collection = (BaseNBTCollectionViewModel) selected;
                    this.IgnoreSelectionChanged = true;
                    List<BaseNBTViewModel> list = new List<BaseNBTViewModel>(collection.Children);
                    list.Sort((a, b) => {
                        int cmp = a.NBTType.Compare4(b.NBTType);
                        if (cmp != 0) {
                            return cmp; // reverse order; compound on top, list below, then rest
                        }

                        return string.Compare(a.Name ?? "", b.Name ?? "", StringComparison.CurrentCulture);
                    });
                    collection.Children.Clear();
                    foreach (BaseNBTViewModel model in list) {
                        collection.Children.Add(model);
                    }

                    this.IgnoreSelectionChanged = false;
                }
            });
        }

        public void RemoveDatFile(NBTDataFileViewModel file) {
            bool isSelected = this.RootDataFileForSelectedItem == file;
            if (this.loadedDataFiles.Remove(file)) {
                if (isSelected) {
                    NBTDataFileViewModel any = this.loadedDataFiles.FirstOrDefault();
                    if (any == null) {
                        this.ExplorerList.CurrentFolder = null;
                        this.ExplorerList.SelectedItem = null;
                    }
                    else {
                        this.ExplorerList.CurrentFolder = any;
                        if (any.Children.Count > 0) {
                            this.ExplorerList.SelectedItem = any.Children[0];
                        }
                    }
                }
            }
        }

        public void OnTreeSelectItem(BaseNBTViewModel item) {
            if (this.IgnoreSelectionChanged) {
                return;
            }

            this.IgnoreSelectionChanged = true;
            try {
                if (item is NBTDataFileViewModel model && this.loadedDataFiles.Contains(item)) {
                    this.ExplorerList.CurrentFolder = model;
                    if (model.Children.Count > 0) {
                        this.ExplorerList.SelectedItem = model.Children[0];
                    }
                }
                else if (item != null) {
                    if (item is BaseNBTCollectionViewModel collection) { // && this.TreeView.IsItemExpanded(collection)
                        this.ExplorerList.CurrentFolder = collection;
                        if (collection.Children.Count > 0) {
                            this.ExplorerList.SelectedItem = collection.Children[0];
                        }
                    }
                    else {
                        if (this.ExplorerList.CurrentFolder == null || this.ExplorerList.CurrentFolder != item.Parent) {
                            // Make sure the list is showing the current folder
                            this.ExplorerList.CurrentFolder = item.Parent;
                            // this.ExplorerList.SelectedItem = item;
                        }

                        this.ExplorerList.SelectedItem = item;
                    }
                }
                else {
                    this.ExplorerList.CurrentFolder = null;
                    this.ExplorerList.SelectedItem = null;
                }
            }
            finally {
                this.IgnoreSelectionChanged = false;
            }
        }

        public void UseItem(BaseNBTViewModel file) {

        }

        public void SetSelectedItem(BaseNBTViewModel item) {
            this.TreeView.SetSelectedFile(item);
        }

        public void AddDataFile(NBTDataFileViewModel file) {
            if (file != null) {
                this.loadedDataFiles.Add(file);
            }
        }
    }
}