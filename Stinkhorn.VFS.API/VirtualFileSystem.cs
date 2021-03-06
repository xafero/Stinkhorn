﻿using FubarDev.FtpServer.FileSystem;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Linq;

namespace Stinkhorn.VFS.API
{
    class VirtualFileSystem : IUnixFileSystem
    {
        string UserId { get; }
        public VirtualFsFactory Parent { get; }

        public VirtualFileSystem(VirtualFsFactory parent, string userId)
        {
            Parent = parent;
            UserId = userId;
            Root = new VirtualDirectory(this);
        }

        public StringComparer FileSystemEntryComparer { get; set; }
            = StringComparer.InvariantCultureIgnoreCase;

        public StringComparison Cmp { get; set; }
            = StringComparison.InvariantCultureIgnoreCase;

        public IUnixDirectoryEntry Root { get; }

        public bool SupportsAppend { get; set; } = false;

        public bool SupportsNonEmptyDirectoryDelete { get; set; } = false;

        public Task<IReadOnlyList<IUnixFileSystemEntry>> GetEntriesAsync(IUnixDirectoryEntry dir, CancellationToken token)
        {
            var folder = (VirtualDirectory)dir;
            var vfs = Parent.Parent.Parent;
            var model = vfs.GetFolder(folder.Path, false) as VfsFolder;
            var entries = GetEntries(folder, model);
            if (entries.Count < 1 /* TODO: || Date check */)
            {
                Guid id;
                string arg, relative;
                DetermineArgs(vfs, model, folder, out id, out arg, out relative);
                vfs.Refresh(id, arg, relative);
                Thread.Sleep(3 * 100);
                entries = GetEntries(folder, model);
            }
            return Task.FromResult(entries);
        }

        IReadOnlyList<IUnixFileSystemEntry> GetEntries(
            VirtualDirectory folder, VfsFolder model)
            => model.Folders.OfType<IEntry>().Concat(model.Files)
            .Select(v => v.ToEntry(this, folder))
            .ToList().AsReadOnly();

        public Task<IUnixFileSystemEntry> GetEntryByNameAsync(IUnixDirectoryEntry dir, string name, CancellationToken token)
        {
            var folder = (VirtualDirectory)dir;
            var vfs = Parent.Parent.Parent;
            var model = vfs.GetFolder(folder.Path, false);
            var entry = model[name];
            return Task.FromResult(entry.ToEntry(this, folder));
        }

        public Task<Stream> OpenReadAsync(IUnixFileEntry file, long startPos, CancellationToken token)
        {
            var entity = (VirtualFile)file;
            var vfs = Parent.Parent.Parent;
            var folder = entity.Path.Replace(entity.Name, "");
            var model = vfs.GetFolder(folder, false) as VfsFolder;
            var entry = model[entity.Name] as IFile;
            Guid id;
            string arg, relative;
            DetermineArgs(vfs, model, entity, out id, out arg, out relative);
            ReadFileChunk rfc = (a, b, c, d) =>
                vfs.ReadFile(id, arg, relative, a, b, c, d);
            var str = new VirtualStream(entry, startPos, rfc);
            return Task.FromResult<Stream>(str);
        }

        void DetermineArgs(MountHandler vfs, VfsEntry model, VirtualEntry entry,
            out Guid id, out string arg, out string relative)
        {
            string serverRef;
            var mountPoint = vfs.GetMountPath(model, out serverRef);
            var absolute = entry.Path;
            var index = absolute.IndexOf(mountPoint, Cmp);
            relative = absolute.Substring(index + mountPoint.Length);
            var tmp = serverRef.Split('@');
            id = Guid.Parse(tmp.First());
            arg = tmp.Last();
        }

        public Task<IBackgroundTransfer> AppendAsync(IUnixFileEntry file, long? startPos, Stream data, CancellationToken token)
                => Task.FromResult<IBackgroundTransfer>(null);

        public Task<IBackgroundTransfer> CreateAsync(IUnixDirectoryEntry dir, string name, Stream data, CancellationToken token)
            => Task.FromResult<IBackgroundTransfer>(null);

        public Task<IUnixDirectoryEntry> CreateDirectoryAsync(IUnixDirectoryEntry dir, string name, CancellationToken token)
            => Task.FromResult<IUnixDirectoryEntry>(null);

        public Task<IUnixFileSystemEntry> MoveAsync(IUnixDirectoryEntry parent, IUnixFileSystemEntry source, IUnixDirectoryEntry target, string file, CancellationToken token)
            => Task.FromResult<IUnixFileSystemEntry>(null);

        public Task<IBackgroundTransfer> ReplaceAsync(IUnixFileEntry file, Stream data, CancellationToken token)
            => Task.FromResult<IBackgroundTransfer>(null);

        public Task<IUnixFileSystemEntry> SetMacTimeAsync(IUnixFileSystemEntry entry, DateTimeOffset? modify, DateTimeOffset? access, DateTimeOffset? create, CancellationToken token)
            => Task.FromResult<IUnixFileSystemEntry>(null);

        public Task UnlinkAsync(IUnixFileSystemEntry entry, CancellationToken token)
            => Task.Run(() => entry);

        public void Dispose()
        {
        }
    }
}