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
            var code = input.Code.Trim();



            return new RunResponse
            {
                Result = 42 + ""
            };
        }
    }
}