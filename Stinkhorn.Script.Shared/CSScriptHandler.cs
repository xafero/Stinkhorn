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
            var evaluator = Resolve(CSScript.Evaluator,
                "System", "System.Core", "System.IO",
                "System.Linq", "System.Data.ComponentModel",
                "System.Data.DataSetExtensions",
                "System.Xml", "System.Xml.Linq");
            var code = input.Code?.Trim();
            var args = new object[0];
            var start = DateTime.Now;
            object res;
            switch (input.Kind)
            {
                case ScriptKind.Class:
                    var obj = evaluator.LoadCodeRemotely<IRunnable>(code);
                    res = obj.Run(args);
                    break;
                case ScriptKind.Method:
                    var meth = evaluator.LoadMethodRemotely<IRunnable>(code);
                    res = meth.Run(args);
                    break;
                case ScriptKind.Expression:
                default:
                    var txt = string.Format("object Run(params object[] args) => {0};", code);
                    var dlgt = evaluator.CreateDelegateRemotely<object>(txt);
                    res = dlgt(new[] { args });
                    break;
            }
            var rsp = new RunResponse
            {
                Duration = DateTime.Now - start,
                Result = res
            };
            res.UnloadOwnerDomain();
            return rsp;
        }

        IEvaluator Resolve(IEvaluator eva, params string[] namesps)
        {
            IEvaluator res = eva;
            foreach (var namesp in namesps)
                res = res.ReferenceAssemblyByNamespace(namesp);
            return res;
        }
    }
}