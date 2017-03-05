using RabbitMQ.Client;
using System;

namespace Stinkhorn.Common
{
    public class Broadcast : Cached<bool, Broadcast>, IBroadcast
    {
        Broadcast(bool value) { }

        public override string ToString() => "*".ToLowerInvariant();

        public static Broadcast Of() => GetOrCreate(true);

        public string Type => ExchangeType.Fanout;
        public string Exchange => GetType().Namespace;
    }

    public class Multicast : Cached<string, Multicast>, IMulticast
    {
        public string Topic { get; }

        Multicast(string topic) { Topic = topic; }

        public override string ToString() => Topic.Trim().ToLowerInvariant();

        public static Multicast Of<T>() => Of(typeof(T).FullName);

        public static Multicast Of(string topic) => GetOrCreate(topic);

        public string Type => ExchangeType.Topic;
        public string Exchange => ToString();
    }

    public class Unicast : Cached<Guid, Unicast>, IUnicast
    {
        public Guid Id { get; }

        Unicast(Guid id) { Id = id; }

        public override string ToString() => Id.ToString("N").ToLowerInvariant();

        public static Unicast Of(Guid id) => GetOrCreate(id);

        public string Type => ExchangeType.Direct;
        public string Exchange => ToString();
    }
}