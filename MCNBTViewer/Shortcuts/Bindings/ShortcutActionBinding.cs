using System;
using System.Windows;
using System.Windows.Input;
using MCNBTViewer.Core.Shortcuts.Managing;

namespace MCNBTViewer.Shortcuts.Bindings {
    public class ShortcutBinding : InputBinding {
        public static readonly DependencyProperty ShortcutAndUsageIdProperty =
            DependencyProperty.Register(
                "ShortcutAndUsageId",
                typeof(string),
                typeof(ShortcutBinding),
                new PropertyMetadata(null, (d, e) => ((ShortcutBinding) d).OnShouldcutAndUsageIdChanged((string) e.OldValue, (string) e.NewValue)));

        /// <summary>
        /// <para>
        /// The target shortcut and the usage id as a single string (cannot be two properties due to the <see cref="InputBinding"/> limitations,
        /// such as the lack of an Loaded or AddedToVisualTree event/function).
        /// </para>
        /// <para>
        /// The ShortcutID and UsageID are separated by a colon ':' character. The UsageID is optional, so you can just set this as the ShortcutID.
        /// A UsageID is only nessesary if you plan on using the same shortcut to invoke code in multiple places, so that only the right callback gets fired
        /// </para>
        /// <para>
        /// Examples: "Path/To/My/Shortcut:UsageID", "My/Action"
        /// </para>
        /// </summary>
        public string ShortcutAndUsageId {
            get => (string) this.GetValue(ShortcutAndUsageIdProperty);
            set => this.SetValue(ShortcutAndUsageIdProperty, value);
        }

        public string ShortcutId {
            get {
                ShortcutUtils.SplitValue(this.ShortcutAndUsageId, out string id, out _);
                return id;
            }
        }

        public string UsageId {
            get {
                ShortcutUtils.SplitValue(this.ShortcutAndUsageId, out _, out string id);
                return id;
            }
        }

        private readonly Action<ManagedShortcut> onShortcutFired;

        public ShortcutBinding() {
            this.onShortcutFired = this.OnShortcutFired;
        }

        private void OnShouldcutAndUsageIdChanged(string oldId, string newId) {
            if (!string.IsNullOrWhiteSpace(oldId)) {
                ShortcutUtils.SplitValue(oldId, out string shortcutId, out string usageId);
                AppShortcutManager.UnregisterHandler(shortcutId, usageId);
            }

            if (!string.IsNullOrWhiteSpace(newId)) {
                ShortcutUtils.SplitValue(newId, out string shortcutId, out string usageId);
                AppShortcutManager.RegisterHandler(shortcutId, usageId, this.onShortcutFired);
            }
        }

        public void OnShortcutFired(ManagedShortcut shortcut) {
            ICommand cmd = this.Command;
            object param = this.CommandParameter;
            if (cmd != null && cmd.CanExecute(param)) {
                cmd.Execute(param);
            }
        }

        protected override Freezable CreateInstanceCore() => new ShortcutBinding();
    }
}