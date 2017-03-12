using Microsoft.Win32;
using Stinkhorn.API;
using System;

namespace Stinkhorn.System.Win
{
    public class ServeRequest : IRequest
    {
        public string Path { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }

    public class ServeResponse : IResponse
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }

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

    public class PowerHandler : IRequestHandler<PowerRequest>
    {
        public IResponse Process(PowerRequest req)
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

    public class RegistryRequest : IRequest
    {
        public RegistryRoot Root { get; set; }
        public string Path { get; set; }
        public string Key { get; set; }
        public object Value { get; set; }
    }

    public class RegistryResponse : IResponse
    {
        public object Value { get; set; }
    }

    public enum RegistryRoot
    {
        CurrentUser,
        LocalMachine,
        ClassesRoot,
        Users,
        PerformanceData,
        CurrentConfig,
        DynData
    }

    public class RegistryHandler : IRequestHandler<RegistryRequest>
    {
        public IResponse Process(RegistryRequest input)
        {
            var root = (RegistryKey)typeof(Registry).GetField(input.Root + "").GetValue(null);
            var writable = input.Value != null;
            var path = input.Path.Trim();
            using (var key = root.OpenSubKey(path, writable))
            {
                var myKey = input.Key.Trim();
                var ret = new RegistryResponse { Value = key.GetValue(myKey) };
                if (writable)
                    key.SetValue(myKey, input.Value);
                return ret;
            }
        }
    }
}