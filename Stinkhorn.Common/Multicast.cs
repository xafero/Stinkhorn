using RabbitMQ.Client;

namespace Stinkhorn.Common
{
    public class Multicast : Cached<string, Multicast>, IMulticast
    {
        public string Topic { get; }

        Multicast(string topic) { Topic = topic; }

        public override string ToString() => Topic.Trim().ToLowerInvariant();

        public static Multicast Of<T>() => Of(typeof(T).FullName);

        public static Multicast Of(string topic) => GetOrCreate(topic);

        public string Type => ExchangeType.Topic;
        public string Exchange => ToString();
        public TransferKind Kind => TransferKind.Multicast;
    }
}