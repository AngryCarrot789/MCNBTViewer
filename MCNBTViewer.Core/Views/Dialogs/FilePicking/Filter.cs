using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MCNBTViewer.Core.Views.Dialogs.FilePicking {
    public class FileFilter {
        private readonly StringBuilder sb;
        private bool hasFirst;

        public FileFilter(StringBuilder sb) {
            this.sb = sb;
        }

        public FileFilter AddFilter(string readableName, string extension) {
            if (string.IsNullOrEmpty(extension)) {
                throw new ArgumentException("Extension cannot be null, empty, or consist of only whitespaces");
            }

            if (string.IsNullOrWhiteSpace(readableName)) {
                readableName = extension.ToUpper();
            }

            if (this.hasFirst) {
                this.sb.Append('|');
            }
            else {
                this.hasFirst = true;
            }

            this.sb.Append(readableName).Append('|').Append(extension);
            return this;
        }

        public string GetFilter() {
            string filter = string.Empty;
            if (this.filters.Count > 0) {
                filter = string.Join("|", this.filters.Select(f => $"{f.Name ?? f.Extension.ToUpper()} ({f.Extension.ToUpper()})|*.{f.Extension}"));
                filter += "|";
            }

            filter += "All Files (*.*)|*.*";
            return filter;
        }
    }
}