﻿using System;
using FubarDev.FtpServer.FileSystem;
using FubarDev.FtpServer.FileSystem.Generic;

namespace Stinkhorn.VFS.API
{
    abstract class VirtualEntry : IUnixFileSystemEntry
    {
        protected VirtualFileSystem Parent { get; }
        protected VirtualDirectory Previous { get; }

        protected VirtualEntry(VirtualFileSystem parent,
            VirtualDirectory dir = null)
        {
            Parent = parent;
            Previous = dir;
            var mode = new GenericAccessMode(true, false, false);
            Permissions = new GenericUnixPermissions(mode, mode, mode);
        }

        public IUnixFileSystem FileSystem => Parent;

        public DateTimeOffset? CreatedTime { get; set; }
            = DateTime.UtcNow;

        public DateTimeOffset? LastWriteTime { get; set; }
            = DateTime.UtcNow;

        public virtual string Name { get; set; } = "someName";

        public string Group { get; set; } = "someGroup";

        public string Owner { get; set; } = "someOwner";

        public long NumberOfLinks { get; set; } = 0L;

        public IUnixPermissions Permissions { get; set; }

        public string Path => $"{Previous?.Path}/{Name}";
    }
}