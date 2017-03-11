using Stinkhorn.API;
using System.Linq;
using Stinkhorn.System.API;
using static Stinkhorn.System.API.SystemExtensions;
using System;
using Mono.Addins;

namespace Stinkhorn.System.Mac
{
    [Extension]
    public class MacSystemFactory : IRequestHandlerFactory<InfoRequest>
    {
        public IRequestHandler<InfoRequest> CreateHandler()
            => new MacSystemHandler();

        public bool IsSuitable()
        {
            var platform = Environment.OSVersion.Platform;
            var env = Environment.GetEnvironmentVariables();
            var isMac = env.Keys.OfType<string>().Any(key => key.Contains("Apple_"));
            return platform == PlatformID.MacOSX
                || (platform == PlatformID.Unix && isMac);
        }
    }

    class MacSystemHandler : IRequestHandler<InfoRequest>
    {
        public IResponse Process(InfoRequest input)
        {
            var info = (new SystemInfo()).PatchDefaults();
            info.Platform = Platform.MacOSX;
            var text = ReadProcess("Unknown type", "serverinfo", "--software").Single();
            info.Type = text.Contains("NOT") ? OSType.Client : OSType.Server;
            text = ReadProcess("V:Unknown", "sw_vers", "-productVersion").Single();
            info.Release = text.Split(':').Last().Trim();
            text = ReadProcess("N:Unknown", "sw_vers", "-productName").Single();
            info.Product = text.Split(':').Last().Trim();
            text = ReadProcess("B:Unknown", "sw_vers", "-buildVersion").Single();
            info.Edition = text.Split(':').Last().Trim();
            return new InfoResponse { Result = info };
        }
    }
}