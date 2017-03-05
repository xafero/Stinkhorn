using RabbitMQ.Client;

namespace Stinkhorn.Common
{
    public class Broadcast : Cached<bool, Broadcast>, IBroadcast
    {
        Broadcast(bool value) { }

        public override string ToString() => "*".ToLowerInvariant();

        public static Broadcast Of() => GetOrCreate(true);

        public string Type => ExchangeType.Fanout;
        public string Exchange => GetType().Namespace;
        public TransferKind Kind => TransferKind.Broadcast;
    }
}