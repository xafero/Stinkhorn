using Mono.Addins;
using Stinkhorn.API;
using System.Collections.Generic;

namespace Stinkhorn.Demo.API
{
    [Extension]
    public class ScreenshotResponse : IResponse
    {
        public IList<IImage> Screenshots { get; set; }
    }
}