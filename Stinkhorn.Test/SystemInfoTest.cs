using NUnit.Framework;
using static NUnit.Framework.Assert;
using Stinkhorn.System.API;

namespace Stinkhorn.Test
{
    [TestFixture]
    public class SystemInfoTest
    {
        [Test]
        public void TestInfoWin10()
        {
            var sys = new SystemInfo();
            AreEqual(Architecture.x64, sys.Arch);
            AreEqual(8, sys.CPUs);
            AreEqual("Windows-1252", sys.Encoding);
            AreEqual(Endianness.Little, sys.Endianness);
            AreEqual(7, sys.HostName.Length);
            AreEqual("en-US", sys.Locale);
            AreEqual(Platform.Windows, sys.Platform);
            AreEqual(4, sys.UserName.Length);
            AreEqual("10.0.14393.0", sys.Version + "");
            AreEqual(OSType.Client, sys.Type);
            AreEqual("Professional", sys.Edition);
            AreEqual("Windows 10 Pro", sys.Product);
            AreEqual("1607", sys.Release);
        }
    }
}