
namespace Stinkhorn.API
{
    public class InfoRequest<T> : IRequest
    {
    }

    public class InfoResponse<T> : IResponse
    {
        public T Result { get; set; }
    }
}