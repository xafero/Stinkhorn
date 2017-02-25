using Stinkhorn.API;
using Stinkhorn.Bureau.Utils;
using System;
using System.Windows.Forms;

namespace Stinkhorn.Bureau.Context
{
    public class ContextMenuItem
    {
        IContextAction context;

        public ContextMenuItem(IContextAction context)
        {
            this.context = context;
        }

        public static implicit operator ToolStripItem(ContextMenuItem item)
        {
            var ctx = item.context;
            EventHandler onclick = null;
            var icon = ctx.Icon.ToDrawingImage();
            return new ToolStripButton(ctx.Title, icon, onclick);
        }
    }
}