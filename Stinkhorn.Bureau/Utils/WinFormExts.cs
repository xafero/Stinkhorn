using Stinkhorn.API;
using System.Drawing;
using System.IO;

namespace Stinkhorn.Bureau.Utils
{
    public static class WinFormsExts
    {
        public static Image ToDrawingImage(this IImage img)
            => img.Bytes.ToDrawingImage();

        public static Image ToDrawingImage(this byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
                return Image.FromStream(stream);
        }

        public static Icon ToIcon(this Image image)
            => Icon.FromHandle(new Bitmap(image).GetHicon());
    }
}