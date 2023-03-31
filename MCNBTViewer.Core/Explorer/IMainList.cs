using System.Collections.Generic;
using MCNBTViewer.Core.Explorer.Items;

namespace MCNBTViewer.Core.Explorer {
    public interface IMainList {
        IEnumerable<BaseNBTViewModel> GetSelectedTags();
    }
}