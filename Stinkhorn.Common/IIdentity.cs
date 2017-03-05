
namespace Stinkhorn.Common
{
    public interface IIdentity
    {
        IBroadcast Broad { get; }

        IMulticast Multi { get; }

        IUnicast Uni { get; }
    }
}