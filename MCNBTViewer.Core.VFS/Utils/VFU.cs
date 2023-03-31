using System;
using MCNBTViewer.Core.VFS.Exceptions;

namespace MCNBTViewer.Core.VFS.Utils {
    public static class VFU {
        // These could be moved into their own classes to support I18N

        public static void ValidateIsValidFile(VirtualFileBase file, string isNotValidMessage = null) {
            if (!file.IsValid) {
                throw new InvalidOperationException(isNotValidMessage ?? ("File is invalid: " + file.Name));
            }
        }

        public static void ValidateFileSystems(VirtualFileBase file, VirtualFileSystem system, string isNotDirectoryMessage = null) {
            if (system != file.FileSystem) {
                throw new InvalidOperationException(isNotDirectoryMessage ?? ("File systems do not match: " + file.Name));
            }
        }

        public static void ValidateFileName(VirtualFileSystem fileSystem, string name, string invalidNameMessage = null) {
            if (!fileSystem.IsNameValid(name)) {
                throw new UserMistakeException(invalidNameMessage ?? ("Invalid file name: " + name));
            }
        }
    }
}