using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Stinkhorn.Comm
{
    public class Identity : IIdentity
    {
        public IBroadcast Broad { get; }
        public IMulticast Multi { get; }
        public IUnicast Uni { get; }

        public Identity(Guid id, params string[] roles)
        {
            Broad = Broadcast.Of();
            Multi = Multicast.Of(roles.First());
            Uni = Unicast.Of(id);
        }

        public override string ToString() => Uni.ToString();

        const string SenderId = "SenderId";
        const string SenderRoles = "SenderRoles";
        const char Separator = ';';

        public static implicit operator Dictionary<string, object>(Identity id)
            => new Dictionary<string, object>
            {
                {SenderId, id.Uni.ToString()},
                {SenderRoles, id.Multi.ToString()}
            };

        public static implicit operator Identity(Dictionary<string, object> dict)
            => new Identity(Guid.Parse(ToString(dict[SenderId])),
                ToString(dict[SenderRoles]).Split(Separator));

        static string ToString(object obj)
            => Encoding.UTF8.GetString((byte[])obj);
    }
}