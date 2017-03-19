using Stinkhorn.API;
using Stinkhorn.Script.API;
using System;
using System.Diagnostics;

namespace Stinkhorn.Script.Shared
{
    [ReqHandlerFilter]
    public class CSScriptHandler : IRequestHandler<RunRequest>
    {
        public IResponse Process(RunRequest input)
        {
            Debugger.Break();
            throw new NotImplementedException();
        }
    }
}