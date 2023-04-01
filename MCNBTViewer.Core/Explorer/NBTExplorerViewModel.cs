using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using MCNBTViewer.Core.Explorer.Items;
using MCNBTViewer.Core.NBT;
using MCNBTViewer.Core.Utils;

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

        public IMainList ExplorerListHandle { get; set; }

        public NBTExplorerViewModel() {
            this.loadedDataFiles = new ObservableCollection<NBTDataFileViewModel>();
            this.LoadedDataFiles = new ReadOnlyObservableCollection<NBTDataFileViewModel>(this.loadedDataFiles);
            this.ExplorerList = new NBTExplorerListViewModel(this);
        }

        public async Task NavigateToPath(string path) {
            List<BaseNBTViewModel> list;
            try {
                list = this.ResolvePath(path).ToList();
            }
            catch (Exception e) {
                await IoC.MessageDialogs.ShowMessageAsync("Failed to resolve path", e.Message);
                return;
            }

            await this.TreeView.Behaviour.RepeatExpandHierarchyFromRootAsync(list);
        }

        public IEnumerable<BaseNBTViewModel> ResolvePath(string path) {
            int i, j = 0;
            string name;
            BaseNBTViewModel tag;
            IList source = this.LoadedDataFiles;
            while ((i = path.IndexOf('/', j)) >= 0) {
                tag = GetChild(source, name = path.JSubstring(j, i));
                if (tag == null) {
                    throw GetNameException(name, j == 0 ? "<root>" : path.Substring(0, j - 1));
                }
                else if (tag is BaseNBTCollectionViewModel) {
                    source = ((BaseNBTCollectionViewModel) tag).Children;
                    yield return tag;
                }
                else {
                    throw new Exception($"Expected collection at '{(j == 0 ? "<root>" : path.Substring(0, i))}', but got {tag.NBTType}");
                }

                j = i + 1;
            }

            tag = GetChild(source, name = path.Substring(j));
            if (tag == null) {
                throw GetNameException(name, j == 0 ? "<root>" : path.Substring(0, j - 1));
            }

            yield return tag;
        }

        private static BaseNBTViewModel GetChild(IList children, string name) {
            if (!string.IsNullOrEmpty(name) && name[0] == '[' && name[name.Length - 1] == ']') {
                if (int.TryParse(name.JSubstring(1, name.Length - 1), out int index) && index >= 0 && index < children.Count) {
                    return (BaseNBTViewModel) children[index];
                }
            }

            foreach (BaseNBTViewModel child in children) {
                if (child.Name == name) {
                    return child;
                }
            }

            return null;
        }

        private static Exception GetNameException(string name, string path) {
            if (name[0] == '[' && name[name.Length - 1] == ']') {
                throw new Exception($"No such child at index '{name.JSubstring(1, name.Length - 1)}' in: '{path}'");
            }
            else {
                throw new Exception($"No such child by the name of '{name}' in: '{path}'");
            }
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
            if (file is BaseNBTCollectionViewModel) {
                this.TreeView.Behaviour.ExpandHierarchyFromRoot(file.ParentChain);
            }
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