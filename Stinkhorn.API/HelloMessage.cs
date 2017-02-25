﻿using System;
using System.Net;

namespace Stinkhorn.API
{
    public class HelloMessage : IMessage
    {
        public string Local { get; set; }

        public string Remote { get; set; }

        public string SenderId { get; set; }
    }

    public static class APIExtensions
    {
        public static string ToShortString(this DnsEndPoint endpoint)
            => $"{endpoint.Host}:{endpoint.Port}";

        public static string ToIdString(this Guid guid)
            => guid.ToString("N");
    }
}