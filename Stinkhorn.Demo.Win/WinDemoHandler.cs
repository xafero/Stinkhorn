using Stinkhorn.API;
using Stinkhorn.Demo.API;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Stinkhorn.Demo.Win
{
    [ReqHandlerFilter(Platform = "Win%")]
    public class WinDemoHandler : IRequestHandler<ScreenshotRequest>
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