using System;

namespace Stinkhorn.Comm
{
    public interface IBroker : IId<Identity>, IDisposable, IPubSubBroker
    {
        void Open(string uri = null);

        void Close();
    }
}