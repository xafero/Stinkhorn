using Stinkhorn.API;
using Stinkhorn.System.API;
using System.Linq;
using static Stinkhorn.System.API.SystemExtensions;

namespace Stinkhorn.System.BSD
{
    [ReqHandlerFilter(Platform = "Unix", HasVar = "SYSCTL_%")]
    public class BsdSystemHandler : IRequestHandler<InfoRequest>
    {
        public IResponse Process(InfoRequest input)
        {
            var info = PatchDefaults(new SystemInfo());
            info.Platform = Platform.BSD;
            var text = ReadProcess("Unknown type", "uname", "-i").Single();
            info.Type = text.Contains("GENERIC") ? OSType.Client : OSType.Server;
            text = ReadProcess("Unknown edition or else", "uname", "-v").Single();
            info.Edition = text.Split(' ')[3].Split('(').Last().TrimEnd(new[] { ')', ':' });
            info.Product = ReadProcess("Unknown product", "uname", "-sr").Single();
            info.Release = ReadProcess("Unknown release", "uname", "-U").Single();
            return new InfoResponse { Result = info };
        }
    }
}