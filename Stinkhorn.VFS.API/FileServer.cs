using System;

namespace Stinkhorn.VFS.API
{
    class FileServer : IDisposable
    {
        readonly FtpServer server;

        public FileServer(string host, int port, string user, string pass)
        {
            var fsFact = new VirtualFsFactory();
            var mbFact = new FixedMemberProvider(user, pass);
            var ass = typeof(FtpServer).Assembly;
            var commands = new AssemblyFtpCommandHandlerFactory(ass);
            server = new FtpServer(fsFact, mbFact, host, port, commands);
            server.Start();
        }

        public void Dispose()
        {
            server.Stop();
            server.Dispose();
        }
    }
}