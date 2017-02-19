using System;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceProcess;
using Stinkhorn.Agent.API;
using Stinkhorn.Util;

namespace Stinkhorn.Agent
{
	public class AgentService : ServiceBase, ISetupService, IConsoleService
	{
		public const string MyServiceName = "StinkhornAgent";
		
		private ServiceHost ServiceHost { get; set; }
		
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
			Stop();
			ServiceHost = null;
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
			var config = ConfigurationManager.AppSettings;
			var host = config["host"];
			var port = config["port"];
			var name = GetType().Name;
			var uri = string.Format("http://{0}:{1}/{2}", host, port, name);
			ServiceHost = new ServiceHost(typeof(StinkAgent), new Uri(uri));
			var binding = new WSHttpBinding();
			ServiceHost.AddServiceEndpoint(typeof(IStinkAgentService), binding, "");
			var behavior = new ServiceMetadataBehavior { HttpGetEnabled = true };
			ServiceHost.Description.Behaviors.Add(behavior);
			Console.WriteLine("Opening listener => {0}", uri);
			ServiceHost.Open();
			var addr = ServiceHost.BaseAddresses.Select(u => u+"");
			var txt = string.Join(" | ", addr.ToArray());
			Console.WriteLine("Service is started => {0}", txt);
		}
		
		public void DoStop()
		{
			if (ServiceHost == null)
				return;
			if (ServiceHost.State == CommunicationState.Closed)
				return;
			ServiceHost.Close();
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