using System;
using System.Collections.Generic;
using RabbitMQ.Client;
using System.Text;

namespace Stinkhorn.Common
{
    class RemoteParticipant : IBrokerParticipant
    {
        public RemoteParticipant(IDictionary<string, object> headers)
        {
            Exchange = GetString(headers, "Exchange");
            Name = GetString(headers, "Name");
            Queue = GetString(headers, "Queue");
            Route = GetString(headers, "Route");
            Tag = GetString(headers, "Tag");
            Topic = GetString(headers, "Topic");
            Type = GetString(headers, "Type");
            Endpoint = new AmqpTcpEndpoint(new Uri(GetString(headers, "Endpoint")));
            Guid id;
            if (Guid.TryParse(GetString(headers, "Id"), out id))
                Id = id;
            Kind = GetEnum<ParticipantKind>(headers, "Kind");
            LocalPort = (int)headers["LocalPort"];
            Mandatory = (bool)headers["Mandatory"];
            Number = (int)headers["Number"];
            RemotePort = (int)headers["RemotePort"];
        }

        public AmqpTcpEndpoint Endpoint { get; }
        public string Exchange { get; }
        public Guid? Id { get; }
        public ParticipantKind Kind { get; }
        public int LocalPort { get; }
        public bool Mandatory { get; }
        public string Name { get; }
        public int Number { get; }
        public string Queue { get; set; }
        public int RemotePort { get; }
        public string Route { get; }
        public string Tag { get; set; }
        public string Topic { get; }
        public string Type { get; }

        T GetEnum<T>(IDictionary<string, object> headers, string key)
              => (T)Enum.Parse(typeof(T), GetString(headers, key), true);

        string GetString(IDictionary<string, object> headers, string key)
                => Encoding.UTF8.GetString((byte[])headers[key] ?? new byte[0]);
    }
}