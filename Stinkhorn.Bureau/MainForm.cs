using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using Stinkhorn.API;
using Stinkhorn.Common;

namespace Stinkhorn.Bureau
{
	public partial class MainForm : Form
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(MainForm));
		
		private BrokerClient Client { get; set; }

		private readonly BindingList<HelloMessage> contactList;
		
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

		private void MainForm_Load(object sender, EventArgs e)
		{
			log.InfoFormat("Manager is starting...");
			var config = ConfigurationManager.AppSettings;
			var host = config["host"];
			var port = int.Parse(config["port"]);
			var name = GetType().Name;
			var client = new BrokerClient(host: host, port: port);
			Client = client;
			log.InfoFormat("Manager is started!");
			client.Subscribe<HelloMessage>(OnHello);
			client.Subscribe<ScreenshotResponse>(OnScreenshot);
		}
		
		private void OnHello(HelloMessage msg)
		{
			BeginInvoke((Action)(() => contactList.Add(msg)));
		}
		
		private void OnScreenshot(ScreenshotResponse msg)
		{
			throw new NotImplementedException();
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			contactList.Clear();
			Client.Dispose();
			Client = null;
		}
	}
}