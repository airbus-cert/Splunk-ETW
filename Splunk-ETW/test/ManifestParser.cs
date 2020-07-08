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
    public class ManifestParserTest
    {
        [TestMethod]
        public void TestManifestParser()
        {
            var eventData = new Dictionary<string, object>();

            // Generate Test manifest
            var manifest = new Manifest
            {
                instrumentation = new Instrumentation
                {
                    events = new Events
                    {
                        provider = new SplunkETW.Manifest.Provider
                        {
                            events = new List<Event>
                            {
                                new Event()
                                {
                                    value = "23",
                                    template = "1"
                                }
                            },
                            templates = new List<Template>
                            {
                                new Template()
                                {
                                    tid = "1",
                                    datas = new List<Data>
                                    {
                                        new Data()
                                        {
                                            name = "FieldTest",
                                            inType = Data.InType.UnicodeString
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
            new ManifestParser(manifest).Parse(new TestEventRecord
            {
                Id = 23,
                ProviderName = "Test",
                ProviderId = Guid.Parse("1418ef04-b0b4-4623-bf7e-d74ab47bbdaa"),
                Fields = new Dictionary<string, object>
                {
                    {"FieldTest" , "success" }
                }
            }, eventData);

            Assert.AreEqual(eventData["FieldTest"], "success");
        }

        [TestMethod]
        public void TestManifestParserFieldNonPresent()
        {
            var eventData = new Dictionary<string, object>();

            // Generate Test manifest
            var manifest = new Manifest
            {
                instrumentation = new Instrumentation
                {
                    events = new Events
                    {
                        provider = new SplunkETW.Manifest.Provider
                        {
                            events = new List<Event>
                            {
                                new Event()
                                {
                                    value = "23",
                                    template = "1"
                                }
                            },
                            templates = new List<Template>
                            {
                                new Template()
                                {
                                    tid = "1",
                                    datas = new List<Data>
                                    {
                                        new Data()
                                        {
                                            name = "FieldTest",
                                            inType = Data.InType.UnicodeString
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
            new ManifestParser(manifest).Parse(new TestEventRecord
            {
                Id = 23,
                ProviderName = "Test",
                ProviderId = Guid.Parse("1418ef04-b0b4-4623-bf7e-d74ab47bbdaa"),
                Fields = new Dictionary<string, object>
                {
                    
                }
            }, eventData);

            Assert.AreEqual(eventData["FieldTest"], ManifestParser.ERROR_PARSING_FIELD);
        }

    }
}