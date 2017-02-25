using System;

namespace Stinkhorn.Common
{
    public interface IBrokerClient : IDisposable
    {
        void Publish<T>(T message, string target = null);

        void Subscribe<T>(Action<T> callback, string target = null);
    }
}