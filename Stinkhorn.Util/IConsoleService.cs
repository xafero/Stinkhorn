namespace Stinkhorn.Util
{
    public interface IConsoleService
	{
		void DoStart(string[] args);
		
		void DoStop();
		
		void DoShutdown();
	}
}