using System.Collections.Generic;

namespace MCNBTViewer.Core.AdvancedContextService {
    public interface IContextProvider {
        IEnumerable<IBaseContextEntry> GetContextEntries();
    }
}