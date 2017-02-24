﻿using System.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

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

        public static string ToGeneral<T>() => typeof(T).Namespace.Split('.').First();
    }
}