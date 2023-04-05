using MCNBTViewer.Core.AdvancedContextMenu.Base;

namespace MCNBTViewer.Core.AdvancedContextMenu {
    /// <summary>
    /// A separator element between menu items
    /// </summary>
    public class ContextEntrySeparator : IContextEntry {
        public static ContextEntrySeparator Instance { get; } = new ContextEntrySeparator();
    }
}