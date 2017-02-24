using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;

namespace Stinkhorn.API
{
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