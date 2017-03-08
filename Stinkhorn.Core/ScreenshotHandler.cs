using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using Stinkhorn.API;

using ImageFormat = System.Drawing.Imaging.ImageFormat;
using SProcess = System.Diagnostics.Process;
using System;
using System.Linq;
using System.IO;

namespace Stinkhorn.Core
{
    class ScreenshotHandler
        : IMessageHandler<ScreenshotRequest, ScreenshotResponse>
    {
        public ScreenshotResponse Process(ScreenshotRequest input)
        {
            var images = new List<IImage>();
            var screens = Screen.AllScreens;
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
            }
            catch (NotImplementedException)
            {
                var tmpFiles = screens.Select(s => Path.GetTempFileName()).ToArray();
                var files = string.Join(" ", tmpFiles);
                using (var proc = SProcess.Start("screencapture", $"-C -tpng -x {files}"))
                    proc.WaitForExit(5 * 1000);
                foreach (var tmpFile in tmpFiles)
                {
                    var img = Image.FromFile(tmpFile);
                    var bytes = img.ToBytes(ImageFormat.Png);
                    images.Add(new PngImage(bytes, img.Width, img.Height));
                    File.Delete(tmpFile);
                }
            }
            return new ScreenshotResponse { Screenshots = images };
        }
    }
}