using System;
using FubarDev.FtpServer.FileSystem;
using System.Diagnostics;

namespace Stinkhorn.VFS.API
{
    class VirtualFile : VirtualEntry, IUnixFileEntry
    {
        public VirtualFile(VirtualFileSystem sys)
            : base(sys) { }

        public long Size
        {
            get
            {
                Debugger.Break(); throw new NotImplementedException();
            }
        }
    }
}