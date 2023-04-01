using System;
using System.Collections.Generic;

namespace MCNBTViewer.Core.AdvancedContextService {
    public class LazyASFContextEntry : ContextEntry {
        public LazyASFContextEntry(string header, Action onCommand, IEnumerable<IBaseContextEntry> children = null) : base(header, new RelayCommand(onCommand), children) {
        }

        public LazyASFContextEntry(string header, Action onCommand, Func<bool> canExecute, IEnumerable<IBaseContextEntry> children = null) : base(header, new RelayCommand(onCommand, canExecute), children) {
        }

        public LazyASFContextEntry(string header, string inputGestureText, Action onCommand, IEnumerable<IBaseContextEntry> children = null) : base(header, inputGestureText, new RelayCommand(onCommand), children) {

        }
    }
}