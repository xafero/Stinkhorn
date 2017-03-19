using Stinkhorn.API;
using System;

namespace Stinkhorn.Script.API
{
    [Serializable]
    [RequestDesc(Category = nameof(Script))]
    public class RunRequest : IRequest
    {
        public ScriptKind Kind { get; set; }
        public string Code { get; set; }
    }

    [Serializable]
    [ResponseDesc]
    public class RunResponse : IResponse
    {
        public object Result { get; set; }
        public TimeSpan Duration { get; set; }
    }

    [Serializable]
    public enum ScriptKind
    {
        Expression,
        Method,
        Class
    }

    public interface IRunnable
    {
        object Run(params object[] args);
    }
}