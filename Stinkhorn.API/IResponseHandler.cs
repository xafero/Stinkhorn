using Mono.Addins;

namespace Stinkhorn.API
{
    [TypeExtensionPoint]
    public interface IResponseHandler
    {
    }

    public interface IResponseHandler<I, S> : IResponseHandler
        where I : IResponse
    {
        ResponseStatus Process(S src, I msg);
    }

    public enum ResponseStatus
    {
        Handled,

        NotHandled,

        Failed
    }
}