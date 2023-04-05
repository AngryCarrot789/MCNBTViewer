using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MCNBTViewer.Core.Utils;

namespace MCNBTViewer.Core.Explorer.New.Trees {
    public class TreeFolderViewModel : TreeItemViewModel {
        protected readonly EfficientObservableCollection<TreeItemViewModel> items;
        public ReadOnlyObservableCollection<TreeItemViewModel> Items { get; }

        public bool IsEmpty => this.items.Count < 1;

        private bool isExpanded;
        public bool IsExpanded {
            get => this.isExpanded;
            set => this.RaisePropertyChanged(ref this.isExpanded, value);
        }

        public TreeFolderViewModel() {
            this.items = new EfficientObservableCollection<TreeItemViewModel>();
            this.Items = new ReadOnlyObservableCollection<TreeItemViewModel>(this.items);
        }

        public virtual void AddRange(IEnumerable<TreeItemViewModel> enumerable) {
            List<TreeItemViewModel> list = enumerable.ToList();
            this.items.AddRange(list);
            this.EnsureParents(list, true);
            this.RaiseIsEmptyChanged();
        }

        public virtual void Add(TreeItemViewModel item) {
            this.items.Add(item);
            this.RaiseIsEmptyChanged();
        }

        public virtual void Insert(int index, TreeItemViewModel item) {
            this.items.Insert(index, item);
            this.EnsureParent(item, true);
            this.RaiseIsEmptyChanged();
        }

        public virtual void InsertRange(int index, IEnumerable<TreeItemViewModel> enumerable) {
            List<TreeItemViewModel> list = enumerable.ToList();
            this.items.InsertRange(index, list);
            this.EnsureParents(list, true);
            this.RaiseIsEmptyChanged();
        }

        public virtual bool Contains(TreeItemViewModel item) {
            return this.items.Contains(item);
        }

        public virtual bool Remove(TreeItemViewModel item) {
            int index = this.IndexOf(item);
            if (index < 0) {
                return false;
            }

            this.RemoveAt(index);
            return true;
        }

        public virtual void RemoveAll(IEnumerable<TreeItemViewModel> enumerable) {
            foreach (TreeItemViewModel item in enumerable) {
                this.Remove(item);
            }
        }

        public virtual void RemoveAll(Predicate<TreeItemViewModel> canRemove) {
            // this.RemoveAll(this.items.Where(canRemove).ToList());
            List<TreeItemViewModel> list = this.items.ToList();
            for (int i = list.Count - 1; i >= 0; i--) {
                TreeItemViewModel item = list[i];
                if (canRemove(item)) {
                    this.EnsureParent(item, false);
                    this.items.RemoveAt(i);
                }
            }

            this.RaiseIsEmptyChanged();
        }

        public virtual int IndexOf(TreeItemViewModel item) {
            return this.items.IndexOf(item);
        }

        public virtual void RemoveAt(int index) {
            this.EnsureParent(this.items[index], false);
            this.items.RemoveAt(index);
            this.RaiseIsEmptyChanged();
        }

        public virtual void Clear() {
            this.EnsureParents(this.items, false);
            this.items.Clear();
            this.RaiseIsEmptyChanged();
        }

        public virtual void RaiseIsEmptyChanged() {
            this.RaisePropertyChanged(nameof(this.IsEmpty));
        }

        protected virtual void EnsureParent(TreeItemViewModel item, bool valid) {
            if (item != null) {
                item.ParentTreeExpander = valid ? this : null;
            }
        }

        protected virtual void EnsureParents(IEnumerable<TreeItemViewModel> enumerable, bool valid) {
            foreach (TreeItemViewModel item in enumerable) {
                this.EnsureParent(item, valid);
            }
        }
    }
}