using FubarDev.FtpServer.FileSystem;
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

        public IUnixDirectoryEntry Root { get; }

        public bool SupportsAppend { get; set; } = false;

        public bool SupportsNonEmptyDirectoryDelete { get; set; } = false;

        public Task<IReadOnlyList<IUnixFileSystemEntry>> GetEntriesAsync(IUnixDirectoryEntry dir, CancellationToken token)
        {
            var model = Parent.Parent.Parent.roots;
            var folder = (VirtualDirectory)dir;
            if (folder.IsRoot)
            {
                var dirs = model.Values;
                var items = dirs.Select(v => v.ToFolder(this, folder));
                var list = items.ToList().AsReadOnly();
                return Task.FromResult<IReadOnlyList<IUnixFileSystemEntry>>(list);
            }
            return Task.FromResult<IReadOnlyList<IUnixFileSystemEntry>>(null);
        }

        public Task<IUnixFileSystemEntry> GetEntryByNameAsync(IUnixDirectoryEntry dir, string name, CancellationToken token)
        {
            var model = Parent.Parent.Parent.roots;
            var folder = (VirtualDirectory)dir;
            if (folder.IsRoot)
            {
                var dirs = model.Values;
                var item = dirs.First(v => FileSystemEntryComparer.Compare(v.Name, name) == 0);
                return Task.FromResult<IUnixFileSystemEntry>(item.ToFolder(this, folder));
            }
            return Task.FromResult<IUnixFileSystemEntry>(null);
        }

        public Task<IBackgroundTransfer> AppendAsync(IUnixFileEntry file, long? startPos, Stream data, CancellationToken token)
            => Task.FromResult<IBackgroundTransfer>(null);

        public Task<IBackgroundTransfer> CreateAsync(IUnixDirectoryEntry dir, string name, Stream data, CancellationToken token)
            => Task.FromResult<IBackgroundTransfer>(null);

        public Task<IUnixDirectoryEntry> CreateDirectoryAsync(IUnixDirectoryEntry dir, string name, CancellationToken token)
            => Task.FromResult<IUnixDirectoryEntry>(null);

        public Task<IUnixFileSystemEntry> MoveAsync(IUnixDirectoryEntry parent, IUnixFileSystemEntry source, IUnixDirectoryEntry target, string file, CancellationToken token)
            => Task.FromResult<IUnixFileSystemEntry>(null);

        public Task<Stream> OpenReadAsync(IUnixFileEntry file, long startPos, CancellationToken token)
            => Task.FromResult<Stream>(null);

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