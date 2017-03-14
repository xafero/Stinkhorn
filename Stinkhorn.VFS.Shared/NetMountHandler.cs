using System;
using Stinkhorn.API;
using Stinkhorn.VFS.API;
using System.Diagnostics;

namespace Stinkhorn.VFS.Shared
{
    [ReqHandlerFilter]
    public class NetMountHandler : IRequestHandler<MountRequest>
    {
        public IResponse Process(MountRequest input)
        {
            Debugger.Break(); throw new NotImplementedException();
        }
    }
}