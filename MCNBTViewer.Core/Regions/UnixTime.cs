using System;

namespace MCNBTViewer.Core.Regions {
    public static class MCTimeStamp {
        public static int GetTime() => (int) ((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).Ticks / 10000000L);

        public static int GetTime(DateTime time) {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (int) ((time - dateTime).Ticks / 10000000L);
        }
    }
}