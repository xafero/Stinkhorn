using FubarDev.FtpServer.FileSystem;

namespace Stinkhorn.VFS.API
{
    class VirtualDirectory : VirtualEntry, IUnixDirectoryEntry
    {
        public VirtualDirectory(VirtualFileSystem sys,
            VirtualDirectory dir = null) : base(sys, dir) { }

        public bool IsDeletable { get; set; } = false;

        public bool IsRoot => Previous == null;

        public override string Name => IsRoot ? "" : base.Name;
    }
}