using Stinkhorn.API;
using Stinkhorn.Comm;

namespace Stinkhorn.Bureau.Context
{
    public class Contact : HelloMessage
    {
        public Contact(IIdentity sender, HelloMessage env)
        {
            Machine = env.Machine;
            User = env.User;
            Local = env.Local;
            Remote = env.Remote;
            Id = (Unicast)sender.Uni;
        }

        public Unicast Id { get; }
    }
}