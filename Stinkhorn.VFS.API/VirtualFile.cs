using System;
using System.IO;
using FubarDev.FtpServer.FileSystem;

namespace Stinkhorn.VFS.API
{
    class VirtualFile : VirtualEntry, IUnixFileEntry
    {
        Func<byte[]> Bytes { get; }

        public VirtualFile(VirtualFileSystem sys)
            : base(sys) { }

        public long Size => Bytes?.Invoke()?.LongLength ?? 0;

        internal IBackgroundTransfer Append(long? startPosition, Stream data)
        {
            throw new NotImplementedException();
        }

        internal Stream OpenRead(long startPosition)
        {
            throw new NotImplementedException();
        }
    }
}