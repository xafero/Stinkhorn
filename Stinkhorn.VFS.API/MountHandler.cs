using Mono.Addins;
using Stinkhorn.API;
using System;

namespace Stinkhorn.VFS.API
{
    [Extension]
    public class MountHandler : IResponseHandler<MountResponse, object>
        , IDisposable
    {
        IDisposable server;

        public ResponseStatus Process(object src, MountResponse msg)
        {
            var user = Guid.NewGuid().ToString("N").Substring(0, 5);
            var pass = Guid.NewGuid().ToString("N").Substring(0, 7);
            var host = "127.0.0.1";
            var port = 21;
            var url = $"ftp://{user}:{pass}@{host}:{port}";
            server = new FileServer(host, port, user, pass);
            return ResponseStatus.Handled;
        }

        public void Dispose()
        {
            server?.Dispose();
            server = null;
        }
    }
}