using System.Collections.Generic;

namespace Stinkhorn.API
{
    public class ScreenshotRequest : IRequest
	{
	}

    public class ScreenshotResponse : IResponse
    {
        public IList<IImage> Screenshots { get; set; }
    }
}