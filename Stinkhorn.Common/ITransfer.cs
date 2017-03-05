using System;

namespace Stinkhorn.Common
{
    public interface ITransfer
    {
        string Exchange { get; }
        string Type { get; }
    }

    public interface IBroadcast : ITransfer
    {
    }

    public interface IMulticast : ITransfer
    {
        string Topic { get; }
    }

    public interface IUnicast : ITransfer
    {
        Guid Id { get; }
    }
}