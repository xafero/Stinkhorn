using Microsoft.Win32;
using Stinkhorn.API;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace Stinkhorn.Core
{
    class SystemInfoHandler : IDisposable,
        IMessageHandler<InfoRequest, InfoResponse>
    {
        public InfoResponse Process(InfoRequest input)
            => new InfoResponse { Result = new SystemInfo() };

        public void Dispose()
        {
        }
    }

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

        public string Version
        {
            get
            {
                var ver = Environment.OSVersion.Version;
                var plat = Environment.OSVersion.Platform;
                return (plat == PlatformID.Win32NT && ver.Major == 6 && ver.Minor >= 2 ? RegistryVersion : ver) + "";
            }
        }

        public string Locale => CultureInfo.InstalledUICulture.Name;

        public string Encoding => System.Text.Encoding.Default.WebName;

        public int CPUs => Environment.ProcessorCount;

        public Endianness Endianness => BitConverter.IsLittleEndian ? Endianness.Little : Endianness.Big;

        OSType WinType
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

        static IEnumerable<string> ReadProcess(string cmd, params string[] args)
        {
            using (var proc = Process.Start(new ProcessStartInfo
            {
                FileName = cmd,
                Arguments = string.Join(" ", args),
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }))
            {
                while (!proc.StandardOutput.EndOfStream)
                    yield return proc.StandardOutput.ReadLine().Trim();
            }
        }

        OSType MacType
        {
            get
            {
                var text = ReadProcess("serverinfo", "--software").Single();
                return text.Contains("NOT") ? OSType.Client : OSType.Server;
            }
        }

        OSType LinuxType
        {
            get
            {
                var text = ReadProcess("uname", "-r").Single();
                return text.Contains("generic") ? OSType.Client : OSType.Server;
            }
        }

        public OSType Type
        {
            get
            {
                if (Platform == Platform.MacOSX)
                    return MacType;
                if (Platform == Platform.Linux)
                    return LinuxType;
                return WinType;
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
                if (Platform == Platform.Linux)
                    return ReadProcess("lsb_release", "-c").Single().Split(':').Last().Trim();
                using (var folder = CurrentVersionKey)
                    return folder.GetValue("EditionID") + "";
            }
        }

        public string Product
        {
            get
            {
                if (Platform == Platform.Linux)
                    return ReadProcess("lsb_release", "-d").Single().Split(':').Last().Trim();
                using (var folder = CurrentVersionKey)
                    return folder.GetValue("ProductName") + "";
            }
        }

        public string Release
        {
            get
            {
                if (Platform == Platform.Linux)
                    return ReadProcess("lsb_release", "-r").Single().Split(':').Last().Trim();
                using (var folder = CurrentVersionKey)
                    return folder.GetValue("ReleaseID") + "";
            }
        }

        RegistryKey CurrentVersionKey
            => Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
    }
}