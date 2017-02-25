using System.Windows.Forms;
using Stinkhorn.API;

namespace Stinkhorn.Bureau.Controls
{
    public partial class RequestDialog : Form
    {
        private IRequest req;

        public RequestDialog() : this(new ServeRequest())
        {
        }

        public RequestDialog(IRequest req)
        {
            InitializeComponent();
            this.req = req;
            Text = req.GetType().Name;
            propertyGrid1.SelectedObject = req;
        }
    }
}