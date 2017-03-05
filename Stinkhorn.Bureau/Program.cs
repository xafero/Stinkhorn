using log4net.Config;
using System;
using System.Windows.Forms;

namespace Stinkhorn.Bureau
{
	sealed class Program
	{
        [STAThread]
        static void Main(string[] args)
        {
            BasicConfigurator.Configure();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}