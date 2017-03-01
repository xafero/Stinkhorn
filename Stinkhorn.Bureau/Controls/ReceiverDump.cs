using System.Windows.Forms;
using Stinkhorn.API;

namespace Stinkhorn.Bureau.Controls
{
    public partial class ReceiverDump : UserControl
    {
        public ReceiverDump()
        {
            InitializeComponent();
        }

        public void Receive(IResponse response)
        {
            flowLayout.Controls.Add(new ResponseControl(response));
        }
    }
}