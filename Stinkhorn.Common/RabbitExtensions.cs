using System.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;

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
    }
}