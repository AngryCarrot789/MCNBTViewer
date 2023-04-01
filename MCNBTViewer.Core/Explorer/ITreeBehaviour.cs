using System.Collections.Generic;
using System.Threading.Tasks;
using MCNBTViewer.Core.Explorer.Items;

namespace MCNBTViewer.Core.Explorer {
    public interface ITreeBehaviour {
        void SetExpanded(BaseNBTViewModel nbt);
        bool IsExpanded(BaseNBTViewModel nbt);
        Task<bool> RepeatExpandHierarchyFromRootAsync(IEnumerable<BaseNBTViewModel> items, bool select = true);
        bool ExpandHierarchyFromRoot(IEnumerable<BaseNBTViewModel> items, bool select = true);
    }
}