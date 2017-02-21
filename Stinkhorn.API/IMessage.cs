using System;
using System.Collections.Generic;
using System.Net;

namespace Stinkhorn.API
{
	public interface IMessage
	{
		
	}

	public interface IRequest : IMessage
	{
		
	}
	
	public interface IResponse : IMessage
	{
		
	}
	
	public interface IMessageHandler<I, O>
		where I : IRequest
		where O : IResponse
	{
		O Process(I input);
	}
}