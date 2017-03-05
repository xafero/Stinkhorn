
namespace Stinkhorn.Common
{
    public interface IFault
    {
        FaultCode Code { get; }

        string Description { get; }

        string Details { get; }
    }
}