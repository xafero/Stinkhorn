
namespace Stinkhorn.API
{
    public class PowerRequest : IRequest
    {
        public PowerAction Action { get; set; }
        public bool Force { get; set; }
    }

    public class PowerResponse : IResponse
    {
        public bool ReturnVal { get; set; }
    }

    public enum PowerAction
    {
        LockWorkStation,
        LogOff,
        Reboot,
        Sleep,
        Hibernate,
        Shutdown,
        Abort
    }
}