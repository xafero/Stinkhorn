using FubarDev.FtpServer.FileSystem;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using System.IO;

namespace Stinkhorn.VFS.API
{
    class VirtualDirectory : VirtualEntry, IUnixDirectoryEntry
    {
        const StringComparison cmp =
            StringComparison.InvariantCultureIgnoreCase;

        VirtualDirectory Previous { get; }

        public VirtualDirectory(VirtualFileSystem sys,
            VirtualDirectory dir = null)
            : base(sys)
        {
            Previous = dir;
        }

        public bool IsDeletable { get; } = false;

        public bool IsRoot => Previous == null;

        Func<IEnumerable<IUnixFileSystemEntry>> EntriesFunc { get; set; }

        public IEnumerable<IUnixFileSystemEntry> Entries => IsRoot ? RootEntries
            : EntriesFunc?.Invoke() ?? new IUnixFileSystemEntry[0];

        IEnumerable<IUnixFileSystemEntry> RootEntries
        {
            get
            {
                var roots = Parent.Parent.Parent.Parent.roots;
                return roots.Select(r =>
                    new VirtualDirectory(Parent, this)
                    {
                        Name = r.Key,
                        EntriesFunc = () =>
                        {
                            var items = r.Value as IEnumerable;
                            return items.OfType<KeyValuePair<string,string>>()
                            .Select(i => new VirtualDirectory(Parent, this)
                            { Name = i.Value });
                        }
                    });
            }
        }

        internal IBackgroundTransfer Create(string fileName, Stream data)
        {
            throw new NotImplementedException();
        }

        internal IUnixDirectoryEntry CreateDirectory(string directoryName)
        {
            throw new NotImplementedException();
        }

        internal IUnixFileSystemEntry GetEntryByName(string name)
            => Entries.FirstOrDefault(e => e.Name.Equals(name, cmp));
    }
}