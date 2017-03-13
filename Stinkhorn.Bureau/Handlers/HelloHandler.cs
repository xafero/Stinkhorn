using Mono.Addins;
using Stinkhorn.API;
using Stinkhorn.Bureau.Context;
using Stinkhorn.Comm;

namespace Stinkhorn.Bureau.Handlers
{
    [Extension]
    public class HelloHandler : IResponseHandler<HelloMessage, IIdentity>
        , IAddresser
    {
        public IAddressBook Book { private get; set; }

        public ResponseStatus Process(IIdentity sender, HelloMessage msg)
        {
            Book.AddOrUpdate(new Contact(sender, msg));
            return ResponseStatus.Handled;
        }
    }
}