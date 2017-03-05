using System.Windows.Forms;

namespace Stinkhorn.Bureau.Controls
{
    public partial class ReceiverDump : UserControl
    {
        public int MaxFlow { get; } = 6;

        public ReceiverDump()
        {
            InitializeComponent();
        }

        public void Receive<T>(T msg, object title = null)
        {
            var lastIndex = flowLayout.Controls.Count - 1;
            if (flowLayout.Controls.Count >= MaxFlow)
                flowLayout.Controls.RemoveAt(lastIndex);
            var ctrl = new ResponseControl(msg, title + string.Empty);
            flowLayout.Controls.Add(ctrl);
            flowLayout.Controls.SetChildIndex(ctrl, 0);
            Invalidate(true);
        }
    }
}