using System.Linq;
using log4net;
using Stinkhorn.API;
using Stinkhorn.Comm;
using Mono.Addins;
using System;

namespace Stinkhorn.Agent
{
    public class AgentService : IAgentService
    {
        static readonly ILog log = LogManager.GetLogger(typeof(AgentService));

        public const string MyServiceName = "StinkhornAgent";

        RabbitBroker Client { get; set; }

        public void Dispose()
        {
            Stop();
            Client = null;
        }

        public void Start()
        {
            log.InfoFormat("Service is starting...");
            var name = GetType().Name;
            var client = new RabbitBroker();
            client.Open();
            Client = client;
            log.InfoFormat("Service is started!");
            var id = client.Id;
            client.Publish(id.Broad, new HelloMessage
            {
                Machine = Environment.MachineName,
                User = Environment.UserName,
                Local = client.LocalEndpoint.ToShortString(),
                Remote = client.RemoteEndpoint.ToShortString()
            });
            Subscribe<ScreenshotRequest, ScreenshotResponse>();
            Subscribe<RegistryRequest, RegistryResponse>();
            Subscribe<PowerRequest, PowerResponse>();
            Subscribe<ServeRequest, ServeResponse>();
            Subscribe<InfoRequest, InfoResponse>();
        }

        void Subscribe<I, O>()
            where I : IRequest where O : IResponse
        {
            var cid = Client.Id;
            var ids = new ITransfer[] { cid.Multi, cid.Uni };
            foreach (var id in ids)
                Client.Subscribe<I>(id, OnRequest<I, O>);
        }

        void OnRequest<I, O>(IIdentity sender, I req)
            where I : IRequest where O : IResponse
        {
            var handler = AddinManager.GetExtensionObjects<IMessageHandler<I, O>>().First();
            var resp = handler.Process(req);
            Client.Publish(sender.Uni, resp);
        }

        public void Stop()
        {
            log.InfoFormat("Service is stopping...");
            if (Client != null)
                Client.Dispose();
            log.InfoFormat("Service is stopped!");
        }
    }
}