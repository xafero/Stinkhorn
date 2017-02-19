using System;

namespace Stinkhorn.Util
{
	public interface ISetupService
	{
		void OnInstallService();

		void OnUninstallService();
	}
	
	public interface IConsoleService
	{
		void DoStart(string[] args);
		
		void DoStop();
		
		void DoShutdown();
	}
}