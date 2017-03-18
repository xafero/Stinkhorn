using Stinkhorn.API;
using Stinkhorn.VFS.API;
using System.IO;
using System.Linq;
using static System.Environment;
using System;

namespace Stinkhorn.VFS.Shared
{
    [ReqHandlerFilter]
    public class NetMountHandler : IRequestHandler<MountRequest>,
        IRequestHandler<ListRequest>
    {
        const SpecialFolderOption opt = SpecialFolderOption.Create;

        public IResponse Process(ListRequest input)
        {
            var specialFld = input.Source.TryEnum<SpecialFolder>();
            if (!specialFld.HasValue)
                return null;
            string root;
            var relative = input.Path.TrimStart('/');
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
            var path = Path.Combine(root, relative);
            string[] dirs;
            string[] files;
            FetchEntries(path, out dirs, out files);
            return new ListResponse
            {
                Folders = dirs.Select(d => new VfsFolder(d)).ToArray(),
                Files = files.Select(f => new VfsFile(f)).ToArray()
            };
        }

        public IResponse Process(MountRequest input)
        {
            var specialFld = input.Source;
            string[] dirs;
            string[] files;
            if (specialFld == SpecialFolder.MyComputer)
            {
                var allDrives = DriveInfo.GetDrives();
                var drives = allDrives.Where(d => d.DriveType == DriveType.Fixed
                        && d.IsReady).ToDictionary(k => k.RootDirectory.FullName,
                        v => v.VolumeLabel);
                dirs = drives.Values.ToArray();
                files = new string[0];
            }
            else
            {
                var folder = GetFolderPath(specialFld, opt);
                FetchEntries(folder, out dirs, out files);
            }
            return new MountResponse
            {
                Folders = dirs.Select(d => new VfsFolder(d)).ToArray(),
                Files = files.Select(f => new VfsFile(f)).ToArray(),
                Source = input.Source + string.Empty,
                Target = input.Target
            };
        }

        void FetchEntries(string folder, out string[] dirs, out string[] files)
        {
            dirs = Directory.GetDirectories(folder);
            files = Directory.GetFiles(folder);
        }
    }
}