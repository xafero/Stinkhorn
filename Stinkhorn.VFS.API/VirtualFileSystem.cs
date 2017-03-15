using FubarDev.FtpServer.FileSystem;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Diagnostics;
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

        public StringComparer FileSystemEntryComparer { get; }
            = StringComparer.InvariantCultureIgnoreCase;

        const StringComparison cmp
            = StringComparison.InvariantCultureIgnoreCase;

        public IUnixDirectoryEntry Root { get; }

        public bool SupportsAppend { get; } = false;

        public bool SupportsNonEmptyDirectoryDelete { get; } = false;

        public Task<IBackgroundTransfer> AppendAsync(IUnixFileEntry fileEntry, long? startPosition, Stream data, CancellationToken cancellationToken)
        {
            var file = (VirtualFile)fileEntry;
            return Task.FromResult(file.Append(startPosition, data));
        }

        public Task<IBackgroundTransfer> CreateAsync(IUnixDirectoryEntry targetDirectory, string fileName, Stream data, CancellationToken cancellationToken)
        {
            var dir = (VirtualDirectory)targetDirectory;
            return Task.FromResult(dir.Create(fileName, data));
        }

        public Task<IUnixDirectoryEntry> CreateDirectoryAsync(IUnixDirectoryEntry targetDirectory, string directoryName, CancellationToken cancellationToken)
        {
            var dir = (VirtualDirectory)targetDirectory;
            return Task.FromResult(dir.CreateDirectory(directoryName));
        }

        public Task<IReadOnlyList<IUnixFileSystemEntry>> GetEntriesAsync(IUnixDirectoryEntry directoryEntry, CancellationToken cancellationToken)
        {
            var dir = (VirtualDirectory)directoryEntry;
            IReadOnlyList<IUnixFileSystemEntry> entries = dir.Entries.ToList().AsReadOnly();
            return Task.FromResult(entries);
        }

        public Task<IUnixFileSystemEntry> GetEntryByNameAsync(IUnixDirectoryEntry directoryEntry, string name, CancellationToken cancellationToken)
        {
            var dir = (VirtualDirectory)directoryEntry;
            return Task.FromResult(dir.GetEntryByName(name));
        }

        public Task<IUnixFileSystemEntry> MoveAsync(IUnixDirectoryEntry parent, IUnixFileSystemEntry source, IUnixDirectoryEntry target, string fileName, CancellationToken cancellationToken)
        {
            Debugger.Break();
            throw new NotImplementedException();
        }

        public Task<Stream> OpenReadAsync(IUnixFileEntry fileEntry, long startPosition, CancellationToken cancellationToken)
        {
            var file = (VirtualFile)fileEntry;
            return Task.FromResult(file.OpenRead(startPosition));
        }

        public Task<IBackgroundTransfer> ReplaceAsync(IUnixFileEntry fileEntry, Stream data, CancellationToken cancellationToken)
        {
            Debugger.Break();
            throw new NotImplementedException();
        }

        public Task<IUnixFileSystemEntry> SetMacTimeAsync(IUnixFileSystemEntry entry, DateTimeOffset? modify, DateTimeOffset? access, DateTimeOffset? create, CancellationToken cancellationToken)
        {
            Debugger.Break();
            throw new NotImplementedException();
        }

        public Task UnlinkAsync(IUnixFileSystemEntry entry, CancellationToken cancellationToken)
        {
            Debugger.Break();
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }
    }
}