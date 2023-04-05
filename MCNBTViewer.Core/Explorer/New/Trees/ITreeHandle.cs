using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MCNBTViewer.Core.Explorer.Items;
using MCNBTViewer.Core.Explorer.New.NotVeryGood;

namespace MCNBTViewer.Core.Explorer.New.NBT {
    public interface ITreeHandle {
        object SelectedItem { get; }

        bool ExpandHierarchyFromRoot(IEnumerable<BaseExplorerItemViewModel> items, bool select = true);

        Task<bool> RepeatExpandHierarchyFromRootAsync(IEnumerable<BaseExplorerItemViewModel> items, bool select = true);

        Task ExpandItemHierarchy(BaseExplorerItemViewModel item);
    }
}