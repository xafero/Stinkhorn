﻿using Microsoft.Win32;
using Stinkhorn.API;
using Stinkhorn.System.API;
using System;
using static Stinkhorn.System.API.SystemExtensions;

namespace Stinkhorn.System.Win
{
    [ReqHandlerFilter(Platform = "Win%")]
    public class WinSystemHandler : IRequestHandler<InfoRequest>
    {
        public IResponse Process(InfoRequest input)
        {
            var info = PatchDefaults(new SystemInfo());
            info.Platform = Platform.Windows;
            var path = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";
            using (var folder = Registry.LocalMachine.OpenSubKey(path))
            {
                var ver = Environment.OSVersion.Version;
                var plat = Environment.OSVersion.Platform;
                if (plat == PlatformID.Win32NT && ver.Major == 6 && ver.Minor >= 2)
                {
                    var major = folder.GetValue("CurrentMajorVersionNumber");
                    var minor = folder.GetValue("CurrentMinorVersionNumber");
                    var build = folder.GetValue("CurrentBuildNumber");
                    info.Version = (new Version($"{major}.{minor}.{build}.0")) + "";
                }
                info.Product = folder.GetValue("ProductName") + "";
                info.Edition = folder.GetValue("EditionID") + "";
                info.Release = folder.GetValue("ReleaseID") + "";
                var installType = folder.GetValue("InstallationType") + "";
                info.Type = installType.Equals("Client") ? OSType.Client : OSType.Server;
            }
            return new InfoResponse { Result = info };
        }
    }
}