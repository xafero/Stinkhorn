using System;
using System.Collections.Generic;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Stinkhorn.Common
{
    class Fault : IFault
    {
        public FaultCode Code { get; }
        public string Description { get; }
        public string Details { get; }

        internal Fault(FaultCode code, string description, string details)
        {
            Code = code;
            Description = description;
            Details = details;
        }
    }

    interface IBrokerParticipant : IParticipant
    {
        bool Mandatory { get; }
        string Type { get; }
        string Exchange { get; }
        string Route { get; }
        string Queue { get; set; }
        string Tag { get; set; }
        int Number { get; }
        string Name { get; }
        AmqpTcpEndpoint Endpoint { get; }
        int LocalPort { get; }
        int RemotePort { get; }
    }

    public static class InternalExts
    {
        internal static IDictionary<string, object> ToDict(this IBrokerParticipant p, Lazy<Guid> id)
            => new Dictionary<string, object> {
                { "Endpoint", p.Endpoint.ToString() },
                { "Exchange", p.Exchange },
                { "Id", id.Value.ToIdString() },
                { "Kind", p.Kind.ToString() },
                { "LocalPort", p.LocalPort },
                { "Mandatory", p.Mandatory },
                { "Name", p.Name },
                { "Number", p.Number },
                { "Queue", p.Queue },
                { "RemotePort", p.RemotePort },
                { "Route", p.Route },
                { "Tag", p.Tag },
                { "Topic", p.Topic },
                { "Type", p.Type }
            };
    }

    class Participant : IBrokerParticipant
    {
        public bool Mandatory { get; }
        public string Type { get; }
        public string Exchange { get; }
        public string Route { get; }
        public string Queue { get; set; }
        public string Tag { get; set; }
        public int Number { get; }
        public string Name { get; }
        public AmqpTcpEndpoint Endpoint { get; }
        public int LocalPort { get; }
        public int RemotePort { get; }

        Guid id;
        public Guid? Id => id == default(Guid) ? default(Guid?) : id;
        public string Topic { get; }
        public ParticipantKind Kind { get; }

        internal Participant(string target, Type type, Lazy<Guid> myId, int number,
            string name, AmqpTcpEndpoint endpoint, int localPort, int remotePort)
        {
            Number = number;
            Name = name;
            Endpoint = endpoint;
            LocalPort = localPort;
            RemotePort = remotePort;
            if (string.IsNullOrWhiteSpace(target))
            {
                Type = ExchangeType.Fanout;
                Exchange = RabbitExtensions.ToGeneral(type);
                Route = string.Empty;
                Kind = ParticipantKind.Broadcast;
                Mandatory = false;
            }
            else if (Guid.TryParse(target, out id))
            {
                Type = ExchangeType.Direct;
                if (Id == default(Guid))
                    Exchange = myId.Value.ToIdString();
                else
                    Exchange = Id.Value.ToIdString();
                Route = RabbitExtensions.ToGeneral(type);
                Kind = ParticipantKind.Unicast;
                Mandatory = true;
            }
            else
            {
                Type = ExchangeType.Topic;
                Exchange = target;
                Route = type.FullName;
                Kind = ParticipantKind.Multicast;
                Topic = target;
                Mandatory = false;
            }
        }
    }

    class Envelope<T> : IEnvelope<T>
    {
        public IParticipant Sender { get; }
        public IParticipant Receiver { get; }
        public T Body { get; }
        public IFault Error { get; }
        public IDictionary<string, object> Headers { get; }

        internal Envelope(IParticipant source, T body, BasicDeliverEventArgs e)
        {
            Receiver = source;
            Body = body;
            Error = null;
            Headers = new Dictionary<string, object>()
            {
                {"ConsumerTag", e.ConsumerTag },
                {"DeliveryTag", e.DeliveryTag },
                {"Exchange", e.Exchange },
                {"Redelivered", e.Redelivered },
                {"RoutingKey", e.RoutingKey },
                {"ContentType", e.BasicProperties.ContentType },
                {"ContentEncoding", e.BasicProperties.ContentEncoding },
                {"DeliveryMode", e.BasicProperties.DeliveryMode },
                {"Persistent", e.BasicProperties.Persistent},
                {"Priority", e.BasicProperties.Priority },
                {"ProtocolClassId", e.BasicProperties.ProtocolClassId },
                {"ProtocolClassName", e.BasicProperties.ProtocolClassName },
                {"Timestamp", e.BasicProperties.Timestamp }
            };
            Sender = new RemoteParticipant(e.BasicProperties.Headers);
        }

        internal Envelope(IParticipant source, Exception error)
        {
            Receiver = source;
            Body = default(T);
            Error = new Fault(FaultCode.GeneralError, error.Message, error.StackTrace);
        }
    }
}