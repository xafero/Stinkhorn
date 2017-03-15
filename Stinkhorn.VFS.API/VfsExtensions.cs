using FubarDev.FtpServer.FileSystem;

namespace Stinkhorn.VFS.API
{
    static class VfsExtensions
    {
        public static IUnixDirectoryEntry ToFolder(this IFolder folder, VirtualFileSystem sys, VirtualDirectory dir)
        {
            var vdir = new VirtualDirectory(sys, dir);
            vdir.Name = folder.Name;
            return vdir;
        }
    }
}