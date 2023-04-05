using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MCNBTViewer.Core.Explorer.Items;
using MCNBTViewer.Core.Utils;

namespace MCNBTViewer.Core.Explorer {
    public class NBTExplorerViewModel : BaseViewModel {
        private readonly EfficientObservableCollection<BaseViewModel> rootFiles;
        public ReadOnlyObservableCollection<BaseViewModel> RootFiles { get; }

        public bool IsEmpty => this.rootFiles.Count < 1;

        public bool IgnoreSelectionChanged { get; set; }

        public NBTDataFileViewModel RootDataFileForSelectedItem {
            get {
                BaseViewModel item = this.SelectedItem;
                if (item is NBTDataFileViewModel model) {
                    return model;
                }
                else if (item is BaseNBTViewModel baseNbt && (baseNbt = baseNbt.RootParent) is NBTDataFileViewModel) {
                    return (NBTDataFileViewModel) baseNbt;
                }
                else {
                    return null;
                }
            }
        }

        public NBTExplorerListViewModel ExplorerList { get; }

        public ITreeView TreeView { get; set; }

        public IMainList ExplorerListHandle { get; set; }

        public BaseViewModel SelectedItem => (BaseViewModel) this.TreeView.GetSelectedItem();

        public NBTExplorerViewModel() {
            this.rootFiles = new EfficientObservableCollection<BaseViewModel>();
            this.RootFiles = new ReadOnlyObservableCollection<BaseViewModel>(this.rootFiles);
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
            IList source = this.RootFiles;
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
            if (string.IsNullOrEmpty(name)) {
                return null;
            }

            foreach (object child in children) {
                if (child is BaseNBTViewModel tag && tag.Name == name) {
                    return tag;
                }
            }

            // finally try to parse the index. the above checks just in case a tag was actually named [3] for example
            if (name[0] == '[' && name[name.Length - 1] == ']') {
                if (int.TryParse(name.JSubstring(1, name.Length - 1), out int index) && index >= 0 && index < children.Count) {
                    return children[index] as BaseNBTViewModel;
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
            if (this.rootFiles.Remove(file)) {
                if (isSelected) {
                    if (this.rootFiles.FirstOrDefault() is BaseNBTCollectionViewModel anyCollection) {
                        this.ExplorerList.CurrentFolder = anyCollection;
                        if (anyCollection.Children.Count > 0) {
                            this.ExplorerList.SelectedItem = anyCollection.Children[0];
                        }
                    }
                    else {
                        this.ExplorerList.CurrentFolder = null;
                        this.ExplorerList.SelectedItem = null;
                    }
                }

                this.RaisePropertyChanged(nameof(this.IsEmpty));
            }
        }

        public void OnTreeSelectItem(BaseNBTViewModel item) {
            if (this.IgnoreSelectionChanged) {
                return;
            }

            this.IgnoreSelectionChanged = true;
            try {
                if (item is BaseNBTCollectionViewModel model) {
                    this.ExplorerList.CurrentFolder = model;
                    if (model.Children.Count > 0) {
                        this.ExplorerList.SelectedItem = model.Children[0];
                    }
                }
                else if (item != null) {
                    if (this.ExplorerList.CurrentFolder == null || this.ExplorerList.CurrentFolder != item.Parent) {
                        // Make sure the list is showing the current folder
                        this.ExplorerList.CurrentFolder = item.Parent;
                        // this.ExplorerList.SelectedItem = item;
                    }

                    this.ExplorerList.SelectedItem = item;
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

        public async Task UseItem(BaseNBTViewModel file) {
            if (file is BaseNBTCollectionViewModel) {
                await this.TreeView.Behaviour.RepeatExpandHierarchyFromRootAsync(file.ParentChain);
            }
            else if (file is NBTPrimitiveViewModel primitive) {
                await primitive.EditAction();
            }
            else if (file is BaseNBTArrayViewModel array) {
                // not sure if i should make RenameAction async or not... probably should
                await array.EditAction();
            }
        }

        public void AddChild(BaseViewModel item) {
            if (item != null) {
                this.rootFiles.Add(item);
                this.RaisePropertyChanged(nameof(this.IsEmpty));
            }
        }
    }
}