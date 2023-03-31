using System.Collections.Generic;

namespace MCNBTViewer.Core.VFS {
    public abstract class VirtualFileSystem {
        /// <summary>
        /// Whether this file system has case-sensitive file names. For example, a windows
        /// NTFS does not have case sensitive names so this would return false
        /// </summary>
        public abstract bool IsCaseSensitive { get; }

        public abstract void DeleteFile(object requestor, VirtualFileBase file);
        public abstract void RenameFileTo(object requestor, VirtualFileBase file, string name);
        public abstract void MoveFileInto(object requestor, VirtualFileBase file, VirtualFolder target);
        public abstract VirtualFileBase CopyFileInto(object requestor, VirtualFileBase file, VirtualFolder target, string name);

        public abstract string GetFilePath(VirtualFileBase file);

        /// <summary>
        /// Finds a virtual file by its full path, or <see langword="null"/> if no such file exists
        /// </summary>
        /// <param name="path">The full path</param>
        /// <returns>The found file, or null if it doesn't exist</returns>
        public abstract VirtualFileBase FindFileByPath(string path);

        /// <summary>
        /// Returns the given file's parent file (which will be a directory), or null if it has no parent
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public abstract VirtualFolder GetParentFile(VirtualFileBase file);

        /// <summary>
        /// Tries to find a child file in the given folder, by the given name
        /// </summary>
        /// <param name="dir">The folder to search</param>
        /// <param name="name">The name of the file to search for</param>
        /// <param name="file">The found file, or null</param>
        /// <returns>Whether a file was found</returns>
        public abstract VirtualFileBase GetFile(VirtualFolder dir, string name);

        /// <summary>
        /// Returns an enumerable that will provide the folder's children
        /// </summary>
        public abstract IEnumerable<VirtualFileBase> GetChildren(VirtualFolder folder);

        /// <summary>
        /// A helper function for checking if two files' names are equal, taking into account <see cref="IsCaseSensitive"/>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public virtual bool AreFileNamesEqual(VirtualFileBase a, VirtualFileBase b) {
            if (this.IsCaseSensitive) {
                return a.Name.Equals(b.Name);
            }
            else {
                return a.Name.ToLower().Equals(b.Name.ToLower());
            }
        }

        /// <summary>
        /// Primarily checks if the name (for a <see cref="VirtualFileBase"/>) is valid. This should
        /// only really check for things like illegal characters
        /// </summary>
        /// <param name="name">The name to check (for null, illegal chars, etc)</param>
        /// <returns></returns>
        public abstract bool IsNameValid(string name);

        /// <summary>
        /// Returns a canonicalised file name. If the name is already valid (<see cref="IsNameValid"/>), then the name is unaffected
        /// </summary>
        public abstract string CanonicaliseFileName(string name, char replacement);

        /// <summary>
        /// Joins two file names into a single path. This is the same as the params version, but this exists for performance helpers
        /// </summary>
        public abstract string JoinFileNames(string a, string b);

        /// <summary>
        /// Joins file names into a full name
        /// </summary>
        public abstract string JoinFileNames(params string[] names);

        /// <summary>
        /// Creates a new virtual file in the given parent, or in this file system's root directory if parent == null
        /// </summary>
        /// <param name="dir">The target parent, or null for a root file</param>
        /// <param name="name">The name of the file</param>
        public abstract VirtualFile CreateVirtualFile(object requestor, VirtualFolder dir, string name);

        /// <summary>
        /// Creates a new virtual directory in the given parent, or in this file system's root directory if parent == null
        /// </summary>
        /// <param name="dir">The target parent, or null for a root file</param>
        /// <param name="name">The name of the directory</param>
        public abstract VirtualFolder CreateVirtualDirectory(object requestor, VirtualFolder dir, string name);

        protected void SetFileName(VirtualFile file, string name) {
            file.Name = name;
        }
    }
}