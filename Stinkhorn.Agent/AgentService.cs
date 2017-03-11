using System.Linq;
using log4net;
using Stinkhorn.API;
using Stinkhorn.Comm;
using Mono.Addins;
using System;
using System.Collections.Generic;

namespace Stinkhorn.Agent
{
    public class AgentService : IAgentService
    {
        static readonly ILog log = LogManager.GetLogger(typeof(AgentService));

        public const string MyServiceName = "StinkhornAgent";

        RabbitBroker Client { get; set; }
        IDictionary<Type, IRequestHandler> Handlers { get; set; }

        public void Dispose()
        {
            Stop();
            Client = null;
            Handlers.Clear();
            Handlers = null;
        }

        public void Start()
        {
            Handlers = new Dictionary<Type, IRequestHandler>();
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
            foreach (var handler in AddinExtensions.GetFiltered<IRequestHandler>())
            {
                var intfName = typeof(IRequestHandler<>).Name;
                var intf = handler.GetType().GetInterface(intfName);
                var reqType = intf.GetGenericArguments()[0];
                var meth = GetType().GetMethod(nameof(Subscribe));
                meth = meth.MakeGenericMethod(reqType);
                meth.Invoke(this, null);
                Handlers[reqType] = handler;
            }

            // TODO ...
            //Subscribe<ScreenshotRequest, ScreenshotResponse>();
            Subscribe<RegistryRequest>();
            Subscribe<PowerRequest>();
            Subscribe<ServeRequest>();
            //Subscribe<InfoRequest, InfoResponse>();
        }

        public void Subscribe<I>() where I : IRequest
        {
            var cid = Client.Id;
            var ids = new ITransfer[] { cid.Multi, cid.Uni };
            foreach (var id in ids)
                Client.Subscribe<I>(id, OnRequest);
        }

        void OnRequest<I>(IIdentity sender, I req) where I : IRequest
        {
            var handler = (IRequestHandler<I>)Handlers[typeof(I)];
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