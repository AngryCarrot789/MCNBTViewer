using System.Collections.Generic;

namespace MCNBTViewer.Core.VFS.Impl {
    public class WinVirtualFolder : VirtualFolder {
        public override VirtualFileSystem FileSystem => WinFileSystem.Instance;

        public override string Name { get; internal set; }

        public override IEnumerable<VirtualFileBase> Children => this.FileSystem.GetChildren(this);
    }
}