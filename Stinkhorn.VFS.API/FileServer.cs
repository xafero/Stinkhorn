using FubarDev.FtpServer;
using System;

namespace Stinkhorn.VFS.API
{
    class FileServer : IDisposable
    {
        readonly FtpServer server;

        public FileServer(MountHandler parent, string host, int port, string user, string pass)
        {
            Parent = parent;
            var fsFact = new VirtualFsFactory(this);
            var mbFact = new FixedMemberProvider(user, pass);
            var ass = typeof(FtpServer).Assembly;
            var commands = new AssemblyFtpCommandHandlerFactory(ass);
            server = new FtpServer(fsFact, mbFact, host, port, commands);
            server.Start();
        }

        public MountHandler Parent { get; }

        public void Dispose()
        {
            server.Stop();
            server.Dispose();
        }
    }
}