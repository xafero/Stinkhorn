namespace Stinkhorn.API
{
    public interface IRequestHandler<I> where I : IRequest
    {
        IResponse Process(I input);
    }

    public interface IRequestHandlerFactory<I> where I : IRequest
    {
        bool IsSuitable();

        IRequestHandler<I> CreateHandler();
    }
}