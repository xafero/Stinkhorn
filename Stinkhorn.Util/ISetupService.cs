using System;

namespace Stinkhorn.Util
{
	public interface ISetupService
	{
		void OnInstallService();

		void OnUninstallService();
	}
}