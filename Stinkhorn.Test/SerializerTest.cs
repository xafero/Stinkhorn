using NUnit.Framework;
using Stinkhorn.Comm;

namespace Stinkhorn.Test
{
    [TestFixture]
    public class SerializerTest
    {
        [Test]
        public void TestJSON()
        {
            using (ISerializer seri = new JsonSerializer())
            {
                const int raw = 42;
                byte[] bytes = seri.Serialize(raw);
                int number = seri.Deserialize<int>(bytes);
                Assert.AreEqual(raw, number);
            }
        }
    }
}