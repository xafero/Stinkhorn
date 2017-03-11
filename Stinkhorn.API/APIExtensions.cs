using System;
using System.Net;

namespace Stinkhorn.API
{
    public static class APIExtensions
    {
        public static string ToShortString(this DnsEndPoint endpoint)
            => $"{endpoint.Host}:{endpoint.Port}";

        public static string ToIdString(this Guid guid)
            => guid.ToString("N");

        public static string ToIdString(this Guid? guid)
            => guid == null ? null : guid.Value.ToIdString();
    }
}