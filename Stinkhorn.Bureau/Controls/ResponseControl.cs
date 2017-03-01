using System.Windows.Forms;
using Stinkhorn.API;
using Stinkhorn.Bureau.Utils;

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
            propertyGrid1.SetSelectedObject(rsp, readOnly:true);
        }
    }
}