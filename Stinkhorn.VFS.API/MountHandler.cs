﻿using Mono.Addins;
using Stinkhorn.API;
using System;
using System.Collections.Generic;
using Proc = System.Diagnostics.Process;

namespace Stinkhorn.VFS.API
{
    [Extension]
    public class MountHandler : IResponseHandler<MountResponse, object>
        , IDisposable
    {
        FileServer server;
        public SortedDictionary<string, object> roots;

        public ResponseStatus Process(object src, MountResponse msg)
        {
            if (server == null)
            {
                var user = Guid.NewGuid().ToString("N").Substring(0, 5);
                var pass = Guid.NewGuid().ToString("N").Substring(0, 7);
                var host = "127.0.0.1";
                var port = 21;
                var url = $"ftp://{user}:{pass}@{host}:{port}";
                server = new FileServer(this, host, port, user, pass);
                roots = new SortedDictionary<string, object>();
                Proc.Start("explorer", url);
            }
            InsertResponse(src, msg);
            return ResponseStatus.Handled;
        }

        void InsertResponse(object src, MountResponse msg)
        {
            var folder = src + string.Empty;
            roots.Add(folder, msg.Drives);
        }

        public void Dispose()
        {
            roots?.Clear();
            roots = null;
            server?.Dispose();
            server = null;
        }
    }
}