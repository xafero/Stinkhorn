using log4net.Config;
using Mono.Addins;
using Stinkhorn.Util;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace Stinkhorn.Bureau
{
    sealed class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            InitAddins();
            BasicConfigurator.Configure();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
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