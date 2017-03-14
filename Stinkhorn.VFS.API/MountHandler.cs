using Mono.Addins;
using Stinkhorn.API;
using System;
using System.Diagnostics;
using Proc = System.Diagnostics.Process;

namespace Stinkhorn.VFS.API
{
    [Extension]
    public class MountHandler : IResponseHandler<MountResponse, object>
        , IDisposable
    {
        FileServer server;

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
                Proc.Start("explorer", url);
            }
            InsertResponse(server, msg);
            return ResponseStatus.Handled;
        }

        void InsertResponse(FileServer server, MountResponse msg)
        {
            Debugger.Break(); throw new NotImplementedException();
        }

        public void Dispose()
        {
            server?.Dispose();
            server = null;
        }
    }
}