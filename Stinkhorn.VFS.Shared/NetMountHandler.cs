using Stinkhorn.API;
using Stinkhorn.VFS.API;

namespace Stinkhorn.VFS.Shared
{
    [ReqHandlerFilter]
    public class NetMountHandler : IRequestHandler<MountRequest>
    {
        public IResponse Process(MountRequest input)
            => new MountResponse();
    }
}