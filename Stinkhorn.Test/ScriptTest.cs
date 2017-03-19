using NUnit.Framework;
using Stinkhorn.Script.API;
using Stinkhorn.Script.Shared;

namespace Stinkhorn.Test
{
    [TestFixture]
    public class ScriptTest
    {
        [Test]
        public void TestRunScript()
        {
            var handler = new CSScriptHandler();
            var rr = new RunRequest
            {
                Code = @" 
                        System.Environment.OSVersion.Platform
                        "
            };
            var rsp = handler.Process(rr) as RunResponse;
            Assert.AreEqual("42", rsp.Result);
        }
    }
}