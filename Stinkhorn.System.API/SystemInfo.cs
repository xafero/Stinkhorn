namespace Stinkhorn.System.API
{
    public class SystemInfo
    {
        public string UserName { get; set; }

        public string HostName { get; set; }

        public Architecture Arch { get; set; }

        public Platform Platform { get; set; }

        public string Version { get; set; }

        public string Locale { get; set; }

        public string Encoding { get; set; }

        public int? CPUs { get; set; }

        public Endianness Endianness { get; set; }

        public OSType Type { get; set; }

        public string Edition { get; set; }

        public string Product { get; set; }

        public string Release { get; set; }
    }
}