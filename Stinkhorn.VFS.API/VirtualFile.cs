using FubarDev.FtpServer.FileSystem;

namespace Stinkhorn.VFS.API
{
    class VirtualFile : VirtualEntry, IUnixFileEntry
    {
        public VirtualFile(VirtualFileSystem sys)
            : base(sys) { }

        public long Size { get; set; } = 0;
    }
}