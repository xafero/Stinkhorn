using Stinkhorn.API;

namespace Stinkhorn.Script.API
{
    [RequestDesc(Category = nameof(Script))]
    public class RunRequest : IRequest
    {
        public string Code { get; set; }
    }

    [ResponseDesc]
    public class RunResponse : IResponse
    {
        public string Result { get; set; }
    }
}