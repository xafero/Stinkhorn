using FubarDev.FtpServer.FileSystem;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace Stinkhorn.VFS.API
{
    class VirtualFileSystem : IUnixFileSystem
    {
        string UserId { get; }
        VirtualFsFactory Parent { get; }

        public VirtualFileSystem(VirtualFsFactory parent, string userId)
        {
            Parent = parent;
            UserId = userId;
        }

        public StringComparer FileSystemEntryComparer
        {
            get
            {
                Debugger.Break(); throw new NotImplementedException();
            }
        }

        public IUnixDirectoryEntry Root
        {
            get
            {
                Debugger.Break(); throw new NotImplementedException();
            }
        }

        public bool SupportsAppend
        {
            get
            {
                Debugger.Break(); throw new NotImplementedException();
            }
        }

        public bool SupportsNonEmptyDirectoryDelete
        {
            get
            {
                Debugger.Break(); throw new NotImplementedException();
            }
        }

        public Task<IBackgroundTransfer> AppendAsync(IUnixFileEntry fileEntry, long? startPosition, Stream data, CancellationToken cancellationToken)
        {
            Debugger.Break(); throw new NotImplementedException();
        }

        public Task<IBackgroundTransfer> CreateAsync(IUnixDirectoryEntry targetDirectory, string fileName, Stream data, CancellationToken cancellationToken)
        {
            Debugger.Break(); throw new NotImplementedException();
        }

        public Task<IUnixDirectoryEntry> CreateDirectoryAsync(IUnixDirectoryEntry targetDirectory, string directoryName, CancellationToken cancellationToken)
        {
            Debugger.Break(); throw new NotImplementedException();
        }

        public void Dispose()
        {
            Debugger.Break(); throw new NotImplementedException();
        }

        public Task<IReadOnlyList<IUnixFileSystemEntry>> GetEntriesAsync(IUnixDirectoryEntry directoryEntry, CancellationToken cancellationToken)
        {
            Debugger.Break(); throw new NotImplementedException();
        }

        public Task<IUnixFileSystemEntry> GetEntryByNameAsync(IUnixDirectoryEntry directoryEntry, string name, CancellationToken cancellationToken)
        {
            Debugger.Break(); throw new NotImplementedException();
        }

        public Task<IUnixFileSystemEntry> MoveAsync(IUnixDirectoryEntry parent, IUnixFileSystemEntry source, IUnixDirectoryEntry target, string fileName, CancellationToken cancellationToken)
        {
            Debugger.Break(); throw new NotImplementedException();
        }

        public Task<Stream> OpenReadAsync(IUnixFileEntry fileEntry, long startPosition, CancellationToken cancellationToken)
        {
            Debugger.Break(); throw new NotImplementedException();
        }

        public Task<IBackgroundTransfer> ReplaceAsync(IUnixFileEntry fileEntry, Stream data, CancellationToken cancellationToken)
        {
            Debugger.Break(); throw new NotImplementedException();
        }

        public Task<IUnixFileSystemEntry> SetMacTimeAsync(IUnixFileSystemEntry entry, DateTimeOffset? modify, DateTimeOffset? access, DateTimeOffset? create, CancellationToken cancellationToken)
        {
            Debugger.Break(); throw new NotImplementedException();
        }

        public Task UnlinkAsync(IUnixFileSystemEntry entry, CancellationToken cancellationToken)
        {
            Debugger.Break(); throw new NotImplementedException();
        }
    }
}