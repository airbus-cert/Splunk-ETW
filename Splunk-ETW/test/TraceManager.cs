using IniParser.Model;
using Microsoft.O365.Security.ETW;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SplunkETW;
using SplunkETW.Manifest;

namespace SplunkETWTest
{
    [TestClass]
    public class TraceManagerTest
    {
        [TestMethod]
        [ExpectedException(typeof(Exception), "Unable to create a trace without provider")]
        public void TestTraceManager()
        {
            IniData input = new IniData
            {
                Sections = new SectionDataCollection
                {
                    new SectionData("foo")
                }
            };

            TraceManager.BuildFromConfig(input, new TestWriter());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Unable to create a trace without provider")]
        public void TestTraceManagerBadId()
        {
            IniData input = new IniData
            {
                Sections = new SectionDataCollection
                {
                    new SectionData("Microsoft-Windows-WMI-Activity://foo")
                }
            };

            TraceManager.BuildFromConfig(input, new TestWriter());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Unable to create a trace without provider")]
        public void TestTraceManagerBadIdTl()
        {
            IniData input = new IniData
            {
                Sections = new SectionDataCollection
                {
                    new SectionData("TL{8e805eb3-6a8f-4a1e-90fa-a831d94e54a1}://foo")
                }
            };

            TraceManager.BuildFromConfig(input, new TestWriter());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Unable to create a trace without provider")]
        public void TestTraceManagerBadType()
        {
            IniData input = new IniData
            {
                Sections = new SectionDataCollection
                {
                    new SectionData("FOO{8e805eb3-6a8f-4a1e-90fa-a831d94e54a1}")
                }
            };

            TraceManager.BuildFromConfig(input, new TestWriter());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Unable to create a trace without provider")]
        public void TestTraceManagerBadGuid()
        {
            IniData input = new IniData
            {
                Sections = new SectionDataCollection
                {
                    new SectionData("TL{foo}")
                }
            };

            TraceManager.BuildFromConfig(input, new TestWriter());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Unable to create a trace without provider")]
        public void TestTraceManagerBadIdWpp()
        {
            IniData input = new IniData
            {
                Sections = new SectionDataCollection
                {
                    new SectionData("WPP{8e805eb3-6a8f-4a1e-90fa-a831d94e54a1}://foo")
                }
            };

            TraceManager.BuildFromConfig(input, new TestWriter());
        }

        [TestMethod]
        public void TestTraceManagerManifest()
        {
            IniData input = new IniData
            {
                Sections = new SectionDataCollection
                {
                    new SectionData("Microsoft-Windows-WMI-Activity")
                }
            };

            
            var trace = TraceManager.BuildFromConfig(input, new TestWriter());
            Assert.IsNotNull(trace);
        }

        [TestMethod]
        public void TestTraceManagerManifestWithId()
        {
            IniData input = new IniData
            {
                Sections = new SectionDataCollection
                {
                    new SectionData("Microsoft-Windows-WMI-Activity://12")
                }
            };


            var trace = TraceManager.BuildFromConfig(input, new TestWriter());
            Assert.IsNotNull(trace);
        }

        [TestMethod]
        public void TestTraceManagerTraceLogging()
        {
            IniData input = new IniData
            {
                Sections = new SectionDataCollection
                {
                    new SectionData("TL{8e805eb3-6a8f-4a1e-90fa-a831d94e54a1}")
                }
            };


            var trace = TraceManager.BuildFromConfig(input, new TestWriter());
            Assert.IsNotNull(trace);
        }

        [TestMethod]
        public void TestTraceManagerTraceLoggingWithId()
        {
            IniData input = new IniData
            {
                Sections = new SectionDataCollection
                {
                    new SectionData("TL{8e805eb3-6a8f-4a1e-90fa-a831d94e54a1}://45")
                }
            };


            var trace = TraceManager.BuildFromConfig(input, new TestWriter());
            Assert.IsNotNull(trace);
        }


        [TestMethod]
        public void TestTraceManagerWPP()
        {
            IniData input = new IniData
            {
                Sections = new SectionDataCollection
                {
                    new SectionData("WPP{8e805eb3-6a8f-4a1e-90fa-a831d94e54a1}")
                }
            };


            var trace = TraceManager.BuildFromConfig(input, new TestWriter());
            Assert.IsNotNull(trace);
        }

        [TestMethod]
        public void TestTraceManagerWPPWithId()
        {
            IniData input = new IniData
            {
                Sections = new SectionDataCollection
                {
                    new SectionData("WPP{8e805eb3-6a8f-4a1e-90fa-a831d94e54a1}://12")
                }
            };


            var trace = TraceManager.BuildFromConfig(input, new TestWriter());
            Assert.IsNotNull(trace);
        }

        [TestMethod]
        public void TestTraceManagerManifestFromGuid()
        {
            IniData input = new IniData
            {
                Sections = new SectionDataCollection
                {
                    new SectionData("Manifest{1418ef04-b0b4-4623-bf7e-d74ab47bbdaa}")
                }
            };


            var trace = TraceManager.BuildFromConfig(input, new TestWriter());
            Assert.IsNotNull(trace);
        }

        [TestMethod]
        public void TestTraceManagerManifestFromGuidWithId()
        {
            IniData input = new IniData
            {
                Sections = new SectionDataCollection
                {
                    new SectionData("Manifest{1418ef04-b0b4-4623-bf7e-d74ab47bbdaa}://45")
                }
            };


            var trace = TraceManager.BuildFromConfig(input, new TestWriter());
            Assert.IsNotNull(trace);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Unable to create a trace without provider")]
        public void TestTraceManagerunknownManifest()
        {
            IniData input = new IniData
            {
                Sections = new SectionDataCollection
                {
                    new SectionData("Manifest{1418ef04-0000-0000-0000-d74ab47bbdaa}")
                }
            };


            var trace = TraceManager.BuildFromConfig(input, new TestWriter());
            Assert.IsNotNull(trace);
        }
    }
}