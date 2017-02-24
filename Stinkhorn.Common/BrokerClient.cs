using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Text;
using log4net;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics;

namespace Stinkhorn.Common
{
    public class BrokerClient : IBrokerClient
    {
        static readonly ILog log = LogManager.GetLogger(typeof(BrokerClient));

        static readonly JsonSerializerSettings config = new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            NullValueHandling = NullValueHandling.Ignore,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
        };

        IConnection conn;

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
            string type;
            string exchange;
            string route;
            bool mandatory;
            Guid id;
            if (string.IsNullOrWhiteSpace(target))
            {
                type = ExchangeType.Fanout;
                exchange = RabbitExtensions.ToGeneral<T>();
                route = string.Empty;
                mandatory = false;
            }
            else if (Guid.TryParse(target, out id))
            {
                type = ExchangeType.Direct;
                exchange = id.ToString("N");
                route = RabbitExtensions.ToGeneral<T>();
                mandatory = true;
            }
            else
            {
                type = ExchangeType.Topic;
                exchange = target;
                route = typeof(T).FullName;
                mandatory = false;
            }
            if (!Channel.ExchangeExists(exchange))
            {
                var durable = false;
                var autoDelete = false;
                IDictionary<string, object> arguments = null;
                Channel.ExchangeDeclare(exchange, type, durable, autoDelete, arguments);
            }
            var text = JsonConvert.SerializeObject(message, config);
            var bytes = Encoding.UTF8.GetBytes(text);
            var props = Channel.CreateBasicProperties();
            props.ContentType = "application/json";
            props.DeliveryMode = (byte)DeliveryMode.Persistent;
            props.Headers = new Dictionary<string, object>();
            Channel.BasicPublish(exchange, route, mandatory, props, bytes);
        }

        public void Subscribe<T>(Action<T> callback)
        {
            var type = typeof(T);
            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += (sender, e) =>
            {
                var text = Encoding.UTF8.GetString(e.Body);
                var msg = JsonConvert.DeserializeObject(text, type, config);
                callback((T)msg);
                Channel.BasicAck(e.DeliveryTag, false);
            };

            /*channel.QueueDeclare("Simple");
            channel.QueueBind("Simple", type.Namespace, type.Name);
            channel.BasicConsume("Simple", false, consumer);*/
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