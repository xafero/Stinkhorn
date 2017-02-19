using System;
using System.ServiceModel;

namespace Stinkhorn.Agent.API
{
	[ServiceContract]
	public interface IStinkAgentService
	{
		[OperationContract]
		string Invoke(string arg);
	}
}