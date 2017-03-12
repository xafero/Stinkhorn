using Mono.Addins;

namespace Stinkhorn.API
{
    public interface IMessage
    {

    }

    [TypeExtensionPoint(ExtensionAttributeType = typeof(RequestDescAttribute))]
    public interface IRequest : IMessage
    {
    }

    public class RequestDescAttribute : CustomExtensionAttribute
    {
        [NodeAttribute]
        public string Category { get; set; }
    }

    [TypeExtensionPoint(ExtensionAttributeType = typeof(ResponseDescAttribute))]
    public interface IResponse : IMessage
    {
    }

    public class ResponseDescAttribute : CustomExtensionAttribute
    {
    }
}