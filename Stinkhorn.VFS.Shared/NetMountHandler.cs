using Stinkhorn.API;
using Stinkhorn.VFS.API;
using System.IO;
using System.Linq;
using static System.Environment;

namespace Stinkhorn.VFS.Shared
{
    [ReqHandlerFilter]
    public class NetMountHandler : IRequestHandler<MountRequest>
    {
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
                var folder = GetFolderPath(specialFld, SpecialFolderOption.Create);
                dirs = Directory.GetDirectories(folder);
                files = Directory.GetFiles(folder);
            }
            return new MountResponse
            {
                Directories = dirs.Select(d => new VirtualDirectory()),
                Files = files.Select(f => new VirtualFile())
            };
        }
    }
}