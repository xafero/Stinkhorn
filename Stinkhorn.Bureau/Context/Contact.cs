using System;
using Stinkhorn.API;

namespace Stinkhorn.Bureau.Context
{
    class Contact : HelloMessage
    {
        public Contact(IEnvelope<HelloMessage> env)
        {
            Machine = env.Body.Machine;
            User = env.Body.User;
            Local = env.Body.Local;
            Remote = env.Body.Remote;
            SenderId = env.Body.SenderId;
            Id = env.Sender.Id.Value;
        }

        public Guid Id { get; }
    }
}