using Stinkhorn.API;
using System.Collections.Generic;

namespace Stinkhorn.Demo.API
{
    [ResponseDesc]
    public class ScreenshotResponse : IResponse
    {
        public IList<IImage> Screenshots { get; set; }
    }
}