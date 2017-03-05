using System;

namespace Stinkhorn.Common
{
    public interface IBroker : IId<Identity>, IDisposable, IPubSubBroker
    {
        void Open(string uri = null);

        void Close();
    }
}