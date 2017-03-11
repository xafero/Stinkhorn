using Stinkhorn.API;
using Stinkhorn.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace Stinkhorn.Demo.API
{
    public static class DemoExtensions
    {
        public static bool Screenshot(IEnumerable<Screen> screens, ICollection<IImage> images)
        {
            try
            {
                foreach (var screen in screens)
                {
                    var width = screen.Bounds.Width;
                    var height = screen.Bounds.Height;
                    using (var img = new Bitmap(width, height))
                    {
                        var size = new Size(img.Width, img.Height);
                        var graph = Graphics.FromImage(img);
                        var x = screen.Bounds.X;
                        var y = screen.Bounds.Y;
                        graph.CopyFromScreen(x, y, 0, 0, size);
                        var bytes = img.ToBytes(ImageFormat.Png);
                        images.Add(new PngImage(bytes, img.Width, img.Height));
                    }
                }
                return true;
            }
            catch (NotImplementedException)
            {
                // In Mono for Mac (for example), they don't want that!
                return false;
            }
        }
    }
}