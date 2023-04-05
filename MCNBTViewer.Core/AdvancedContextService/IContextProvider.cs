using System.Collections.Generic;
using MCNBTViewer.Core.AdvancedContextMenu.Base;

namespace MCNBTViewer.Core.AdvancedContextMenu {
    public interface IContextProvider {
        void GetContext(List<IContextEntry> list);
    }
}