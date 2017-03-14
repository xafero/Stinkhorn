using System;
using FubarDev.FtpServer.FileSystem;

namespace Stinkhorn.VFS.API
{
    abstract class VirtualEntry : IUnixFileSystemEntry
    {
        VirtualFileSystem Parent { get; }

        protected VirtualEntry(VirtualFileSystem parent)
        {
            Parent = parent;
        }

        public IUnixFileSystem FileSystem => Parent;

        public DateTimeOffset? CreatedTime
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Group
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public DateTimeOffset? LastWriteTime
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public long NumberOfLinks
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Owner
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IUnixPermissions Permissions
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}