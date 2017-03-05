
namespace Stinkhorn.Common
{
    public interface IMulticast : ITransfer
    {
        string Topic { get; }
    }
}