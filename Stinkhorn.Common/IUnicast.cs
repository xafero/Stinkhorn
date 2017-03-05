using System;

namespace Stinkhorn.Common
{
    public interface IUnicast : ITransfer
    {
        Guid Id { get; }
    }
}