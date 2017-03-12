using Stinkhorn.API;
using Stinkhorn.Demo.API;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Stinkhorn.Demo.Linux
{
    [ReqHandlerFilter(Platform = "Unix", HasVar = "%_LINUX_% INFINALITY_%")]
    public class LinuxDemoHandler : IRequestHandler<ScreenshotRequest>
    {
        public IResponse Process(ScreenshotRequest input)
        {
            var images = new List<IImage>();
            var screens = Screen.AllScreens;
            DemoExtensions.Screenshot(screens, images);
            return new ScreenshotResponse { Screenshots = images };
        }
    }
}