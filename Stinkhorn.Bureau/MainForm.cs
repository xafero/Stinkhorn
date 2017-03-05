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
using Stinkhorn.Bureau.Utils;
using Stinkhorn.Bureau.Controls;
using Stinkhorn.IoC;
using Stinkhorn.Bureau.Context;

namespace Stinkhorn.Bureau
{
    public partial class MainForm : Form
    {
        static readonly ILog log = LogManager.GetLogger(typeof(MainForm));

        RabbitBroker Client { get; set; }

        readonly BindingList<Contact> contactList;

        public MainForm()
        {
            InitializeComponent();
            // Bindings
            contactList = new BindingList<Contact>();
            var contactSource = new BindingSource(contactList, null);
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DataSource = contactSource;
            // Logging
            BasicConfigurator.Configure();
            // Events
            Load += MainForm_Load;
            FormClosing += MainForm_FormClosing;
            dataGridView1.RowsAdded += DataGridView1_RowsAdded;
            Icon = Icons.Logo.ToIcon();
        }

        void MainForm_Load(object sender, EventArgs e)
        {
            log.InfoFormat("Manager is starting...");
            var config = ConfigurationManager.AppSettings;
            var host = config["host"];
            var port = int.Parse(config["port"]);
            var name = GetType().Name;
            var client = new RabbitBroker();
            client.Connect(host: host, port: port);
            Client = client;
            log.InfoFormat("Manager is started!");
            var id = client.Id;
            client.Subscribe<HelloMessage>(id.Broad, OnHello);
            client.Subscribe<ScreenshotResponse>(id.Uni, OnScreenshot);
        }

        void OnHello(IIdentity sender, HelloMessage msg)
        {
            BeginInvoke((Action)(() =>
            {
                receiverDump1.Receive(msg, sender.Uni);
                contactList.Add(new Contact(sender, msg));
            }));
        }

        void DataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            var grid = (DataGridView)sender;
            var row = grid.Rows[e.RowIndex];
            var msg = (Contact)row.DataBoundItem;
            var menu = new ContextMenuStrip();
            var reqs = ServiceLoader.Load<IRequest>().ToArray();
            foreach (var req in reqs)
            {
                var name = req.GetType().Name.Replace(typeof(IRequest)
                    .Name.Substring(1), string.Empty);
                var icon = (Image)Icons.ResourceManager.GetObject(name);
                EventHandler onclick = (s, a) =>
                {
                    var userReq = (IMessage)Activator.CreateInstance(req.GetType());
                    using (var dialog = new RequestDialog(req, icon))
                        if (dialog.ShowDialog(this) == DialogResult.OK)
                            Client.Publish(msg.Id, req);
                };
                var item = new ToolStripMenuItem(name, icon, onclick);
                menu.Items.Add(item);
            }
            row.ContextMenuStrip = menu;
        }

        void OnScreenshot(IIdentity sender, ScreenshotResponse msg)
        {
            BeginInvoke((Action)(() =>
            {
                receiverDump1.Receive(msg, sender.Uni);
                /*var screen = msg.Screenshots.First();
                var dialog = new Form();
                var box = new PictureBox();
                box.Image = screen.ToDrawingImage();
                box.Dock = DockStyle.Fill;
                box.SizeMode = PictureBoxSizeMode.Zoom;
                dialog.Controls.Add(box);
                dialog.Size = new Size(screen.Width, screen.Height);
                dialog.ShowDialog(this);*/
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