using System.Diagnostics;
using System.IO;
using MCNBTViewer.Core.Services;

namespace MCNBTViewer.Services {
    public class WinOSFileExplorer : IOSFileExplorer {
        public void OpenFileInExplorer(string filePath) {
            if (File.Exists(filePath)) {
                Process.Start("explorer.exe", $"/select, \"{filePath.Replace('/', '\\')}\"");
            }
        }
    }
}