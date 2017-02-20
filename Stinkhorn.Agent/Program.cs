using System;
using System.Linq;
using System.ServiceProcess;
using log4net.Config;
using Stinkhorn.Util;

namespace Stinkhorn.Agent
{
	static class Program
	{
		static void Main(string[] args)
		{
			try
			{
				var cmp = StringComparer.InvariantCultureIgnoreCase;
				var service = new AgentService();
				if (Environment.UserInteractive)
				{
					BasicConfigurator.Configure();
					if (args.Contains("-install", cmp))
					{
						ServiceHelper.InstallService(service);
						return;
					}
					if (args.Contains("-uninstall", cmp))
					{
						ServiceHelper.UninstallService(service);
						return;
					}
					ServiceHelper.RunConsole(service, args);
					return;
				}
				ServiceBase.Run(new ServiceBase[] { service });
			}
			catch (Exception ex)
			{
				Console.WriteLine("An exception occurred in Main(): {0}", ex);
			}
		}
	}
}