using System;
using System.Net;
using System.Reflection;

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

        public static MethodInfo GenericMe(this Type type, string name, Type param)
            => type.GetMethod(name).MakeGenericMethod(param);
    }
}