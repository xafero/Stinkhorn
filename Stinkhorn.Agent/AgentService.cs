﻿using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using log4net;
using Stinkhorn.API;
using Stinkhorn.Common;
using Stinkhorn.Util;

namespace Stinkhorn.Agent
{
	public class AgentService : ServiceBase, ISetupService, IConsoleService
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(AgentService));
		
		public const string MyServiceName = "StinkhornAgent";
		
		private BrokerClient Client { get; set; }
		
		public AgentService()
		{
			InitializeComponent();
		}
		
		private void InitializeComponent()
		{
			this.ServiceName = MyServiceName;
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
		protected override bool OnPowerEvent(PowerBroadcastStatus status) { return DoPowerEvent(status); }
		protected override void OnSessionChange(SessionChangeDescription desc) { DoSessionChange(desc); }

		public void DoSessionChange(SessionChangeDescription desc)
		{
		}
		
		public bool DoPowerEvent(PowerBroadcastStatus status)
		{
			return true;
		}
		
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
			var client = new BrokerClient(host: host, port: port);
			Client = client;
			log.InfoFormat("Service is started!");
			
			client.Subscribe<HelloMessage>(msg => {
			                               	Debugger.Break();
			                               });
			
			client.Publish(new HelloMessage {
			               	Local = client.LocalEndpoint + "",
			               	Remote = client.RemoteEndpoint + ""
			               });
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