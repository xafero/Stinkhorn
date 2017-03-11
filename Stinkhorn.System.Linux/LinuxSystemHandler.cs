using Stinkhorn.API;
using System.Linq;
using Stinkhorn.System.API;
using System.Collections.Generic;
using static Stinkhorn.System.API.SystemExtensions;

namespace Stinkhorn.System.Linux
{
    [ReqHandlerFilter(Platform = "Unix", HasVar = "%_LINUX_%")]
    public class LinuxSystemHandler : IRequestHandler<InfoRequest>
    {
        const string lsbRelease = "/usr/bin/lsb_release";

        IDictionary<string, string> LinuxReleaseDict => ReadFiles("/etc", "*release");

        public IResponse Process(InfoRequest input)
        {
            var info = PatchDefaults(new SystemInfo());
            info.Platform = Platform.Linux;
            var text = ReadProcess("Unknown type", "uname", "-r").Single();
            info.Type = text.Contains("generic") ? OSType.Client : OSType.Server;
            text = ReadProcess("C:Unknown edition", lsbRelease, "-c").Single();
            info.Edition = text.Split(':').Last().Trim();
            text = ReadProcess("D:Unknown product", lsbRelease, "-d").Single();
            info.Product = text.Split(':').Last().Trim();
            text = ReadProcess("R:Unknown release", lsbRelease, "-r").Single();
            info.Release = text.Split(':').Last().Trim();
            return new InfoResponse { Result = info };
        }
    }
}