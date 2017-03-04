using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using Stinkhorn.Common;
using Stinkhorn.API;

namespace Stinkhorn.Test
{
    [TestFixture]
    public class CommunicationTest
    {
        [Test]
        public void TestBroadcast()
        {
            using (var client1 = new BrokerClient())
            using (var client2 = new BrokerClient())
            {
                client2.Subscribe<HelloMessage>(m =>
                {
                    Assert.Fail(m + "");
                });
                client1.Publish(new HelloMessage());
            }
        }

        [Test]
        public void TestMulticast()
        {

        }

        [Test]
        public void TestUnicast()
        {

        }
    }
}