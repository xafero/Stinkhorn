using System;
using System.Threading.Tasks;
using FubarDev.FtpServer.FileSystem;

namespace Stinkhorn.VFS.API
{
    class VirtualFsFactory : IFileSystemClassFactory
    {
        FileServer Parent { get; }

        public VirtualFsFactory(FileServer parent)
        {
            Parent = parent;
        }

        public Task<IUnixFileSystem> Create(string userId, bool isAnonymous)
        {
            if (string.IsNullOrWhiteSpace(userId) || isAnonymous)
                throw new NotSupportedException("Anonymous not allowed!");
            return Task.FromResult<IUnixFileSystem>(new VirtualFileSystem(this, userId));
        }
    }
}