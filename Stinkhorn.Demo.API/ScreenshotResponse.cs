using Stinkhorn.API;
using System.Collections.Generic;

namespace Stinkhorn.Demo.API
{
    public class ScreenshotResponse : IResponse
    {
        public IList<IImage> Screenshots { get; set; }
    }
}