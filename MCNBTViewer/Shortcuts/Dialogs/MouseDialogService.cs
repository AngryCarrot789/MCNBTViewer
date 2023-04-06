using MCNBTViewer.Core.Shortcuts.Dialogs;
using MCNBTViewer.Core.Shortcuts.Inputs;

namespace MCNBTViewer.Shortcuts.Dialogs {
    public class MouseDialogService : IMouseDialogService {
        public MouseStroke? ShowGetMouseStrokeDialog() {
            MouseStrokeInputWindow window = new MouseStrokeInputWindow();
            if (window.ShowDialog() != true || window.Stroke.Equals(default)) {
                return null;
            }
            else {
                return window.Stroke;
            }
        }
    }
}