using System.Drawing;

namespace Stinkhorn.Bureau.Utils
{
    public static class WinFormsExts
    {
        public static Icon ToIcon(this Image image)
            => Icon.FromHandle(new Bitmap(image).GetHicon());
    }
}