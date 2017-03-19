using Stinkhorn.API;
using Stinkhorn.Demo.API;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Stinkhorn.Demo.BSD
{
    [ReqHandlerFilter(Platform = "Unix", HasVar = "SYSCTL_%")]
    public class BsdDemoHandler : IRequestHandler<ScreenshotRequest>
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