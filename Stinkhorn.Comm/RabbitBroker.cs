using log4net;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;

namespace Stinkhorn.Comm
{
    public class RabbitBroker : IBroker
    {
        static readonly ILog log = LogManager.GetLogger(typeof(RabbitBroker));

        public ISerializer Serializer { get; set; } = new JsonSerializer();
        public Identity Id { get; set; } = new Identity(Guid.NewGuid(),
            typeof(RabbitBroker).FullName);

        ConnectionFactory Factory { get; set; }
        IConnection Connection { get; set; }

        public DnsEndPoint RemoteEndpoint =>
            new DnsEndPoint(Connection.Endpoint.HostName, Connection.Endpoint.Port);

        public DnsEndPoint LocalEndpoint =>
            new DnsEndPoint("localhost", Connection.LocalPort);

        public void Connect(string userName = "guest", string password = "guest",
                            string path = "/", string host = "localhost", int port = 5672)
            => Open($"amqp://{userName}:{password}@{host}:{port}{path}");

        public void Open(string uri = null)
        {
            if (string.IsNullOrWhiteSpace(uri))
            {
                var cfg = ConfigurationManager.ConnectionStrings;
                uri = cfg["RabbitMQ"].ConnectionString;
            }
            Factory = new ConnectionFactory
            {
                Uri = uri,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };
            log.InfoFormat("Connecting to '{0}'...", uri);
            Connection = Factory.CreateConnection();
            Connection.CallbackException += Connection_CallbackException;
            Connection.ConnectionBlocked += Connection_ConnectionBlocked;
            Connection.ConnectionShutdown += Connection_ConnectionShutdown;
            Connection.ConnectionUnblocked += Connection_ConnectionUnblocked;
        }

        void Connection_ConnectionUnblocked(object sender, EventArgs e)
        {
            log.InfoFormat("Connection unblocked");
        }

        void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            log.InfoFormat("Connection unblocked: {0} {1} {2}", e.Initiator, e.ReplyCode, e.ReplyText);
        }

        void Connection_ConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            log.InfoFormat("Connection blocked: {0}", e.Reason);
        }

        void Connection_CallbackException(object sender, CallbackExceptionEventArgs e)
        {
            log.Error("Connection callback exception!", e.Exception);
        }

        public void Close()
        {
            if (Connection == null)
                return;
            Connection.CallbackException -= Connection_CallbackException;
            Connection.ConnectionBlocked -= Connection_ConnectionBlocked;
            Connection.ConnectionShutdown -= Connection_ConnectionShutdown;
            Connection.ConnectionUnblocked -= Connection_ConnectionUnblocked;
            Connection.Close();
            Connection.Dispose();
            Connection = null;
        }

        public void Dispose()
        {
            if (Connection != null)
                Close();
            Factory = null;
        }

        IModel _model;
        IModel Model => _model != null && _model.IsOpen ? _model : (_model = CreateModel());

        IModel CreateModel()
        {
            var model = Connection.CreateModel();
            model.BasicAcks += Model_BasicAcks;
            model.BasicNacks += Model_BasicNacks;
            model.BasicRecoverOk += Model_BasicRecoverOk;
            model.BasicReturn += Model_BasicReturn;
            model.CallbackException += Model_CallbackException;
            model.FlowControl += Model_FlowControl;
            model.ModelShutdown += Model_ModelShutdown;
            return model;
        }

        void Model_ModelShutdown(object sender, ShutdownEventArgs e)
        {
            log.InfoFormat("Model shutdown: {0} {1} {2} {3}", e.Initiator,
                e.Cause, e.ReplyCode, e.ReplyText);
        }

        void Model_FlowControl(object sender, FlowControlEventArgs e)
        {
            log.InfoFormat("Flow control: {0}", e.Active);
        }

        void Model_CallbackException(object sender, CallbackExceptionEventArgs e)
        {
            log.Error("Model callback exception!", e.Exception);
        }

        void Model_BasicReturn(object sender, BasicReturnEventArgs e)
        {
            log.InfoFormat("Model basic return: {0} {1}",
                e.ReplyCode, e.ReplyText);
        }

        void Model_BasicRecoverOk(object sender, EventArgs e)
        {
            log.InfoFormat("Model basic recover ok");
        }

        void Model_BasicNacks(object sender, BasicNackEventArgs e)
        {
            log.InfoFormat("Model basic nacks: {0} {1} {2}",
                e.DeliveryTag, e.Multiple, e.Requeue);
        }

        void Model_BasicAcks(object sender, BasicAckEventArgs e)
        {
            log.InfoFormat("Model basic acks: {0} {1}", e.DeliveryTag, e.Multiple);
        }

        public void Publish<T>(ITransfer addr, T message)
        {
            Model.CreateExchange(addr);
            var route = message.GetType().FullName;
            var bytes = Serializer.Serialize(message);
            Model.Publish(addr, route, bytes, Id);
        }

        public void Subscribe<T>(ITransfer addr, Action<IIdentity, T> callback)
        {
            Model.CreateExchange(addr);
            var route = typeof(T).FullName;
            var queue = Model.CreateQueue();
            var consumer = new EventingBasicConsumer(Model);
            consumer.Received += (s, e) =>
            {
                var message = Serializer.Deserialize<T>(e.Body);
                var heads = e.BasicProperties.Headers;
                Identity src = new Dictionary<string, object>(heads);
                callback(src, message);
                Model.BasicAck(e.DeliveryTag, false);
            };
            consumer.Registered += (s, e) =>
            {
                log.InfoFormat("Consumer '{0}' registered for '{1}'!", e.ConsumerTag, addr);
            };
            consumer.ConsumerCancelled += (s, e) =>
            {
                log.InfoFormat("Consumer '{0}' cancelled for '{1}'!", e.ConsumerTag, addr);
            };
            consumer.Shutdown += (s, e) =>
            {
                var cons = (EventingBasicConsumer)s;
                log.InfoFormat("Consumer '{0}' shutdown: {1} {2} {3} {4}",
                    cons.ConsumerTag, e.Initiator, e.Cause, e.ReplyCode, e.ReplyText);
            };
            consumer.Unregistered += (s, e) =>
            {
                log.InfoFormat("Consumer '{0}' unregistered for '{1}'!", e.ConsumerTag, addr);
            };
            Model.Bind(addr, route, queue);
            Model.Consume(consumer, queue);
        }
    }
}