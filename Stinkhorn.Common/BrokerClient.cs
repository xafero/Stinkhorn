using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using log4net;
using log4net.Repository.Hierarchy;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Stinkhorn.Common
{
	public class BrokerClient : IDisposable
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(BrokerClient));
		
		private static readonly JsonSerializerSettings config = new JsonSerializerSettings {
			Formatting = Formatting.None, NullValueHandling = NullValueHandling.Ignore,
			ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
		};
		
		private IConnection conn;
		private IModel channel;

		public BrokerClient(string userName = "guest", string password = "guest",
		                    string path = "/", string host = "localhost", int port = 5672)
		{
			var uri = string.Format("amqp://{0}:{1}@{2}:{3}{4}", userName, password, host, port, path);
			var factory = new ConnectionFactory {
				Uri = uri, AutomaticRecoveryEnabled = true,
				NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
			};
			log.InfoFormat("Connecting to '{0}'...", uri);
			conn = factory.CreateConnection();
			channel = conn.CreateModel();
		}
		
		public void Publish<T>(T message)
		{
			var exchange = message.GetType().Namespace;
			var route = message.GetType().Name;
			var text = JsonConvert.SerializeObject(message, config);
			var bytes = Encoding.UTF8.GetBytes(text);
			var props = channel.CreateBasicProperties();
			props.ContentType = "application/json";
			props.DeliveryMode = (byte)DeliveryMode.Persistent;
			props.Headers = new Dictionary<string, object>();
			channel.ExchangeDeclare(exchange, "direct");
			channel.BasicPublish(exchange, route, props, bytes);
		}
		
		public void Subscribe<T>(Action<T> callback)
		{
			var type = typeof(T);
			var consumer = new EventingBasicConsumer(channel);
			consumer.Received += (sender, e) =>
			{
				var text = Encoding.UTF8.GetString(e.Body);
				var msg = JsonConvert.DeserializeObject(text, type, config);
				callback((T)msg);
				channel.BasicAck(e.DeliveryTag, false);
			};
			channel.QueueDeclare("Simple");
			channel.QueueBind("Simple", type.Namespace, type.Name);
			channel.BasicConsume("Simple", false, consumer);
		}
		
		public DnsEndPoint RemoteEndpoint {
			get { return new DnsEndPoint(conn.Endpoint.HostName, conn.Endpoint.Port); }
		}
		
		public DnsEndPoint LocalEndpoint {
			get { return new DnsEndPoint("localhost", conn.LocalPort); }
		}

		public void Dispose()
		{
			if (conn != null)
			{
				conn.Close();
				conn.Dispose();
			}
			channel = null;
			conn = null;
		}
	}
}