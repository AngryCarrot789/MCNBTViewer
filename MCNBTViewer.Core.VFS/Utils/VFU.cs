using System;

namespace MCNBTViewer.Core.VFS.Utils {
    public static class VFUtils {
        // These could be moved into their own classes to support I18N

        public static void ValidateIsValidFile(this VirtualFileBase file, string isNotValidMessage = null) {
            if (!file.IsValid) {
                throw new InvalidOperationException(isNotValidMessage ?? ("File is invalid: " + file.Name));
            }
        }

        public static void ValidateFileSystems(this VirtualFileBase file, VirtualFileSystem system, string isNotDirectoryMessage = null) {
            if (system != file.FileSystem) {
                throw new InvalidOperationException(isNotDirectoryMessage ?? ("File systems do not match: " + file.Name));
            }
        }

        public static void ValidateFileName(VirtualFileSystem fileSystem, string name, string invalidNameMessage = null) {
            if (!fileSystem.IsNameValid(name)) {
                throw new InvalidOperationException(invalidNameMessage ?? ("Invalid file name: " + name));
            }
        }
    }
}