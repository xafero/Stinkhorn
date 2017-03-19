using CSScriptLibrary;
using Stinkhorn.API;
using Stinkhorn.Script.API;

namespace Stinkhorn.Script.Shared
{
    [ReqHandlerFilter]
    public class CSScriptHandler : IRequestHandler<RunRequest>
    {
        public IResponse Process(RunRequest input)
        {
            var code = input.Code.Trim();
            var txt = string.Format("object Run() => {0};", code);
            var res = CSScript.Evaluator.CreateDelegateRemotely<object>(txt);
            var rsp = new RunResponse
            {
                Result = res() + ""
            };
            res.UnloadOwnerDomain();
            return rsp;
        }
    }
}