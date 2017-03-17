using FubarDev.FtpServer.FileSystem;

namespace Stinkhorn.VFS.API
{
    static class VfsExtensions
    {
        public static IUnixFileSystemEntry ToEntry(this IEntry entry, VirtualFileSystem sys, VirtualDirectory dir)
        {
            var file = entry as IFile;
            if (file != null)
                return file.ToFile(sys, dir);
            var folder = entry as IFolder;
            if (folder != null)
                return folder.ToFolder(sys, dir);
            return null;
        }

        public static IUnixFileEntry ToFile(this IFile file, VirtualFileSystem sys, VirtualDirectory dir)
        {
            var vfile = new VirtualFile(sys, dir);
            vfile.Name = file.Name;
            return vfile;
        }

        public static IUnixDirectoryEntry ToFolder(this IFolder folder, VirtualFileSystem sys, VirtualDirectory dir)
        {
            var vdir = new VirtualDirectory(sys, dir);
            vdir.Name = folder.Name;
            return vdir;
        }
    }
}