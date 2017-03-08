
using System;

namespace Stinkhorn.API
{
    public class InfoRequest : IRequest
    {
    }

    public class InfoResponse : IResponse
    {
        public ISystemInfo Result { get; set; }
    }

    public interface ISystemInfo
    {
        string UserName { get; }

        string HostName { get; }

        Architecture Arch { get; }

        Platform Platform { get; }

        string Version { get; }

        string Locale { get; }

        string Encoding { get; }

        int CPUs { get; }

        Endianness Endianness { get; }

        OSType Type { get; }

        string Edition { get; }

        string Product { get; }

        string Release { get; }
    }

    public enum Architecture
    {
        Unknown = 0,

        x64,

        x86
    }

    public enum Endianness
    {
        Unknown = 0,

        Big,

        Little
    }

    public enum Platform
    {
        Unknown = 0,

        MacOSX,

        Linux,

        Windows,

        Unix
    }

    public enum OSType
    {
        Unknown,

        Client,

        Server
    }
}