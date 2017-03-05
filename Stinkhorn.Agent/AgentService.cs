using System.Linq;
using System.Configuration;
using System.ServiceProcess;
using log4net;
using Stinkhorn.API;
using Stinkhorn.Common;
using Stinkhorn.Util;
using Stinkhorn.Core;
using Stinkhorn.IoC;
using System;

namespace Stinkhorn.Agent
{
    public class AgentService : ServiceBase, ISetupService, IConsoleService
    {
        static readonly ILog log = LogManager.GetLogger(typeof(AgentService));
        static readonly object dummy = typeof(ImageExtensions);

        public const string MyServiceName = "StinkhornAgent";

        RabbitBroker Client { get; set; }

        public AgentService()
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            ServiceName = MyServiceName;
        }

        protected override void Dispose(bool disposing)
        {
            DoStop();
            Client = null;
            base.Dispose(disposing);
        }

        protected override void OnStart(string[] args) { DoStart(args); }
        protected override void OnStop() { DoStop(); }
        protected override void OnShutdown() { DoShutdown(); }
        protected override void OnContinue() { DoContinue(); }
        protected override void OnCustomCommand(int cmd) { DoCustomCommand(cmd); }
        protected override void OnPause() { DoPause(); }
        protected override bool OnPowerEvent(PowerBroadcastStatus status) => DoPowerEvent(status);
        protected override void OnSessionChange(SessionChangeDescription desc) => DoSessionChange(desc);

        public void DoSessionChange(SessionChangeDescription desc)
        {
        }

        public bool DoPowerEvent(PowerBroadcastStatus status) => true;

        public void DoPause()
        {
        }

        public void DoCustomCommand(int cmd)
        {
        }

        public void DoContinue()
        {
        }

        public void DoStart(string[] args)
        {
            log.InfoFormat("Service is starting...");
            var config = ConfigurationManager.AppSettings;
            var host = config["host"];
            var port = int.Parse(config["port"]);
            var name = GetType().Name;
            var client = new RabbitBroker();
            client.Connect(host: host, port: port);
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
            client.Subscribe<ScreenshotRequest>(id.Multi, OnScreenShotReq);
            client.Subscribe<ScreenshotRequest>(id.Uni, OnScreenShotReq);
        }

        void OnScreenShotReq(IIdentity sender, ScreenshotRequest req)
        {
            var handler = ServiceLoader.Load<IMessageHandler<ScreenshotRequest,
                                                             ScreenshotResponse>>().First();
            var resp = handler.Process(req);
            Client.Publish(sender.Uni, resp);
        }

        public void DoStop()
        {
            log.InfoFormat("Service is stopping...");
            if (Client != null)
                Client.Dispose();
            log.InfoFormat("Service is stopped!");
        }

        public void DoShutdown()
        {
        }

        public void OnInstallService()
        {
        }

        public void OnUninstallService()
        {
        }
    }
}