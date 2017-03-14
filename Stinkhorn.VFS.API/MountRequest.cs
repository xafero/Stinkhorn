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

    }
}