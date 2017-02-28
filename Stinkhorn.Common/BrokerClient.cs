using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using log4net;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Stinkhorn.Common
{
    public class BrokerClient : IBrokerClient
    {
        static readonly ILog log = LogManager.GetLogger(typeof(BrokerClient));

        static readonly JsonSerializerSettings config = new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            NullValueHandling = NullValueHandling.Ignore,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            TypeNameHandling = TypeNameHandling.All
        };

        IConnection conn;

        public Lazy<Guid> MyID => new Lazy<Guid>(() => Guid.NewGuid());

        public BrokerClient(string userName = "guest", string password = "guest",
                            string path = "/", string host = "localhost", int port = 5672)
            : this($"amqp://{userName}:{password}@{host}:{port}{path}")
        {
        }

        public BrokerClient(string uri)
        {
            var factory = new ConnectionFactory
            {
                Uri = uri,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };
            log.InfoFormat("Connecting to '{0}'...", uri);
            conn = factory.CreateConnection();
        }

        IModel _channel;
        IModel Channel => _channel == null || _channel.IsClosed
            ? (_channel = conn.CreateModel()) : _channel;

        public void Publish<T>(T message, string target = null)
        {
            IBrokerParticipant dest = new Participant(target, typeof(T), MyID, Channel.ChannelNumber,
                conn.ClientProvidedName, conn.Endpoint, conn.LocalPort, conn.Endpoint.Port);
            if (!Channel.ExchangeExists(dest.Exchange))
            {
                var durable = false;
                var autoDelete = true;
                IDictionary<string, object> arguments = null;
                Channel.ExchangeDeclare(dest.Exchange, dest.Type, durable, autoDelete, arguments);
            }
            var text = JsonConvert.SerializeObject(message, config);
            var enc = Encoding.UTF8;
            var bytes = enc.GetBytes(text);
            var props = Channel.CreateBasicProperties();
            props.ContentEncoding = enc.EncodingName;
            props.ContentType = "application/json";
            props.DeliveryMode = (byte)DeliveryMode.Persistent;
            props.Headers = dest.ToDict(MyID);
            Channel.BasicPublish(dest.Exchange, dest.Route, dest.Mandatory, props, bytes);
        }

        public void Subscribe<T>(Action<IEnvelope<T>> callback, string target = null)
        {
            IBrokerParticipant source = new Participant(target, typeof(T), MyID, Channel.ChannelNumber,
                conn.ClientProvidedName, conn.Endpoint, conn.LocalPort, conn.Endpoint.Port);
            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += (sender, e) =>
            {
                var text = Encoding.UTF8.GetString(e.Body);
                var msg = JsonConvert.DeserializeObject(text, config);
                if (!(msg is T))
                    return;
                callback(new Envelope<T>(source, (T)msg, e));
                Channel.BasicAck(e.DeliveryTag, false);
            };
            var queue = string.Empty;
            var durable = false;
            var exclusive = true;
            var autoDelete = true;
            IDictionary<string, object> arguments = null;
            var qOk = Channel.QueueDeclare(queue, durable, exclusive, autoDelete, arguments);
            source.Queue = qOk.QueueName;
            Channel.QueueBind(queue, source.Exchange, source.Route, arguments);
            bool noAck = false;
            string consumerTag = "";
            bool noLocal = false;
            source.Tag = Channel.BasicConsume(queue, noAck, consumerTag, noLocal, exclusive, arguments, consumer);
        }

        public DnsEndPoint RemoteEndpoint =>
            new DnsEndPoint(conn.Endpoint.HostName, conn.Endpoint.Port);

        public DnsEndPoint LocalEndpoint =>
            new DnsEndPoint("localhost", conn.LocalPort);

        public void Dispose()
        {
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
            _channel = null;
            conn = null;
        }
    }
}