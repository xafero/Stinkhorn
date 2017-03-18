using FubarDev.FtpServer.FileSystem;

namespace Stinkhorn.VFS.API
{
    class VirtualFile : VirtualEntry, IUnixFileEntry
    {
        public VirtualFile(VirtualFileSystem sys,
            VirtualDirectory dir = null) : base(sys, dir) { }

        public long Size { get; set; } = 0L;
    }
}