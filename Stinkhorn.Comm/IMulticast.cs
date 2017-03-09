
namespace Stinkhorn.Comm
{
    public interface IMulticast : ITransfer
    {
        string Topic { get; }
    }
}