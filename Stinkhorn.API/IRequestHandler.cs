using Mono.Addins;

namespace Stinkhorn.API
{
    public class PlatformAttribute : CustomExtensionAttribute
    {
        [NodeAttribute]
        public string Platform { get; set; }
        [NodeAttribute]
        public string Version { get; set; }
        [NodeAttribute]
        public string Arch { get; set; }
        [NodeAttribute]
        public string Locale { get; set; }
    }

    [TypeExtensionPoint(ExtensionAttributeType = typeof(PlatformAttribute))]
    public interface IRequestHandler
    {

    }

    [TypeExtensionPoint(ExtensionAttributeType = typeof(PlatformAttribute))]
    public interface IRequestHandler<I> : IRequestHandler
        where I : IRequest
    {
        IResponse Process(I input);
    }

    [TypeExtensionPoint(ExtensionAttributeType = typeof(PlatformAttribute))]
    public interface IRequestHandlerFactory
    {

    }

    [TypeExtensionPoint(ExtensionAttributeType = typeof(PlatformAttribute))]
    public interface IRequestHandlerFactory<I> : IRequestHandlerFactory
        where I : IRequest
    {
        bool IsSuitable();

        IRequestHandler<I> CreateHandler();
    }
}