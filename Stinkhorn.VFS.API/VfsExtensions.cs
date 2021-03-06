﻿using FubarDev.FtpServer.FileSystem;
using System;

namespace Stinkhorn.VFS.API
{
    public static class VfsExtensions
    {
        internal static IUnixFileSystemEntry ToEntry(this IEntry entry, VirtualFileSystem sys, VirtualDirectory dir)
        {
            var file = entry as IFile;
            if (file != null)
                return file.ToFile(sys, dir);
            var folder = entry as IFolder;
            if (folder != null)
                return folder.ToFolder(sys, dir);
            return null;
        }

        internal static IUnixFileEntry ToFile(this IFile file, VirtualFileSystem sys, VirtualDirectory dir)
        {
            var vfile = new VirtualFile(sys, dir);
            vfile.Name = file.Name;
            vfile.Size = file.Size;
            return vfile;
        }

        internal static IUnixDirectoryEntry ToFolder(this IFolder folder, VirtualFileSystem sys, VirtualDirectory dir)
        {
            var vdir = new VirtualDirectory(sys, dir);
            vdir.Name = folder.Name;
            return vdir;
        }

        public static string ToFileName(this Exception error)
            => $"{error.GetType().Name}.err";
    }
}