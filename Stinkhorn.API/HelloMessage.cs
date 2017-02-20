using System;
using System.Collections.Generic;
using System.Net;

namespace Stinkhorn.API
{
	public class HelloMessage : IMessage
	{
		public string Local {
			get; set;
		}

		public string Remote {
			get; set;
		}
	}
}