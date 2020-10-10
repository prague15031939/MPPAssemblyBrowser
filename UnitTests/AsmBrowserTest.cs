using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AsmBrowser;

namespace UnitTests
{
    [TestClass]
    public class AsmBrowserTest
    {
        public AssemblyBrowser browser;
        public BrowserResult result;

        [TestInitialize]
        public void Setup()
        {
            browser = new AssemblyBrowser();
        }

        [TestMethod]
        public void TestSimpleAssembly()
        {
            result = browser.Browse(@"Assemblies\Tracer.dll");
            Assert.AreEqual(result.Namespaces.Count, 1);
            Assert.AreEqual(result.Namespaces[0].Name, "Tracer");
            Assert.AreEqual(result.Namespaces[0].DataTypes.Count, 7);
            Assert.IsTrue(result.Namespaces[0].DataTypes.Exists(obj => obj.Name == "TracerMain"));
            Assert.IsTrue(result.Namespaces[0].DataTypes.Exists(obj => obj.Name == "TraceResult"));
            Assert.IsTrue(result.Namespaces[0].DataTypes.Exists(obj => obj.Name == "ITracer"));

            List<IMember> members = result.Namespaces[0].DataTypes.Single(obj => obj.Name == "ThreadStack").Members;
            Assert.IsTrue(members.Exists(obj => obj.Name == "TraceWatches"));
            members = result.Namespaces[0].DataTypes.Single(obj => obj.Name == "ThreadItem").Members;
            Assert.IsTrue(members.Exists(obj => obj.Name == "ThreadID"));
            Assert.IsTrue(members.Single(obj => obj.Name == "ThreadID").Accessor == "public");

            IMember member = result.Namespaces[0].DataTypes.Single(obj => obj.Name == "TracerMain").Members.Single(ex => ex.Name == "AddToTraceResult");
            Assert.AreEqual((member as AssemblyMethod).Parameters[0].Name, "watch");
        }

        [TestMethod]
        public void TestInvalidAssembly()
        {
            result = browser.Browse(@"Assemblies\appxpackaging.dll");
            Assert.AreEqual(result, null);
            result = browser.Browse(@"Assemblies\libcurl.dll");
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void TestSeveralNamespacesAssembly()
        {
            result = browser.Browse(@"Assemblies\Newtonsoft.Json.dll");
            Assert.AreEqual(result.Namespaces.Count, 12);
            Assert.IsTrue(result.Namespaces.All(obj => obj.DataTypes.Count > 0));
            Assert.IsTrue(result.Namespaces[8].DataTypes.All(ex => ex.Members.Count > 0));
        }

        [TestMethod]
        public void TestExtensionMethods()
        {
            result = browser.Browse(@"Assemblies\Chess.dll");

            Assert.AreEqual(result.Namespaces[0].DataTypes.Single(obj => obj.Name == "ColorMethods").Members.Count, 0);
            IMember member = result.Namespaces[0].DataTypes.Single(obj => obj.Name == "Color").Members.Single(ex => ex.Name == "SwitchColor");
            Assert.IsTrue((member as AssemblyMethod).Note == "ext" && (member as AssemblyMethod).Parameters[0].Name == "color");

            Assert.AreEqual(result.Namespaces[0].DataTypes.Single(obj => obj.Name == "FigureMethods").Members.Count, 0);
            member = result.Namespaces[0].DataTypes.Single(obj => obj.Name == "Figure").Members.Single(ex => ex.Name == "GetColor");
            Assert.IsTrue((member as AssemblyMethod).Note == "ext" && (member as AssemblyMethod).Parameters[0].ParameterType.Name == "Figure");
        }
    }

}
