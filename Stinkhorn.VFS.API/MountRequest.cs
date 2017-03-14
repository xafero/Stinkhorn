using System.Collections.Generic;
using Stinkhorn.API;

namespace Stinkhorn.VFS.API
{
    [RequestDesc(Category = nameof(VFS))]
    public class MountRequest : IRequest
    {

    }

    [ResponseDesc]
    public class MountResponse : IResponse
    {
        public IDictionary<string, string> Drives { get; set; }
    }
}