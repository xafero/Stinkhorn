using System.IO;
using System.Linq;
using static System.Environment;
using Stinkhorn.API;
using Stinkhorn.VFS.API;
using System;

namespace Stinkhorn.VFS.Shared
{
    static class NetExtensions
    {
        public static string FindRealPath(string source, string path,
            SpecialFolderOption opt)
        {
            var specialFld = source.TryEnum<SpecialFolder>();
            if (!specialFld.HasValue)
                return null;
            string root;
            var relative = path.TrimStart('/');
            if (specialFld == SpecialFolder.MyComputer)
            {
                var part = relative.Split(new[] { '/' }, 2);
                var driveName = part.First();
                var drive = DriveInfo.GetDrives().First(
                    d => PatchLabel(d.VolumeLabel) == driveName);
                relative = part.Length < 2 ? "" : part.Last();
                root = drive.RootDirectory.FullName;
            }
            else
            {
                root = GetFolderPath(specialFld.Value, opt);
            }
            return Path.Combine(root, relative);
        }

        public static IFile GetFileInfo(string path)
        {
            var file = new VfsFile(path);
            var info = new FileInfo(path);
            file.Size = info.Length;
            return file;
        }

        public static string PatchLabel(string label)
            => label == "/" ? "Root" : label.TrimStart('/')
            .Replace('/', '_').Replace('.', '-');

        public static bool DriveFilter(DriveInfo di)
            => di.IsReady &&
                di.DriveType != DriveType.Network &&
                di.DriveType != DriveType.NoRootDirectory;
    }
}