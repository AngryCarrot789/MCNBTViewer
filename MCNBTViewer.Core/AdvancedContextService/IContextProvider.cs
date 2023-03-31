using System.Collections.Generic;

namespace FramePFX.Core.AdvancedContextService {
    public interface IContextProvider {
        IEnumerable<IBaseContextEntry> GetContextEntries();
    }
}