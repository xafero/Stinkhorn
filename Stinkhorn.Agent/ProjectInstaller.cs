using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Stinkhorn.Agent
{
	[RunInstaller(true)]
	public class ProjectInstaller : Installer
	{
		private readonly ServiceProcessInstaller serviceProcessInstaller;
		private readonly ServiceInstaller serviceInstaller;
		
		public ProjectInstaller()
		{
			serviceProcessInstaller = new ServiceProcessInstaller();
			serviceInstaller = new ServiceInstaller();
			serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
			
			serviceInstaller.ServiceName = AgentService.MyServiceName;
			this.Installers.AddRange(new Installer[] { serviceProcessInstaller, serviceInstaller });
		}
	}
}