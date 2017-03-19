using NUnit.Framework;
using Stinkhorn.Script.API;
using Stinkhorn.Script.Shared;

namespace Stinkhorn.Test
{
    [TestFixture]
    public class ScriptTest
    {
        [Test]
        public void TestRunExpression()
        {
            var handler = new CSScriptHandler();
            var rr = new RunRequest
            {
                Kind = ScriptKind.Expression,
                Code = @" 
                        System.Environment.OSVersion.Platform
                        "
            };
            var rsp = handler.Process(rr) as RunResponse;
            Assert.AreEqual("Win32NT", rsp.Result + "");
        }

        [Test]
        public void TestRunMethod()
        {
            var handler = new CSScriptHandler();
            var rr = new RunRequest
            {
                Kind = ScriptKind.Method,
                Code = @" 
                        public object Run(params object[] args)
                        {
                            return System.Environment.OSVersion.Platform;
                        }                        
                        "
            };
            var rsp = handler.Process(rr) as RunResponse;
            Assert.AreEqual("Win32NT", rsp.Result + "");
        }

        [Test]
        public void TestRunClass()
        {
            var handler = new CSScriptHandler();
            var rr = new RunRequest
            {
                Kind = ScriptKind.Class,
                Code = @"
                         using System;
                         using Stinkhorn.Script.API;

                         public class Example : MarshalByRefObject, IRunnable 
                         {
                            public object Run(params object[] args)
                            {
                                return System.Environment.OSVersion.Platform;
                            }
                         }
                        "
            };
            var rsp = handler.Process(rr) as RunResponse;
            Assert.AreEqual("Win32NT", rsp.Result + "");
        }
    }
}