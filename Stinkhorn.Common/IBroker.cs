using System;
using System.Configuration;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Reflection;
using RabbitMQ.Client.Events;
using System.Linq;
using log4net;

namespace Stinkhorn.Common
{
    public interface IId<T>
    {
        T Id { get; }
    }

    public interface IPubSubBroker
    {
        void Publish<T>(ITransfer addr, T message);

        void Subscribe<T>(ITransfer addr, Action<T> callback);
    }

    public interface IBroker : IId<Guid>, IDisposable, IPubSubBroker
    {
        void Open(string uri = null);

        void Close();
    }

    public class RabbitBroker : IBroker
    {
        static readonly ILog log = LogManager.GetLogger(typeof(RabbitBroker));

        public ISerializer Serializer { get; set; } = new JsonSerializer();
        public Guid Id { get; set; } = Guid.NewGuid();

        ConnectionFactory Factory { get; set; }
        IConnection Connection { get; set; }

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
                AutomaticRecoveryEnabled = true
            };
            Connection = Factory.CreateConnection();
            Connection.CallbackException += Connection_CallbackException;
            Connection.ConnectionBlocked += Connection_ConnectionBlocked;
            Connection.ConnectionShutdown += Connection_ConnectionShutdown;
            Connection.ConnectionUnblocked += Connection_ConnectionUnblocked;
        }

        void Connection_ConnectionUnblocked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            throw new NotImplementedException();
        }

        void Connection_ConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void Connection_CallbackException(object sender, CallbackExceptionEventArgs e)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        void Model_CallbackException(object sender, CallbackExceptionEventArgs e)
        {
            log.Error("Model callback exception!", e.Exception);
        }

        void Model_BasicReturn(object sender, BasicReturnEventArgs e)
        {
            throw new NotImplementedException();
        }

        void Model_BasicRecoverOk(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void Model_BasicNacks(object sender, BasicNackEventArgs e)
        {
            throw new NotImplementedException();
        }

        void Model_BasicAcks(object sender, BasicAckEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Publish<T>(ITransfer addr, T message)
        {
            Model.CreateExchange(addr);
            var route = message.GetType().FullName;
            var bytes = Serializer.Serialize(message);
            Model.Publish(addr, route, bytes);
        }

        public void Subscribe<T>(ITransfer addr, Action<T> callback)
        {
            Model.CreateExchange(addr);
            var route = typeof(T).FullName;
            var queue = Model.CreateQueue();
            var consumer = new EventingBasicConsumer(Model);
            consumer.Received += (s, e) =>
            {
                var message = Serializer.Deserialize<T>(e.Body);
                callback(message);
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
               throw new NotImplementedException();
           };
            Model.Bind(addr, route, queue);
            Model.Consume(consumer, queue);
        }
    }

    public interface ITransfer
    {
        string Exchange { get; }
        string Type { get; }
    }

    public interface IBroadcast : ITransfer
    {
    }

    public interface IMulticast : ITransfer
    {
        string Topic { get; }
    }

    public interface IUnicast : ITransfer
    {
        Guid Id { get; }
    }

    public class Broadcast : Cached<bool, Broadcast>, IBroadcast
    {
        Broadcast(bool value) { }

        public override string ToString() => "*".ToLowerInvariant();

        public static Broadcast Of() => GetOrCreate(true);

        public string Type => ExchangeType.Fanout;
        public string Exchange => GetType().Namespace;
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

    public class Multicast : Cached<string, Multicast>, IMulticast
    {
        public string Topic { get; }

        Multicast(string topic) { Topic = topic; }

        public override string ToString() => Topic.Trim().ToLowerInvariant();

        public static Multicast Of<T>() => GetOrCreate(typeof(T).FullName);

        public string Type => ExchangeType.Topic;
        public string Exchange => ToString();
    }

    public class Cached<I, O>
    {
        protected static O CreateObject(I input)
            => (O)typeof(O).GetConstructor(BindingFlags.NonPublic
                | BindingFlags.Instance, null, new[] { typeof(I) }, null)
            .Invoke(new object[] { input });

        internal static IDictionary<I, O> cached = new Dictionary<I, O>();

        internal static O GetOrCreate(I id)
        {
            O adr;
            if (cached.TryGetValue(id, out adr))
                return adr;
            return (cached[id] = CreateObject(id));
        }
    }

    static class RabbitExts
    {
        internal static void CreateExchange(this IModel model, ITransfer addr)
        {
            var exchange = addr.Exchange;
            var type = addr.Type;
            var durable = false;
            var autoDel = true;
            IDictionary<string, object> args = null;
            model.ExchangeDeclare(exchange, type, durable, autoDel, args);
        }

        internal static QueueDeclareOk CreateQueue(this IModel model, string queue = "")
        {
            var durable = false;
            var exclusive = true;
            var autoDel = true;
            IDictionary<string, object> args = null;
            return model.QueueDeclare(queue, durable, exclusive, autoDel, args);
        }

        internal static void Publish(this IModel model, ITransfer addr, string route, byte[] body)
        {
            var exchange = addr.Exchange;
            var mandatory = false;
            var props = model.CreateBasicProperties();
            model.BasicPublish(exchange, route, mandatory, props, body);
        }

        internal static void Bind(this IModel model, ITransfer addr, string route, string queue)
        {
            var exchange = addr.Exchange;
            IDictionary<string, object> args = null;
            model.QueueBind(queue, exchange, route, args);
        }

        internal static string Consume(this IModel model, IBasicConsumer consumer, string queue)
        {
            var noAck = false;
            var tag = "";
            var noLocal = false;
            var exclusive = false;
            IDictionary<string, object> args = null;
            return model.BasicConsume(queue, noAck, tag, noLocal, exclusive, args, consumer);
        }
    }
}