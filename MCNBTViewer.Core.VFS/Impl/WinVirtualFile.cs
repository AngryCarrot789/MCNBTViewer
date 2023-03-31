namespace MCNBTViewer.Core.VFS.Impl {
    public class WinVirtualFile : VirtualFile {
        public override VirtualFileSystem FileSystem => WinFileSystem.Instance;

        public override string Name { get; internal set; }
    }
}