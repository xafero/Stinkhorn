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

        public Version Version => Environment.OSVersion.Version;

        public string Locale => CultureInfo.InstalledUICulture.Name;

        public string Encoding => System.Text.Encoding.Default.WebName.ToUpperInvariant();

        public int CPUs => Environment.ProcessorCount;

        public Endianness Endianness => BitConverter.IsLittleEndian ? Endianness.Little : Endianness.Big;
    }
}