using System;
using FubarDev.FtpServer.FileSystem;

namespace Stinkhorn.VFS.API
{
    class VirtualFile : VirtualEntry, IUnixFileEntry
    {
        Func<byte[]> Bytes { get; }

        public VirtualFile(VirtualFileSystem sys)
            : base(sys) { }

        public long Size => Bytes?.Invoke()?.LongLength ?? 0;
    }
}