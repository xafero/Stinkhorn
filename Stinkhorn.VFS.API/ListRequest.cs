using Stinkhorn.API;

namespace Stinkhorn.VFS.API
{
    [RequestDesc(Category = nameof(VFS))]
    public class ListRequest : IRequest
    {

    }

    [ResponseDesc]
    public class ListResponse : IResponse
    {

    }
}