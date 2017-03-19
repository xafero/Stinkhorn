using NUnit.Framework;
using Stinkhorn.VFS.API;
using Stinkhorn.VFS.Shared;
using System.Linq;
using static System.Environment;

namespace Stinkhorn.Test
{
    [TestFixture]
    public class FileTest
    {
        [Test]
        public void TestMount()
        {
            var handler = new NetMountHandler();
            var mr = new MountRequest
            {
                Type = FsType.Auto,
                Source = SpecialFolder.MyComputer,
                Target = "/drives"
            };
            var rr = handler.Process(mr) as MountResponse;
            Assert.AreEqual("MyComputer", rr.Source + "");
            Assert.AreEqual("/drives", rr.Target);
            Assert.AreEqual(0, rr.Files.Count());
            Assert.IsTrue(rr.Folders.Count() >= 1, "No folders ?!");
        }
    }
}