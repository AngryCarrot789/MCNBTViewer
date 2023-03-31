using MCNBTViewer.Core.VFS.Data;

namespace MCNBTViewer.Core.VFS {
    public abstract class VirtualFileBase : VFDataHolderMap {
        /// <summary>
        /// Gets this file's associated file system
        /// </summary>
        public abstract VirtualFileSystem FileSystem { get; }

        /// <summary>
        /// The name of this file
        /// </summary>
        public abstract string Name { get; internal set; }

        /// <summary>
        /// This virtual file's pull path, where calling <see cref="VirtualFileSystem.FindFileByPath(string)"/> with
        /// this path will return the current instance (as long as this file is valid)
        /// </summary>
        public virtual string Path => this.FileSystem.GetFilePath(this);

        /// <summary>
        /// Whether this file is valid or not
        /// </summary>
        public virtual bool IsValid { get; set; }

        /// <summary>
        /// Whether this file exists or not. This should take into account <see cref="IsValid"/>
        /// </summary>
        public virtual bool Exists => this.IsValid;

        /// <summary>
        /// This virtual file's parent directory, or null if this file is a root file
        /// </summary>
        public virtual VirtualFolder Parent => this.FileSystem.GetParentFile(this);

        protected VirtualFileBase() {

        }

        // These delegate the calls because why not

        public virtual void Delete(object requestor) {
            this.FileSystem.DeleteFile(requestor, this);
        }

        public virtual void RenameTo(object requestor, string name) {
            this.FileSystem.RenameFileTo(requestor, this, name);
        }

        public virtual void MoveInto(object requestor, VirtualFolder target) {
            this.FileSystem.MoveFileInto(requestor, this, target);
        }

        public virtual VirtualFileBase CopyInto(object requestor, VirtualFolder target) {
            return this.FileSystem.CopyFileInto(requestor, this, target, this.Name);
        }

        public virtual VirtualFileBase CopyInto(object requestor, VirtualFolder target, string copiedName) {
            return this.FileSystem.CopyFileInto(requestor, this, target, copiedName);
        }
    }
}