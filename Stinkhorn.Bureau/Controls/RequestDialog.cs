using System.Drawing;
using System.Windows.Forms;
using Stinkhorn.API;
using Stinkhorn.Bureau.Utils;

namespace Stinkhorn.Bureau.Controls
{
    public partial class RequestDialog : Form
    {
        IRequest req;

        public RequestDialog() : this(new ServeRequest(), null)
        {
        }

        public RequestDialog(IRequest req, Image image)
        {
            InitializeComponent();
            this.req = req;
            Text = req.GetType().Name;
            Icon = image?.ToIcon();
            propertyGrid1.SetSelectedObject(req);
        }

        void button1_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        void button2_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}