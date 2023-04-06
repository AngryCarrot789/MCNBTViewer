using MCNBTViewer.Core.Shortcuts.Dialogs;
using MCNBTViewer.Core.Shortcuts.Inputs;

namespace MCNBTViewer.Shortcuts.Dialogs {
    public class KeyboardDialogService : IKeyboardDialogService {
        public KeyStroke? ShowGetKeyStrokeDialog() {
            KeyStrokeInputWindow window = new KeyStrokeInputWindow();
            if (window.ShowDialog() != true || window.Stroke.Equals(default)) {
                return null;
            }
            else {
                return window.Stroke;
            }
        }
    }
}