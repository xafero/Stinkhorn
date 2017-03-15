using FubarDev.FtpServer.FileSystem;

namespace Stinkhorn.VFS.API
{
    class VirtualDirectory : VirtualEntry, IUnixDirectoryEntry
    {
        VirtualDirectory Previous { get; }

        public VirtualDirectory(VirtualFileSystem sys,
            VirtualDirectory dir = null)
            : base(sys)
        {
            Previous = dir;
        }

        public bool IsDeletable { get; set; } = false;

        public bool IsRoot => Previous == null;

        public string Path => $"{Previous?.Name}/{Name}";
    }
}