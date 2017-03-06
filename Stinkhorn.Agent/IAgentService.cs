using System;

namespace Stinkhorn.Agent
{
    interface IAgentService : IDisposable
    {
        void Start();

        void Stop();
    }
}