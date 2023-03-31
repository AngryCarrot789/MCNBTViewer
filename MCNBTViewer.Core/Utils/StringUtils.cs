namespace MCNBTViewer.Core.Utils {
    public static class StringUtils {
        public static string JSubstring(this string @this, int startIndex, int endIndex) {
            return @this.Substring(startIndex, endIndex - startIndex);
        }

        public static bool IsEmpty(this string @this) {
            return string.IsNullOrEmpty(@this);
        }
    }
}