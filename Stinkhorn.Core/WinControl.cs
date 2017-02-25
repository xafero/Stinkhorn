using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Stinkhorn.Core
{
    static class WinControl
    {
        [DllImport("user32.dll")]
        public static extern bool LockWorkStation();

        [DllImport("PowrProf.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);

        public static bool Hibernate()
        {
            if (SetSuspendState(true, true, true))
                StartRunDLL("powrprof.dll,SetSuspendState 1,1,1");
            return true;
        }

        public static bool Sleep()
        {
            if (SetSuspendState(false, true, true))
                StartRunDLL("powrprof.dll,SetSuspendState 0,1,1");
            return true;
        }

        static void StartRunDLL(params string[] args)
        {
            var info = new ProcessStartInfo
            {
                FileName = "rundll32",
                Arguments = string.Join(" ", args),
                UseShellExecute = true,
                Verb = "runas"
            };
            Process.Start(info).WaitForExit();
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        const ushort EWX_LOGOFF = 0;
        const ushort EWX_POWEROFF = 0x00000008;
        const ushort EWX_REBOOT = 0x00000002;
        const ushort EWX_RESTARTAPPS = 0x00000040;
        const ushort EWX_SHUTDOWN = 0x00000001;
        const ushort EWX_FORCE = 0x00000004;

        public static void Shutdown(bool force = false)
        {
            if (!ExitWindowsEx(EWX_SHUTDOWN | (uint)(force ? EWX_FORCE : 0) | EWX_POWEROFF, 0))
                StartShutdown("-s", "-t 00", force ? "-f" : "");
        }

        public static void Reboot(bool force = false)
        {
            if (!ExitWindowsEx(EWX_REBOOT | (uint)(force ? EWX_FORCE : 0), 0))
                StartShutdown("-r", "-t 00", force ? "-f" : "");
        }

        public static void LogOff(bool force = false)
        {
            if (!ExitWindowsEx(EWX_LOGOFF | (uint)(force ? EWX_FORCE : 0), 0))
                StartShutdown("-l", "-t 00", force ? "-f" : "");
        }

        public static void AbortShutdown()
        {
            StartShutdown("-a");
        }

        static void StartShutdown(params string[] args)
        {
            var info = new ProcessStartInfo
            {
                FileName = "shutdown",
                Arguments = string.Join(" ", args),
                UseShellExecute = true,
                Verb = "runas"
            };
            Process.Start(info).WaitForExit();
        }
    }
}