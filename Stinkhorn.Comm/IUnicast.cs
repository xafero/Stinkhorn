using System;

namespace Stinkhorn.Comm
{
    public interface IUnicast : ITransfer
    {
        Guid Id { get; }
    }
}