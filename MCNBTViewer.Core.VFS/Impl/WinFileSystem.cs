using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MCNBTViewer.Core.VFS.Exceptions;

namespace MCNBTViewer.Core.VFS.Impl {
    public class WinFileSystem : VirtualFileSystem {
        public static VirtualFileSystem Instance { get; } = new WinFileSystem();

        public override bool IsCaseSensitive => false;

        private WinFileSystem() {

        }

        public override void DeleteFile(object requestor, VirtualFileBase file) {
            if (file.Parent == null)
                throw new UserMistakeException("Cannot delete root folder");
            string path = file.Path;
            if (!File.Exists(path))
                throw new UserMistakeException($"File does not exist: {path}");
            try {
                File.Delete(path);
            }
            catch (Exception e) {
                throw new UserMistakeException($"Failed to delete '{path}': {e.Message}", e);
            }
        }

        public override void RenameFileTo(object requestor, VirtualFileBase file, string name) {
            if (!this.IsNameValid(name))
                throw new UserMistakeException($"Invalid file name: {name}");
            VirtualFolder parent = file.Parent;
            if (parent == null)
                throw new UserMistakeException("Cannot rename root folder");
            string parentPath = parent.Path;
            if (!Directory.Exists(parentPath))
                throw new UserMistakeException($"File's parent directory does not exist: {parentPath}");
            string path = file.Path;
            if (!File.Exists(path))
                throw new UserMistakeException($"File does not exist: {path}");
            string newPath = this.JoinFileNames(parentPath, name);
            if (File.Exists(newPath))
                throw new UserMistakeException($"File already exists: {newPath}");

            try {
                File.Move(path, newPath);
            }
            catch (Exception e) {
                throw new UserMistakeException($"Failed to delete '{path}': {e.Message}", e);
            }
        }

        public override void MoveFileInto(object requestor, VirtualFileBase file, VirtualFolder target) {
            throw new System.NotImplementedException();
        }

        public override VirtualFileBase CopyFileInto(object requestor, VirtualFileBase file, VirtualFolder target, string name) {
            throw new System.NotImplementedException();
        }

        public override string GetFilePath(VirtualFileBase file) {
            List<string> parts = new List<string>(10);
            do {
                parts.Add(file.Name);
            } while ((file = file.Parent) != null);
            parts.Reverse();
            return string.Join("\\", parts);
        }

        public override VirtualFileBase FindFileByPath(string path) {
            string name = Path.GetFileName(path);
            if (File.Exists(path)) {
                return new WinVirtualFile {Name = name};
            }
            else if (Directory.Exists(path)) {
                return new WinVirtualFolder {Name = name};
            }
            else {
                return null;
            }
        }

        public override VirtualFolder GetParentFile(VirtualFileBase file) {
            string path = Path.GetDirectoryName(file.Path);
            if (!Directory.Exists(path)) {
                throw new UserMistakeException($"Parent directory does not exist: {path}");
            }
            return new WinVirtualFolder() {Name = Path.GetFileName(path)};
        }

        public override VirtualFileBase GetFile(VirtualFolder dir, string name) {
            return this.FindFileByPath(this.JoinFileNames(dir.Path, name));
        }

        public override IEnumerable<VirtualFileBase> GetChildren(VirtualFolder folder) {
            string path = folder.Path;
            if (!Directory.Exists(path))
                throw new UserMistakeException($"Folder does not exist: {path}");
            return Directory.EnumerateDirectories(path).Select(this.FindFileByPath);
        }

        public override bool IsNameValid(string name) {
            char[] chars = Path.GetInvalidFileNameChars();
            foreach (char ch in name) {
                if (Array.IndexOf(chars, ch) != -1) {
                    return false;
                }
            }

            return true;
        }

        public override string CanonicaliseFileName(string name, char replacement) {
            if (this.IsNameValid(name)) {
                return name;
            }

            StringBuilder sb = new StringBuilder(name.Length);
            char[] chars = Path.GetInvalidFileNameChars();
            foreach (char ch in name)
                sb.Append(Array.IndexOf(chars, ch) == -1 ? ch : replacement);
            return sb.ToString();
        }

        public override string JoinFileNames(string a, string b) {
            return Path.Combine(a, b);
        }

        public override string JoinFileNames(params string[] names) {
            return Path.Combine(names);
        }

        public override VirtualFile CreateVirtualFile(object requestor, VirtualFolder dir, string name) {
            if (!this.IsNameValid(name))
                throw new UserMistakeException($"File name is invalid: {name}");
            string dirPath = dir.Path;
            if (!Directory.Exists(dirPath))
                throw new UserMistakeException($"Directory does not exist: {dirPath}");
            string path = this.JoinFileNames(dirPath, name);
            if (File.Exists(path))
                throw new UserMistakeException($"File already exists: {path}");
            try {
                File.Create(path);
            }
            catch (Exception e) {
                throw new IOException($"Failed to create file: {e.Message}", e);
            }

            return new WinVirtualFile() {Name = name};
        }

        public override VirtualFolder CreateVirtualDirectory(object requestor, VirtualFolder dir, string name) {
            if (!this.IsNameValid(name))
                throw new UserMistakeException($"File name is invalid: {name}");
            string dirPath = dir.Path;
            if (!Directory.Exists(dirPath))
                throw new UserMistakeException($"Directory does not exist: {dirPath}");
            string path = this.JoinFileNames(dirPath, name);
            if (Directory.Exists(path))
                throw new UserMistakeException($"File already exists: {path}");
            try {
                Directory.CreateDirectory(path);
            }
            catch (Exception e) {
                throw new IOException($"Failed to create file: {e.Message}", e);
            }

            return new WinVirtualFolder() {Name = name};
        }
    }
}