using NUnit.Framework;
using Stinkhorn.Common;
using System;
using System.Threading;

namespace Stinkhorn.Test
{
    [TestFixture]
    public class CommunicationTest
    {
        void TestSomething(ITransfer transfer)
        {
            var server = new Thread(ServerLoop);
            server.Start(transfer);
            var client = new Thread(ClientLoop);
            client.Start(transfer);
            Thread.Sleep(30 * 1000);
            server.Join();
            client.Join();
        }

        [Test]
        public void TestBroadcast()
        {
            TestSomething(Broadcast.Of());
        }

        [Test]
        public void TestMulticast()
        {
            TestSomething(Multicast.Of<CommunicationTest>());
        }

        [Test]
        public void TestUnicast()
        {
            TestSomething(Unicast.Of(Guid.NewGuid()));
        }

        static void ClientLoop(object state)
        {
            var addr = (ITransfer)state;
            var client = new RabbitBroker();
            client.Open();
            Thread.Sleep(3 * 1000);
            client.Publish(addr, $"Hello, I am a client ({addr})");
        }

        static void ServerLoop(object state)
        {
            var addr = (ITransfer)state;
            var server = new RabbitBroker();
            server.Open();
            Thread.Sleep(2 * 1000);
			server.Subscribe<string>(addr, (s, m) => Console.WriteLine($"{s.Uni} => {m}"));
        }
    }
}