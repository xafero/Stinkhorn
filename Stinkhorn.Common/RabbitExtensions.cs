using System.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;

namespace Stinkhorn.Common
{
    static class RabbitExtensions
    {
        public static bool ExchangeExists(this IModel model, string exchangeName)
        {
            try
            {
                model.ExchangeDeclarePassive(exchangeName);
                return true;
            }
            catch (OperationInterruptedException)
            {
                return false;
            }
        }

        public static bool QueueExists(this IModel model, string queueName)
        {
            try
            {
                return model.QueueDeclarePassive(queueName).QueueName == queueName;
            }
            catch (OperationInterruptedException)
            {
                return false;
            }
        }

        public static string ToGeneral(Type type) => type.Namespace.Split('.').First();
        public static string ToGeneral<T>() => ToGeneral(typeof(T));

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

        internal static void Publish(this IModel model, ITransfer addr, string route,
            byte[] body, Dictionary<string, object> headers)
        {
            var exchange = addr.Exchange;
            var mandatory = false;
            var props = model.CreateBasicProperties();
            props.DeliveryMode = (byte)DeliveryMode.Persistent;
            props.Headers = headers;
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