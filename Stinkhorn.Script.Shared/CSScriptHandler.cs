using CSScriptLibrary;
using Stinkhorn.API;
using Stinkhorn.Script.API;
using System;

namespace Stinkhorn.Script.Shared
{
    [ReqHandlerFilter]
    public class CSScriptHandler : IRequestHandler<RunRequest>
    {
        public IResponse Process(RunRequest input)
        {
            var code = input.Code.Trim();
            var args = new object[0];
            var start = DateTime.Now;
            object res;
            switch (input.Kind)
            {
                case ScriptKind.Class:
                    var obj = CSScript.Evaluator.LoadCodeRemotely<IRunnable>(code);
                    res = obj.Run(args);
                    break;
                case ScriptKind.Method:
                    var meth = CSScript.Evaluator.LoadMethodRemotely<IRunnable>(code);
                    res = meth.Run(args);
                    break;
                case ScriptKind.Expression:
                default:
                    var txt = string.Format("object Run(params object[] args) => {0};", code);
                    var dlgt = CSScript.Evaluator.CreateDelegateRemotely<object>(txt);
                    res = dlgt(new[] { args });
                    break;
            }
            var rsp = new RunResponse
            {
                Duration = DateTime.Now - start,
                Result = res + ""
            };
            res.UnloadOwnerDomain();
            return rsp;
        }
    }
}