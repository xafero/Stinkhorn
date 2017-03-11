using Mono.Addins;

namespace Stinkhorn.API
{
    public interface IMessage
    {

    }

    [TypeExtensionPoint(ExtensionAttributeType = typeof(MessageAttribute))]
    public interface IRequest : IMessage
    {

    }

    [TypeExtensionPoint(ExtensionAttributeType = typeof(MessageAttribute))]
    public interface IResponse : IMessage
    {

    }

    public interface IMessageHandler<I, O>
        where I : IRequest
        where O : IResponse
    {
        O Process(I input);
    }

    public class MessageAttribute : CustomExtensionAttribute
    {
        [NodeAttribute]
        public string Category { get; set; }
    }
}