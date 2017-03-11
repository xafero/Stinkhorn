using Mono.Addins;
using Stinkhorn.API;

namespace Stinkhorn.System.API
{
    [Extension]
    public class InfoResponse : IResponse
    {
        public SystemInfo Result { get; set; }
    }
}