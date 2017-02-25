using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using FubarDev.FtpServer;
using FubarDev.FtpServer.AccountManagement;
using FubarDev.FtpServer.AccountManagement.Anonymous;
using FubarDev.FtpServer.AuthTls;
using FubarDev.FtpServer.FileSystem.DotNet;
using Stinkhorn.API;

namespace Stinkhorn.Core
{
	class ServeHandler : IDisposable,
					IMessageHandler<ServeRequest, ServeResponse>
	{
        FtpServer Server { get; set; }

        public ServeResponse Process(ServeRequest req)
		{
			var cert = new X509Certificate2("myftp.pfx");
			AuthTlsCommandHandler.ServerCertificate = cert;
			var commands = new AssemblyFtpCommandHandlerFactory(
				typeof(FtpServer).Assembly, typeof(AuthTlsCommandHandler).Assembly);
			var mbFact = new AnonymousMembershipProvider(new NoValidation());
			var fsFact = new DotNetFileSystemProvider(req.Path);
			Server = new FtpServer(fsFact, mbFact, req.Host, req.Port, commands)
							{ DefaultEncoding = Encoding.ASCII };
			Server.Start();
			return new ServeResponse { Host = Server.ServerAddress, Port = Server.Port };
		}
		
		public void Dispose()
		{
			Server.Stop();
			Server.Dispose();
			Server = null;
		}
	}
}