using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MCNBTViewer.Explorer {
    public class FolderItemViewModel : FileItemViewModel {
        /// <summary>
        /// This file's children. If this instance is <see cref="FileItemViewModel"/> then this will be
        /// empty. Only <see cref="FolderItemViewModel"/> or any over "folder" type derivation should have children (<see cref="CanExpand"/>)
        /// </summary>
        public ObservableCollection<FileItemViewModel> Children { get; }

        public FolderItemViewModel() {
            this.Children = new ObservableCollection<FileItemViewModel>();
            this.Children.CollectionChanged += this.OnChildrenModified;
        }

        private void OnChildrenModified(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.OldItems != null) {
                foreach (FileItemViewModel file in e.OldItems) {
                    file.OnRemovedFromFolder();
                    file.Parent = null;
                }
            }

            if (e.NewItems != null) {
                foreach (FileItemViewModel file in e.NewItems) {
                    file.Parent = this;
                    file.OnAddedToFolder();
                }
            }
        }

        protected override bool QueryCanExpand() {
            return this.Children.Count > 0;
        }

        protected override void OnNameChanged(string oldName, string newName) {

        }
    }
}