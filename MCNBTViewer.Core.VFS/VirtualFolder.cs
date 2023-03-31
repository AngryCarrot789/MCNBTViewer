using System.Collections.Generic;

namespace MCNBTViewer.Core.VFS {
    public abstract class VirtualFolder : VirtualFileBase {
        /// <summary>
        /// Returns an enumerable that will provide this folder's children
        /// </summary>
        public abstract IEnumerable<VirtualFileBase> Children { get; }

        protected VirtualFolder() {

        }

        public virtual void Delete(object requestor, VirtualFileBase file) {
            this.FileSystem.DeleteFile(requestor, file);
        }

        public virtual VirtualFileBase GetChildByName(string name) {
            return this.FileSystem.GetFile(this, name);
        }

        /// <summary>
        /// Creates a new virtual file in the given parent, or in this file system's root directory if parent == null
        /// </summary>
        /// <param name="requestor">The instance that requested this action</param>
        /// <param name="name">The name of the file</param>
        /// <param name="parent">The target parent, or null for a root file</param>
        public virtual VirtualFile CreateVirtualFile(object requestor, string name) {
            return this.FileSystem.CreateVirtualFile(requestor, this, name);
        }

        /// <summary>
        /// Creates a new virtual directory in the given parent, or in this file system's root directory if parent == null
        /// </summary>
        /// <param name="requestor">The instance that requested this action</param>
        /// <param name="name">The name of the directory</param>
        public virtual VirtualFolder CreateVirtualDirectory(object requestor, string name) {
            return this.FileSystem.CreateVirtualDirectory(requestor, this, name);
        }
    }
}