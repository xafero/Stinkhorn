﻿using System.IO;
using System.Linq;
using static System.Environment;
using Stinkhorn.API;

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
                    d => d.VolumeLabel == driveName);
                relative = part.Length < 2 ? "" : part.Last();
                root = drive.RootDirectory.FullName;
            }
            else
            {
                root = GetFolderPath(specialFld.Value, opt);
            }
            return Path.Combine(root, relative);
        }
    }
}