using Stinkhorn.API;
using System.Linq;
using Stinkhorn.System.API;
using System.Collections.Generic;
using static Stinkhorn.System.API.SystemExtensions;
using System;

namespace Stinkhorn.System.Linux
{
    public class LinuxSystemFactory : IRequestHandlerFactory<InfoRequest>
    {
        public IRequestHandler<InfoRequest> CreateHandler()
            => new LinuxSystemHandler();

        public bool IsSuitable()
        {
            var platform = Environment.OSVersion.Platform;
            var env = Environment.GetEnvironmentVariables();
            var isLinux = env.Keys.OfType<string>().Any(key => key.Contains("_LINUX_"));
            return platform == PlatformID.Unix && isLinux;
        }
    }

    class LinuxSystemHandler : IRequestHandler<InfoRequest>
    {
        const string lsbRelease = "/usr/bin/lsb_release";

        IDictionary<string, string> LinuxReleaseDict => ReadFiles("/etc", "*release");

        public IResponse Process(InfoRequest input)
        {
            var info = (new SystemInfo()).PatchDefaults();
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