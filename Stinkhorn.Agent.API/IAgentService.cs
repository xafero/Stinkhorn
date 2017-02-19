using System;
using System.ServiceModel;

namespace Stinkhorn.Agent.API
{
	[ServiceContract]
	public interface IAgentService
	{
		[OperationContract]
		string Invoke(string arg);
	}
}