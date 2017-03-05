using System;
using System.Net;
using RabbitMQ.Client;

namespace Stinkhorn.Common
{
    public class BrokerClient : IBrokerClient
    {        
        public void Subscribe<T>(Action<IEnvelope<T>> callback, string target = null)
        {
             
            
            
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