using System;
using FubarDev.FtpServer.FileSystem;
using FubarDev.FtpServer.FileSystem.Generic;

namespace Stinkhorn.VFS.API
{
    abstract class VirtualEntry : IUnixFileSystemEntry
    {
        VirtualFileSystem Parent { get; }

        protected VirtualEntry(VirtualFileSystem parent)
        {
            Parent = parent;
            var mode = new GenericAccessMode(true, false, false);
            Permissions = new GenericUnixPermissions(mode, mode, mode);
        }

        public IUnixFileSystem FileSystem => Parent;

        public DateTimeOffset? CreatedTime { get; }
            = DateTime.UtcNow;

        public DateTimeOffset? LastWriteTime { get; }
            = DateTime.UtcNow;

        public string Name { get; } = "defName";

        public string Group { get; } = "defGroup";

        public string Owner { get; } = "defOwner";

        public long NumberOfLinks { get; } = 0L;

        public IUnixPermissions Permissions { get; }
    }
}