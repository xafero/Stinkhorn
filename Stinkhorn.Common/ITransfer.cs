
namespace Stinkhorn.Common
{
    public interface ITransfer
    {
        string Exchange { get; }

        string Type { get; }

        TransferKind Kind { get; }
    }
}