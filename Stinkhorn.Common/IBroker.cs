using System;

namespace Stinkhorn.Common
{
    public interface IId<T>
    {
        T Id { get; }
    }

    public interface IPubSubBroker
    {
        void Publish<T>(ITransfer addr, T message);

        void Subscribe<T>(ITransfer addr, Action<IIdentity, T> callback);
    }

    public interface IBroker : IId<Identity>, IDisposable, IPubSubBroker
    {
        void Open(string uri = null);

        void Close();
    }
}