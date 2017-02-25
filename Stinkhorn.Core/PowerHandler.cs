using Stinkhorn.API;
using System;

namespace Stinkhorn.Core
{
    class PowerHandler : IMessageHandler<PowerRequest, PowerResponse>
    {
        public PowerResponse Process(PowerRequest req)
        {
            bool retVal;
            switch (req.Action)
            {
                case PowerAction.Hibernate:
                    retVal = WinControl.Hibernate();
                    break;
                case PowerAction.LockWorkStation:
                    retVal = WinControl.LockWorkStation();
                    break;
                case PowerAction.LogOff:
                    WinControl.LogOff(req.Force);
                    retVal = true;
                    break;
                case PowerAction.Reboot:
                    WinControl.Reboot(req.Force);
                    retVal = true;
                    break;
                case PowerAction.Shutdown:
                    WinControl.Shutdown(req.Force);
                    retVal = true;
                    break;
                case PowerAction.Sleep:
                    retVal = WinControl.Sleep();
                    break;
                case PowerAction.Abort:
                    WinControl.AbortShutdown();
                    retVal = true;
                    break;
                default:
                    throw new NotImplementedException(req.Action + "?!");
            }
            return new PowerResponse { ReturnVal = retVal };
        }
    }
}