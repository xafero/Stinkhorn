using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Stinkhorn.Util
{
	public static class ServiceHelper
	{
		public static void RunConsole<T>(T service, string[] args) where T : ServiceBase, IConsoleService
		{
			Console.WriteLine("Starting {0}...", service.ServiceName);
			service.DoStart(args);
			Console.WriteLine("Waiting for ENTER to quit...");
			Console.ReadLine();
			service.DoStop();
			service.DoShutdown();
		}

		public static int InstallService<T>(T service) where T : ServiceBase, ISetupService
		{
			try
			{
				service.OnInstallService();
				var assembly = service.GetType().Assembly;
				ManagedInstallerClass.InstallHelper(new [] { assembly.Location });
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null && ex.InnerException.GetType() == typeof(Win32Exception))
				{
					var wex = (Win32Exception)ex.InnerException;
					Console.WriteLine("Error(0x{0:X}): Service already installed!", wex.ErrorCode);
					return wex.ErrorCode;
				}
				else
				{
					Console.WriteLine(ex.ToString());
					return -1;
				}
			}
			return 0;
		}

		public static int UninstallService<T>(T service) where T : ServiceBase, ISetupService
		{
			try
			{
				service.OnUninstallService();
				var assembly = service.GetType().Assembly;
				ManagedInstallerClass.InstallHelper(new [] { "/u", assembly.Location });
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null && ex.InnerException.GetType() == typeof(Win32Exception))
				{
					var wex = (Win32Exception)ex.InnerException;
					Console.WriteLine("Error(0x{0:X}): Service not installed!", wex.ErrorCode);
					return wex.ErrorCode;
				}
				else
				{
					Console.WriteLine(ex.ToString());
					return -1;
				}
			}
			return 0;
		}
	}
}