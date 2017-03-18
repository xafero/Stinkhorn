using System;
using System.Linq;
using System.ComponentModel;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using Stinkhorn.API;
using Stinkhorn.Comm;
using System.Drawing;
using Stinkhorn.Bureau.Utils;
using Stinkhorn.Bureau.Controls;
using Stinkhorn.Bureau.Context;
using System.Reflection;
using Mono.Addins;

namespace Stinkhorn.Bureau
{
    public partial class MainForm : Form, IAddressBook
    {
        static readonly ILog log = LogManager.GetLogger(typeof(MainForm));

        RabbitBroker Client { get; set; }

        readonly BindingList<Contact> contactList;
        readonly IResponseHandler[] handlers;

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
            // Extensions
            handlers = AddinExtensions.GetFiltered<CustomExtensionAttribute,
                IResponseHandler>().Select(v => v.Value).ToArray();
        }

        void MainForm_Load(object sender, EventArgs e)
        {
            log.InfoFormat("Manager is starting...");
            var name = GetType().Name;
            var client = new RabbitBroker();
            client.Open();
            Client = client;
            log.InfoFormat("Manager is started!");
            var id = client.Id;
            client.Subscribe<HelloMessage>(id.Broad, OnResponse);
            foreach (var pair in AddinExtensions.GetFiltered<ResponseDescAttribute, IResponse>())
            {
                var resp = pair.Value;
                var rType = resp.GetType();
                var subName = nameof(client.Subscribe);
                var subMeth = client.GetType().GenericMe(subName, rType);
                var hndlMeth = GetType().GenericMe(nameof(OnResponse), rType);
                var act = typeof(Action<,>).MakeGenericType(typeof(IIdentity), rType);
                var dlgt = Delegate.CreateDelegate(act, this, hndlMeth);
                Invoke(client, subMeth, id.Uni, dlgt, rType);
                Invoke(client, subMeth, id.Multi, dlgt, rType);
            }
        }

        void Invoke(RabbitBroker client, MethodInfo meth, ITransfer trf, Delegate dlgt, Type type)
        {
            meth.Invoke(client, new object[] { trf, dlgt });
            log.InfoFormat("Subscribed for '{0}' on '{1}'.", type, trf);
        }

        void DataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            var grid = (DataGridView)sender;
            var row = grid.Rows[e.RowIndex];
            var msg = (Contact)row.DataBoundItem;
            var menu = new ContextMenuStrip();
            var reqs = AddinExtensions.GetFiltered<RequestDescAttribute, IRequest>().ToArray();
            foreach (var pair in reqs)
            {
                var req = pair.Value;
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
                var category = pair.Key?.Category;
                if (string.IsNullOrWhiteSpace(category))
                {
                    menu.Items.Add(item);
                }
                else
                {
                    var subMenu = menu.Items.OfType<ToolStripDropDownItem>().FirstOrDefault(i => i.Text == category);
                    if (subMenu == null)
                        menu.Items.Add(subMenu = new ToolStripMenuItem(category, Icons.Category));
                    subMenu.DropDownItems.Add(item);
                }
            }
            row.ContextMenuStrip = menu;
        }

        public void OnResponse<T>(IIdentity sender, T msg) where T : IResponse
        {
            BeginInvoke((Action)(() =>
            {
                foreach (var handler in handlers.Reverse())
                {
                    var dumper = handler as IDumper;
                    if (dumper != null)
                        dumper.Dump = receiverDump1;
                    var contacter = handler as IAddresser;
                    if (contacter != null)
                        contacter.Book = this;
                    var publisher = handler as IPublisher;
                    if (publisher != null)
                        publisher.Pub = Publish;
                    foreach (var proc in handler.GetType().GetMethods())
                    {
                        if (!proc.Name.Equals("Process"))
                            continue;
                        var argType = proc.GetParameters().Last().ParameterType;
                        if (!argType.IsAssignableFrom(msg.GetType()))
                            continue;
                        var res = proc.Invoke(handler, new object[] { sender, msg });
                        if ((res + "").Equals(ResponseStatus.Handled + ""))
                            return;
                    }
                }
            }));
        }

        void Publish(Guid id, IMessage msg)
            => Client.Publish(Unicast.Of(id), msg);

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            contactList.Clear();
            Client.Dispose();
            Client = null;
        }

        public void AddOrUpdate(Contact contact)
        {
            contactList.Add(contact);
        }
    }
}