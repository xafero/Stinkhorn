using FubarDev.FtpServer.FileSystem;
using System;
using System.Diagnostics;

namespace Stinkhorn.VFS.API
{
    class VirtualDirectory : VirtualEntry, IUnixDirectoryEntry
    {
        public VirtualDirectory(VirtualFileSystem sys)
            : base(sys) { }

        public bool IsDeletable
        {
            get
            {
                Debugger.Break(); throw new NotImplementedException();
            }
        }

        public bool IsRoot
        {
            get
            {
                Debugger.Break(); throw new NotImplementedException();
            }
        }
    }
}