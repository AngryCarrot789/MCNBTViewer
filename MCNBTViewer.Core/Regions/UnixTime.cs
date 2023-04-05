using System;

namespace MCNBTViewer.Core.Regions {
    public static class UnixTime {
        public static int GetUnixTime() {
            return GetUnixTime(DateTime.UtcNow);
        }

        public static int GetUnixTime(DateTime time) {
            DateTime start = new DateTime(1970, 1, 1);
            return (int) ((time - start).Ticks / 10000000L);
        }
    }
}