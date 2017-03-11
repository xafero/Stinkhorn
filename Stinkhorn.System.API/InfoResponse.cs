using Stinkhorn.API;

namespace Stinkhorn.System.API
{
    public class InfoResponse : IResponse
    {
        public SystemInfo Result { get; set; }
    }
}