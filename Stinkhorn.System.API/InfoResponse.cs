using Stinkhorn.API;

namespace Stinkhorn.System.API
{
    [ResponseDesc]
    public class InfoResponse : IResponse
    {
        public SystemInfo Result { get; set; }
    }
}