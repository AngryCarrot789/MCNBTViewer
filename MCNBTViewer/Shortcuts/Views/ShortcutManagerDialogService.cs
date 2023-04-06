using System;
using MCNBTViewer.Core.Shortcuts.Dialogs;
using MCNBTViewer.Core.Shortcuts.ViewModels;

namespace MCNBTViewer.Shortcuts.Views {
    public class ShortcutManagerDialogService : IShortcutManagerDialogService {
        private ShortcutEditorWindow window;

        public bool IsOpen => this.window != null;

        public void ShowEditorDialog() {
            if (this.window != null) {
                return;
            }

            this.window = new ShortcutEditorWindow();
            ShortcutManagerViewModel manager = new ShortcutManagerViewModel();
            manager.LoadFromRoot(AppShortcutManager.Instance.Root);
            this.window.DataContext = manager;
            this.window.Closed += (sender, args) => {
                this.window = null;
            };

            this.window.Show();
        }
    }
}