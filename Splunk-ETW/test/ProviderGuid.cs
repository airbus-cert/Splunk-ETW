using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SplunkETW;

namespace SplunkETWTest
{
    [TestClass]
    public class ProviderGuidTest
    {
        [TestMethod]
        public void TestParseUnknownNamed()
        {
            ProviderGuid guid = null;
            Assert.IsFalse(ProviderGuid.TryParse("foo", out guid));
        }

        [TestMethod]
        public void TestParseKnownNamed()
        {
            ProviderGuid guid = null;
            Assert.IsTrue(ProviderGuid.TryParse("Microsoft-Windows-WMI-Activity", out guid));
            Assert.AreEqual(guid.Type, ProviderGuid.ProviderType.Manifest);
            Assert.AreEqual(guid.Guid, Guid.Parse("1418ef04-b0b4-4623-bf7e-d74ab47bbdaa"));
        }

        [TestMethod]
        public void TestParseTLBadGuid()
        {
            ProviderGuid guid = null;
            Assert.IsFalse(ProviderGuid.TryParse("TL{123}", out guid));
        }

        [TestMethod]
        public void TestParseTLOk()
        {
            ProviderGuid guid = null;
            Assert.IsTrue(ProviderGuid.TryParse("TL{8e805eb3-6a8f-4a1e-90fa-a831d94e54a1}", out guid));
            Assert.AreEqual(guid.Type, ProviderGuid.ProviderType.TL);
            Assert.AreEqual(guid.Guid, Guid.Parse("8e805eb3-6a8f-4a1e-90fa-a831d94e54a1"));
        }

        [TestMethod]
        public void TestParseWPPOk()
        {
            ProviderGuid guid = null;
            Assert.IsTrue(ProviderGuid.TryParse("WPP{8e805eb3-6a8f-4a1e-90fa-a831d94e54a1}", out guid));
            Assert.AreEqual(guid.Type, ProviderGuid.ProviderType.WPP);
            Assert.AreEqual(guid.Guid, Guid.Parse("8e805eb3-6a8f-4a1e-90fa-a831d94e54a1"));
        }

        [TestMethod]
        public void TestParseUnknownType()
        {
            ProviderGuid guid = null;
            Assert.IsFalse(ProviderGuid.TryParse("FOO{8e805eb3-6a8f-4a1e-90fa-a831d94e54a1}", out guid));
        }
    }
}