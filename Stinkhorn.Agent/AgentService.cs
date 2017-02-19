using System;
using System.ServiceProcess;
using Stinkhorn.Util;

namespace Stinkhorn.Agent
{
	public class AgentService : ServiceBase, ISetupService, IConsoleService
	{
		public const string MyServiceName = "StinkhornAgent";
		
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
		}
		
		public void DoStop()
		{
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