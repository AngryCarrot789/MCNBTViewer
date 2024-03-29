using System.Collections.Generic;
using System.Threading.Tasks;
using MCNBTViewer.Core.Explorer.New.NotVeryGood;

namespace MCNBTViewer.Core.Explorer.New.Trees {
    public interface ITreeHandle {
        object SelectedItem { get; }

        bool ExpandHierarchyFromRoot(IEnumerable<BaseExplorerItemViewModel> items, bool select = true);

        Task<bool> RepeatExpandHierarchyFromRootAsync(IEnumerable<BaseExplorerItemViewModel> items, bool select = true);

        Task ExpandItemHierarchy(BaseExplorerItemViewModel item);
    }
}