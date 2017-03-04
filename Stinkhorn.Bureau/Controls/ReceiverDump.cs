using System.Windows.Forms;
using Stinkhorn.Common;

namespace Stinkhorn.Bureau.Controls
{
    public partial class ReceiverDump : UserControl
    {
        public int MaxFlow { get; } = 6;

        public ReceiverDump()
        {
            InitializeComponent();
        }

        public void Receive<T>(IEnvelope<T> msg)
        {
            var body = msg.Body;
            var lastIndex = flowLayout.Controls.Count - 1;
            if (flowLayout.Controls.Count >= MaxFlow)
                flowLayout.Controls.RemoveAt(lastIndex);
            var ctrl = new ResponseControl(body);
            flowLayout.Controls.Add(ctrl);
            flowLayout.Controls.SetChildIndex(ctrl, 0);
            Invalidate(true);
        }
    }
}