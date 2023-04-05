using System.Collections.Generic;
using System.Windows.Input;
using MCNBTViewer.Core.AdvancedContextMenu.Base;

namespace MCNBTViewer.Core.AdvancedContextMenu {
    public class CheckableCommandContextEntry : CommandContextEntry {
        private bool isChecked;
        public bool IsChecked {
            get => this.isChecked;
            set => this.RaisePropertyChanged(ref this.isChecked, value);
        }

        public CheckableCommandContextEntry(IEnumerable<IContextEntry> children = null) : base(children) {
        }

        public CheckableCommandContextEntry(ICommand command, object commandParameter, IEnumerable<IContextEntry> children = null) : base(command, commandParameter, children) {
        }

        public CheckableCommandContextEntry(object dataContext, ICommand command, object commandParameter, IEnumerable<IContextEntry> children = null) : base(dataContext, command, commandParameter, children) {
        }

        public CheckableCommandContextEntry(object dataContext, string header, ICommand command, object commandParameter, IEnumerable<IContextEntry> children = null) : base(dataContext, header, command, commandParameter, children) {
        }

        public CheckableCommandContextEntry(object dataContext, string header, ICommand command, object commandParameter, string inputGestureText, IEnumerable<IContextEntry> children = null) : base(dataContext, header, command, commandParameter, inputGestureText, children) {
        }

        public CheckableCommandContextEntry(object dataContext, string header, ICommand command, object commandParameter, string inputGestureText, string toolTip, IEnumerable<IContextEntry> children = null) : base(dataContext, header, command, commandParameter, inputGestureText, toolTip, children) {
        }
    }
}