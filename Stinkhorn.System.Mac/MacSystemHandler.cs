using Stinkhorn.API;
using System.Linq;
using Stinkhorn.System.API;
using static Stinkhorn.System.API.SystemExtensions;

namespace Stinkhorn.System.Mac
{
    [ReqHandlerFilter(Platform = "Unix", HasVar = "Apple_%")]
    [ReqHandlerFilter(Platform = "MacOSX")]
    public class MacSystemHandler : IRequestHandler<InfoRequest>
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