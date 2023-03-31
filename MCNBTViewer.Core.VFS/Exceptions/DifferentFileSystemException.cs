using System;

namespace MCNBTViewer.Core.VFS.Exceptions {
    public class DifferentFileSystemException : FileSystemException {
        public DifferentFileSystemException(VirtualFileSystem a, VirtualFileSystem b) : this($"File systems to not match: '{a}' != '{b}'") {

        }

        public DifferentFileSystemException(string message) : base(message) {
        }

        public DifferentFileSystemException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}