using Stinkhorn.API;
using Stinkhorn.VFS.API;
using System.IO;
using System.Linq;

namespace Stinkhorn.VFS.Shared
{
    [ReqHandlerFilter]
    public class NetMountHandler : IRequestHandler<MountRequest>
    {
        public IResponse Process(MountRequest input)
        {
            var allDrives = DriveInfo.GetDrives();
            var drives = allDrives.Where(d => d.DriveType == DriveType.Fixed
                && d.IsReady).ToDictionary(k => k.RootDirectory.FullName,
                v => v.VolumeLabel);
            return new MountResponse { Drives = drives };
        }
    }
}