using System;

namespace Stinkhorn.Util
{
    public interface ISystemInfo
    {
        string UserName { get; }

        string HostName { get; }

        Architecture Arch { get; }

        Platform Platform { get; }

        Version Version { get; }

        string Locale { get; }

        string Encoding { get; }

        int CPUs { get; }

        Endianness Endianness { get; }

        OSType Type { get; }

        string Edition { get; }

        string Product { get; }

        string Release { get; }
    }
}