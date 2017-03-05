using System;
using RabbitMQ.Client;

namespace Stinkhorn.Common
{
    public class Unicast : Cached<Guid, Unicast>, IUnicast
    {
        public Guid Id { get; }

        Unicast(Guid id) { Id = id; }

        public override string ToString() => Id.ToString("N").ToLowerInvariant();

        public static Unicast Of(Guid id) => GetOrCreate(id);

        public string Type => ExchangeType.Direct;
        public string Exchange => ToString();
        public TransferKind Kind => TransferKind.Unicast;
    }
}