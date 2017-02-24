using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Stinkhorn.Agent
{
    [RunInstaller(true)]
	public class ProjectInstaller : Installer
	{
        readonly ServiceProcessInstaller serviceProcessInstaller;
        readonly ServiceInstaller serviceInstaller;

        public ProjectInstaller()
		{
			serviceProcessInstaller = new ServiceProcessInstaller();
			serviceInstaller = new ServiceInstaller();
			serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
			
			serviceInstaller.ServiceName = AgentService.MyServiceName;
            Installers.AddRange(new Installer[] { serviceProcessInstaller, serviceInstaller });
		}
	}
}