using Mono.Addins;
using Stinkhorn.API;
using Stinkhorn.Bureau.Context;
using Stinkhorn.Comm;

namespace Stinkhorn.Bureau.Handlers
{
    [Extension]
    public class GenericHandler : IResponseHandler<IResponse, IIdentity>
        , IDumper
    {
        public IDump Dump { private get; set; }

        public ResponseStatus Process(IIdentity sender, IResponse msg)
        {
            Dump.Receive(msg, sender.Uni);
            return ResponseStatus.Handled;
        }
    }
}