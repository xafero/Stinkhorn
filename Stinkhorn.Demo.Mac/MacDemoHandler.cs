using Stinkhorn.API;
using System.Linq;
using Stinkhorn.Demo.API;

using System.Collections.Generic;
using System.Drawing;

using ImageFormat = System.Drawing.Imaging.ImageFormat;
using SProcess = System.Diagnostics.Process;
using System.IO;
using System.Windows.Forms;
using Stinkhorn.Util;

namespace Stinkhorn.Demo.Mac
{
    [ReqHandlerFilter(Platform = "Unix", HasVar = "Apple_%")]
    [ReqHandlerFilter(Platform = "MacOSX")]
    public class MacDemoHandler : IRequestHandler<ScreenshotRequest>
    {
        public IResponse Process(ScreenshotRequest input)
        {
            var images = new List<IImage>();
            var screens = Screen.AllScreens;
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
            return new ScreenshotResponse { Screenshots = images };
        }
    }
}