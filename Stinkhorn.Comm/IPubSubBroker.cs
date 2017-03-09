using System;

namespace Stinkhorn.Comm
{
    public interface IPubSubBroker
    {
        void Publish<T>(ITransfer addr, T message);

        void Subscribe<T>(ITransfer addr, Action<IIdentity, T> callback);
    }
}