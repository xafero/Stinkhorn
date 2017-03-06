using Topshelf;

namespace Stinkhorn.Agent
{
    static class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(config =>
            {
                config.Service<AgentService>(
                    service =>
                    {
                        service.ConstructUsing(x => new AgentService());
                        service.WhenStarted(x => x.Start());
                        service.WhenStopped(x => x.Stop());
                    });
                config.SetServiceName(AgentService.MyServiceName);
                config.SetDisplayName("An agent for Stinkhorn");
                config.SetDescription("Don't let the mushrooms kill you.");
            });
        }
    }
}