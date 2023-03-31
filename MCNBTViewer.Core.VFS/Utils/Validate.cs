using System;

namespace MCNBTViewer.Core.VFS.Utils {
    public static class Validate {
        public static void NotNull<T>(T value, string message = null) where T : class {
            if (value == null) {
                throw new ArgumentNullException(nameof(value), message ?? "Value is null");
            }
        }
    }
}