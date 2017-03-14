using FubarDev.FtpServer.FileSystem;
using System.Collections.Generic;
using System.Linq;

namespace Stinkhorn.VFS.API
{
    class VirtualDirectory : VirtualEntry, IUnixDirectoryEntry
    {
        VirtualDirectory Previous { get; }

        public VirtualDirectory(VirtualFileSystem sys,
            VirtualDirectory dir = null)
            : base(sys)
        {
            Previous = dir;
        }

        public bool IsDeletable { get; } = false;

        public bool IsRoot => Previous == null;

        public IEnumerable<IUnixFileSystemEntry> Entries
            => IsRoot ? RootEntries : new IUnixFileSystemEntry[0];

        IEnumerable<IUnixFileSystemEntry> RootEntries
        {
            get
            {
                var roots = Parent.Parent.Parent.Parent.roots;
                return roots.Select(r => new VirtualDirectory(Parent) { Name = r });
            }
        }
    }
}