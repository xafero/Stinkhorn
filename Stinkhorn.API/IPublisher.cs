using System;

namespace Stinkhorn.API
{
    public interface IPublisher
    {
        Action<Guid, IMessage> Pub { set; }
    }
}