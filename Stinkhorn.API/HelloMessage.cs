using System;
using System.Collections.Generic;
using System.Net;

namespace Stinkhorn.API
{
	public class HelloMessage : IMessage
	{
		public string Local { get; set;	}
		public string Remote { get; set; }
	}
	
	public class ScreenshotRequest : IRequest
	{
	}
	
	public class ScreenshotResponse : IResponse
	{
		public IList<IImage> Screenshots { get; set; }
	}
	
	public class ServeRequest : IRequest
	{
		public string Path { get; set; }
		public string Host { get; set; }
		public int Port { get; set; }
	}
	
	public class ServeResponse : IResponse
	{
		public string Host { get; set; }
		public int Port { get; set; }
	}
}