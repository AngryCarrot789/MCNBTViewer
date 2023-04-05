using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MCNBTViewer.Core.Utils;

namespace MCNBTViewer.Core.Explorer.New {
    public class ExplorerFolderViewModel : BaseExplorerItemViewModel {
        private readonly EfficientObservableCollection<BaseExplorerItemViewModel> items;
        public ReadOnlyObservableCollection<BaseExplorerItemViewModel> Items { get; }

        public bool IsEmpty => this.items.Count < 1;

        private bool isExpanded;
        public bool IsExpanded {
            get => this.isExpanded;
            set => this.RaisePropertyChanged(ref this.isExpanded, value);
        }

        public ExplorerFolderViewModel() {
            this.items = new EfficientObservableCollection<BaseExplorerItemViewModel>();
            this.Items = new ReadOnlyObservableCollection<BaseExplorerItemViewModel>(this.items);
        }

        public virtual void AddRange(IEnumerable<BaseExplorerItemViewModel> enumerable) {
            List<BaseExplorerItemViewModel> list = enumerable.ToList();
            this.items.AddRange(list);
            this.EnsureParents(list, true);
            this.RaiseIsEmptyChanged();
        }

        public virtual void Add(BaseExplorerItemViewModel item) {
            this.items.Add(item);
            this.RaiseIsEmptyChanged();
        }

        public virtual void Insert(int index, BaseExplorerItemViewModel item) {
            this.items.Insert(index, item);
            this.EnsureParent(item, true);
            this.RaiseIsEmptyChanged();
        }

        public virtual void InsertRange(int index, IEnumerable<BaseExplorerItemViewModel> enumerable) {
            List<BaseExplorerItemViewModel> list = enumerable.ToList();
            this.items.InsertRange(index, list);
            this.EnsureParents(list, true);
            this.RaiseIsEmptyChanged();
        }

        public virtual bool Contains(BaseExplorerItemViewModel item) {
            return this.items.Contains(item);
        }

        public virtual bool Remove(BaseExplorerItemViewModel item) {
            int index = this.IndexOf(item);
            if (index < 0) {
                return false;
            }

            this.RemoveAt(index);
            return true;
        }

        public virtual void RemoveAll(IEnumerable<BaseExplorerItemViewModel> enumerable) {
            foreach (BaseExplorerItemViewModel item in enumerable) {
                this.Remove(item);
            }
        }

        public virtual void RemoveAll(Predicate<BaseExplorerItemViewModel> canRemove) {
            // this.RemoveAll(this.items.Where(canRemove).ToList());
            List<BaseExplorerItemViewModel> list = this.items.ToList();
            for (int i = list.Count - 1; i >= 0; i--) {
                BaseExplorerItemViewModel item = list[i];
                if (canRemove(item)) {
                    this.EnsureParent(item, false);
                    this.items.RemoveAt(i);
                }
            }

            this.RaiseIsEmptyChanged();
        }

        public virtual int IndexOf(BaseExplorerItemViewModel item) {
            return this.items.IndexOf(item);
        }

        public virtual void RemoveAt(int index) {
            this.EnsureParent(this.items[index], false);
            this.items.RemoveAt(index);
            this.RaiseIsEmptyChanged();
        }

        public virtual void Clear() {
            this.EnsureParents(this.items, false);
            this.RaiseIsEmptyChanged();
            this.items.Clear();
        }

        public virtual void RaiseIsEmptyChanged() {
            this.RaisePropertyChanged(nameof(this.IsEmpty));
        }

        protected virtual void EnsureParent(BaseExplorerItemViewModel item, bool valid) {
            if (item != null) {
                item.Parent = valid ? this : null;
                item.Explorer = valid ? this.Explorer : null;
            }
        }

        protected virtual void EnsureParents(IEnumerable<BaseExplorerItemViewModel> enumerable, bool valid) {
            foreach (BaseExplorerItemViewModel item in enumerable) {
                this.EnsureParent(item, valid);
            }
        }
    }
}