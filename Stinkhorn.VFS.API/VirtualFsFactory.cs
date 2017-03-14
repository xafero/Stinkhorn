using System;
using System.Threading.Tasks;
using FubarDev.FtpServer.FileSystem;

namespace Stinkhorn.VFS.API
{
    class VirtualFsFactory : IFileSystemClassFactory
    {
        public Task<IUnixFileSystem> Create(string userId, bool isAnonymous)
        {
            throw new NotImplementedException();
        }
    }
}