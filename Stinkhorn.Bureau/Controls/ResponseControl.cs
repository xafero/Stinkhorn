using System.Windows.Forms;
using Stinkhorn.API;
using System.ComponentModel;

namespace Stinkhorn.Bureau.Controls
{
    public partial class ResponseControl : UserControl
    {
        IResponse rsp;

        public ResponseControl() : this(new ServeResponse())
        {
        }

        public ResponseControl(IResponse rsp)
        {
            InitializeComponent();
            this.rsp = rsp;
            groupBox1.Text = rsp.GetType().Name;
            TypeDescriptor.AddAttributes(rsp, new[] { new ReadOnlyAttribute(true) });
            propertyGrid1.SelectedObject = rsp;
        }
    }
}