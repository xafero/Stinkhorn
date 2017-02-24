using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;

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