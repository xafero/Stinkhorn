using System;
using System.Linq;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using Stinkhorn.API;
using Stinkhorn.Common;
using System.Drawing;
using System.IO;

namespace Stinkhorn.Bureau
{
    public partial class MainForm : Form
	{
        static readonly ILog log = LogManager.GetLogger(typeof(MainForm));

        BrokerClient Client { get; set; }

        readonly BindingList<HelloMessage> contactList;

        public MainForm()
		{
			InitializeComponent();
			// Bindings
			contactList = new BindingList<HelloMessage>();
			var contactSource = new BindingSource(contactList, null);
			dataGridView1.AllowUserToAddRows = false;
			dataGridView1.DataSource = contactSource;
			// Logging
			BasicConfigurator.Configure();
			// Events
			Load += MainForm_Load;
			FormClosing += MainForm_FormClosing;
		}

        void MainForm_Load(object sender, EventArgs e)
        {
            log.InfoFormat("Manager is starting...");
            var config = ConfigurationManager.AppSettings;
            var host = config["host"];
            var port = int.Parse(config["port"]);
            var name = GetType().Name;
            var client = new BrokerClient(host: host, port: port);
            Client = client;
            log.InfoFormat("Manager is started!");
            client.Publish<HelloMessage>(null);
            client.Subscribe<HelloMessage>(OnHello);
            client.Subscribe<ScreenshotResponse>(OnScreenshot);
        }

        void OnHello(HelloMessage msg)
        {
            BeginInvoke((Action)(() => {
                contactList.Add(msg);
                Client.Publish(new ScreenshotRequest(), msg.SenderId);
            }));
        }

        void OnScreenshot(ScreenshotResponse msg)
        {
            BeginInvoke((Action)(() =>
            {
                var screen = msg.Screenshots.First();
                var dialog = new Form();
                var box = new PictureBox();
                using (var stream = new MemoryStream(screen.Bytes))
                    box.Image = Image.FromStream(stream);
                box.Dock = DockStyle.Fill;
                dialog.Controls.Add(box);
                dialog.Size = new Size(screen.Width, screen.Height);
                dialog.ShowDialog(this);
            }));
        }

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            contactList.Clear();
            Client.Dispose();
            Client = null;
        }
    }
}