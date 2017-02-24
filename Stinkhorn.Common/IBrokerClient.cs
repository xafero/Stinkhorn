using System;

namespace Stinkhorn.Common
{
    public interface IBrokerClient : IDisposable
    {
        void Publish<T>(T message);

        void Subscribe<T>(Action<T> callback);
    }
}