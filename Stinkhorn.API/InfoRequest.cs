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

    public class SimpleInfo : ISystemInfo
    {
        readonly ISystemInfo sys;

        public SimpleInfo(ISystemInfo systemInfo)
        {
            sys = systemInfo;
            Arch = sys.Arch;
            CPUs = sys.CPUs;
            Edition = sys.Edition;
            Encoding = sys.Encoding;
            Endianness = sys.Endianness;
            HostName = sys.HostName;
            Locale = sys.Locale;
            Platform = sys.Platform;
            Product = sys.Product;
            Release = sys.Release;
            Type = sys.Type;
            UserName = sys.UserName;
            Version = sys.Version;
        }

        public SimpleInfo() { }

        public Architecture Arch { get; set; }
        public int CPUs { get; set; }
        public string Edition { get; set; }
        public string Encoding { get; set; }
        public Endianness Endianness { get; set; }
        public string HostName { get; set; }
        public string Locale { get; set; }
        public Platform Platform { get; set; }
        public string Product { get; set; }
        public string Release { get; set; }
        public OSType Type { get; set; }
        public string UserName { get; set; }
        public string Version { get; set; }
    }
}