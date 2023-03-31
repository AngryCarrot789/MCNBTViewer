using System.Collections.Generic;

namespace MCNBTViewer.Core.Views.ViewModels {
    public interface IErrorAccumulator {
        void Accumulate(Dictionary<string, object> errors);
    }
}