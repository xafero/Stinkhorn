using Microsoft.Win32;
using System;
using System.Globalization;
using System.Linq;

namespace Stinkhorn.Util
{
    public class SystemInfo : ISystemInfo
    {
        public string UserName => Environment.UserName;

        public string HostName => Environment.MachineName;

        public Architecture Arch => Environment.Is64BitOperatingSystem ? Architecture.x64 : Architecture.x86;

        public Platform Platform
        {
            get
            {
                var platform = Environment.OSVersion.Platform;
                var env = Environment.GetEnvironmentVariables();
                var isMac = env.Keys.OfType<string>().Any(key => key.Contains("Apple_"));
                if (isMac && platform != PlatformID.MacOSX)
                    platform = PlatformID.MacOSX;
                switch (platform)
                {
                    case PlatformID.MacOSX:
                        return Platform.MacOSX;
                    case PlatformID.Unix:
                        var isLinux = env.Keys.OfType<string>().Any(key => key.Contains("_LINUX_"));
                        return isLinux ? Platform.Linux : Platform.Unix;
                    default:
                        return Platform.Windows;
                }
            }
        }

        public Version Version
        {
            get
            {
                var ver = Environment.OSVersion.Version;
                var plat = Environment.OSVersion.Platform;
                return plat == PlatformID.Win32NT && ver.Major == 6 && ver.Minor >= 2 ? RegistryVersion : ver;
            }
        }

        public string Locale => CultureInfo.InstalledUICulture.Name;

        public string Encoding => System.Text.Encoding.Default.WebName;

        public int CPUs => Environment.ProcessorCount;

        public Endianness Endianness => BitConverter.IsLittleEndian ? Endianness.Little : Endianness.Big;

        public OSType Type
        {
            get
            {
                using (var folder = CurrentVersionKey)
                {
                    var installType = folder.GetValue("InstallationType") + "";
                    return installType.Equals("Client") ? OSType.Client : OSType.Server;
                }
            }
        }

        Version RegistryVersion
        {
            get
            {
                using (var folder = CurrentVersionKey)
                {
                    var major = folder.GetValue("CurrentMajorVersionNumber");
                    var minor = folder.GetValue("CurrentMinorVersionNumber");
                    var build = folder.GetValue("CurrentBuildNumber");
                    return new Version($"{major}.{minor}.{build}.0");
                }
            }
        }

        public string Edition
        {
            get
            {
                using (var folder = CurrentVersionKey)
                    return folder.GetValue("EditionID") + "";
            }
        }

        public string Product
        {
            get
            {
                using (var folder = CurrentVersionKey)
                    return folder.GetValue("ProductName") + "";
            }
        }

        public string Release
        {
            get
            {
                using (var folder = CurrentVersionKey)
                    return folder.GetValue("ReleaseID") + "";
            }
        }

        RegistryKey CurrentVersionKey
            => Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
    }
}