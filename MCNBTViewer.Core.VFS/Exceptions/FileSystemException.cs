using System;

namespace MCNBTViewer.Core.VFS.Exceptions {
    public class FileSystemException : Exception {
        public FileSystemException(string message) : base(message) {

        }

        public FileSystemException(string message, Exception innerException) : base(message, innerException) {

        }
    }
}