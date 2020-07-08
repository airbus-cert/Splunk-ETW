using Microsoft.O365.Security.ETW;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SplunkETW;

namespace SplunkETWTest
{
    [TestClass]
    public class ForwarderTest
    {
        [TestMethod]
        public void TestBuildManifestForwarder()
        {
            ProviderGuid guid = null;
            Assert.IsTrue(ProviderGuid.TryParse("Microsoft-Windows-WMI-Activity", out guid));
            var forwarder = Forwarder.Build(guid);
            Assert.IsInstanceOfType(forwarder.Parser, typeof(ManifestParser));
        }

        [TestMethod]
        public void TestBuildTraceloggingForwarder()
        {
            ProviderGuid guid = null;
            Assert.IsTrue(ProviderGuid.TryParse("TL{8e805eb3-6a8f-4a1e-90fa-a831d94e54a1}", out guid));
            var forwarder = Forwarder.Build(guid);
            Assert.IsInstanceOfType(forwarder.Parser, typeof(TraceLoggingParser));
        }

        [TestMethod]
        public void TestBuildTWPPForwarder()
        {
            ProviderGuid guid = null;
            Assert.IsTrue(ProviderGuid.TryParse("WPP{8e805eb3-6a8f-4a1e-90fa-a831d94e54a1}", out guid));
            var forwarder = Forwarder.Build(guid);
            Assert.IsInstanceOfType(forwarder.Parser, typeof(NullParser));
        }

        [TestMethod]
        public async Task TestForwardManifest()
        {
            ProviderGuid guid = null;
            Assert.IsTrue(ProviderGuid.TryParse("Microsoft-Windows-WMI-Activity", out guid));
            var forwarder = Forwarder.Build(guid);

            var writer = new TestWriter();
            writer.OnWrite += async (Dictionary<string, object> eventData) =>
            {
                // Test if correctly parsed
                Assert.AreEqual(eventData["Commandline"], "cmd.exe");
            };

            await forwarder.Forward(new TestEventRecord
            {
                Id = 23,
                ProviderName = "Microsoft-Windows-WMI-Activity",
                ProviderId = Guid.Parse("1418ef04-b0b4-4623-bf7e-d74ab47bbdaa"),
                Fields = new Dictionary<string, object>
                {
                    {"Commandline" , "cmd.exe" }
                }
            }, writer);
        }

        [TestMethod]
        public async Task TestForwardTracelogging()
        {
            ProviderGuid guid = null;
            Assert.IsTrue(ProviderGuid.TryParse("TL{8e805eb3-6a8f-4a1e-90fa-a831d94e54a1}", out guid));
            var forwarder = Forwarder.Build(guid);

            var writer = new TestWriter();
            writer.OnWrite += async (Dictionary<string, object> eventData) =>
            {
                // Test if correctly parsed
                Assert.AreEqual(eventData["Test"], "Test");
            };

            await forwarder.Forward(new TestEventRecord
            {
                Id = 23,
                ProviderName = "TraceLogging",
                ProviderId = Guid.Parse("8e805eb3-6a8f-4a1e-90fa-a831d94e54a1"),
                Fields = new Dictionary<string, object>
                {
                    {"Test" , "Test" }
                },
                // Add a property of type 1 => Unicode String
                Properties = new List<Property> { new Property("Test", 1) }
            }, writer);
        }

        [TestMethod]
        public async Task TestForwardManifestWithFilterKo()
        {
            ProviderGuid guid = null;
            Assert.IsTrue(ProviderGuid.TryParse("Microsoft-Windows-WMI-Activity", out guid));
            var forwarder = Forwarder.Build(guid);

            var writer = new TestWriter();
            writer.OnWrite += async (Dictionary<string, object> eventData) =>
            {
                // this code does not be reached
                throw new AssertFailedException();
            };

            forwarder.AddFilter("Commandline", "toto.exe");

            await forwarder.Forward(new TestEventRecord
            {
                Id = 23,
                ProviderName = "Microsoft-Windows-WMI-Activity",
                ProviderId = Guid.Parse("1418ef04-b0b4-4623-bf7e-d74ab47bbdaa"),
                Fields = new Dictionary<string, object>
                {
                    {"Commandline" , "cmd.exe" }
                }
            }, writer);
        }

        [TestMethod]
        public async Task TestForwardManifestWithFilterOk()
        {
            ProviderGuid guid = null;
            Assert.IsTrue(ProviderGuid.TryParse("Microsoft-Windows-WMI-Activity", out guid));
            var forwarder = Forwarder.Build(guid);

            var writer = new TestWriter();
            writer.OnWrite += async (Dictionary<string, object> eventData) =>
            {
                // Test if correctly parsed
                Assert.AreEqual(eventData["Commandline"], "cmd.exe");
            };

            forwarder.AddFilter("Commandline", "cmd.exe");

            await forwarder.Forward(new TestEventRecord
            {
                Id = 23,
                ProviderName = "Microsoft-Windows-WMI-Activity",
                ProviderId = Guid.Parse("1418ef04-b0b4-4623-bf7e-d74ab47bbdaa"),
                Fields = new Dictionary<string, object>
                {
                    {"Commandline" , "cmd.exe" }
                }
            }, writer);
        }
    }
}