using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MCNBTViewer.Core.Utils;

namespace MCNBTViewer.Core.Explorer.New.Trees {
    public class BaseTreeFolderViewModel : BaseTreeItemViewModel {
        protected readonly EfficientObservableCollection<BaseTreeItemViewModel> items;
        public ReadOnlyObservableCollection<BaseTreeItemViewModel> Items { get; }

        public bool IsEmpty => this.items.Count < 1;

        private bool isExpanded;
        public bool IsExpanded {
            get => this.isExpanded;
            set => this.RaisePropertyChanged(ref this.isExpanded, value);
        }

        protected BaseTreeFolderViewModel() {
            this.items = new EfficientObservableCollection<BaseTreeItemViewModel>();
            this.Items = new ReadOnlyObservableCollection<BaseTreeItemViewModel>(this.items);
        }

        public virtual void AddRange(IEnumerable<BaseTreeItemViewModel> enumerable) {
            List<BaseTreeItemViewModel> list = enumerable.ToList();
            this.items.AddRange(list);
            this.EnsureParents(list, true);
            this.RaiseIsEmptyChanged();
        }

        public virtual void Add(BaseTreeItemViewModel item) {
            this.items.Add(item);
            this.RaiseIsEmptyChanged();
        }

        public virtual void Insert(int index, BaseTreeItemViewModel item) {
            this.items.Insert(index, item);
            this.EnsureParent(item, true);
            this.RaiseIsEmptyChanged();
        }

        public virtual void InsertRange(int index, IEnumerable<BaseTreeItemViewModel> enumerable) {
            List<BaseTreeItemViewModel> list = enumerable.ToList();
            this.items.InsertRange(index, list);
            this.EnsureParents(list, true);
            this.RaiseIsEmptyChanged();
        }

        public virtual bool Contains(BaseTreeItemViewModel item) {
            return this.items.Contains(item);
        }

        public virtual bool Remove(BaseTreeItemViewModel item) {
            int index = this.IndexOf(item);
            if (index < 0) {
                return false;
            }

            this.RemoveAt(index);
            return true;
        }

        public virtual void RemoveAll(IEnumerable<BaseTreeItemViewModel> enumerable) {
            foreach (BaseTreeItemViewModel item in enumerable) {
                this.Remove(item);
            }
        }

        public virtual void RemoveAll(Predicate<BaseTreeItemViewModel> canRemove) {
            // this.RemoveAll(this.items.Where(canRemove).ToList());
            List<BaseTreeItemViewModel> list = this.items.ToList();
            for (int i = list.Count - 1; i >= 0; i--) {
                BaseTreeItemViewModel item = list[i];
                if (canRemove(item)) {
                    this.EnsureParent(item, false);
                    this.items.RemoveAt(i);
                }
            }

            this.RaiseIsEmptyChanged();
        }

        public virtual int IndexOf(BaseTreeItemViewModel item) {
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

        protected virtual void EnsureParent(BaseTreeItemViewModel item, bool valid) {
            if (item != null) {
                item.ParentTreeExpander = valid ? this : null;
            }
        }

        protected virtual void EnsureParents(IEnumerable<BaseTreeItemViewModel> enumerable, bool valid) {
            foreach (BaseTreeItemViewModel item in enumerable) {
                this.EnsureParent(item, valid);
            }
        }
    }
}