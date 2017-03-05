using System;

namespace Stinkhorn.Common
{
    public interface IPubSubBroker
    {
        void Publish<T>(ITransfer addr, T message);

        void Subscribe<T>(ITransfer addr, Action<IIdentity, T> callback);
    }
}