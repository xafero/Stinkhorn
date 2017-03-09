using Topshelf;
using System.Configuration;
using Mono.Addins;
using Stinkhorn.Util;

namespace Stinkhorn.Agent
{
    static class Program
    {
        static void Main()
        {
            InitAddins();
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

        static void InitAddins()
        {
            var cfg = ConfigurationManager.AppSettings;
            var cfgDir = cfg.TryGetPath("config_dir");
            var plgDir = cfg.TryGetPath("addins_dir");
            var dbDir = cfg.TryGetPath("database_dir");
            AddinManager.Initialize(cfgDir, plgDir, dbDir);
            AddinManager.Registry.Update(new ConsoleProgressStatus(true));
        }
    }
}