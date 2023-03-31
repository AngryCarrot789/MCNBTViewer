using System;

namespace MCNBTViewer.Core.VFS.Exceptions {
    /// <summary>
    /// An exception used to indicate that the user (the actual person using the UI) made a mistake and an action cannot continue as a result
    /// </summary>
    public class UserMistakeException : FileSystemException {
        public UserMistakeException(string message) : base(message) {

        }

        public UserMistakeException(string message, Exception innerException) : base(message, innerException) {

        }
    }
}