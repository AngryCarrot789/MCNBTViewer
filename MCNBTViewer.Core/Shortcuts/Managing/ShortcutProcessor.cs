using System.Collections.Generic;
using MCNBTViewer.Core.Shortcuts.Usage;

namespace MCNBTViewer.Core.Shortcuts.Managing {
    public class ShortcutUsageFrame {
        public ShortcutManager Manager { get; }

        public Dictionary<IShortcutUsage, ManagedShortcut> ActiveUsages { get; }
    }
}