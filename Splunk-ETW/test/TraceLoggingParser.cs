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
    public class TraceLoggingParserTest
    {
        [TestMethod]
        public void TestTraceLoggingParser()
        {
            var eventData = new Dictionary<string, object>();
            
            new TraceLoggingParser().Parse(new TestEventRecord
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
            }, eventData);

            Assert.AreEqual(eventData["Test"], "Test");
        }

        [TestMethod]
        public void TestTraceLoggingParserKo()
        {
            var eventData = new Dictionary<string, object>();

            new TraceLoggingParser().Parse(new TestEventRecord
            {
                Id = 23,
                ProviderName = "TraceLogging",
                ProviderId = Guid.Parse("8e805eb3-6a8f-4a1e-90fa-a831d94e54a1"),
                Fields = new Dictionary<string, object>
                {
                    {"Test" , 1234 }
                },
                // Add a property of type 1 => Unicode String
                Properties = new List<Property> { new Property("Test", 1) }
            }, eventData);

            Assert.AreEqual(eventData["Test"], TraceLoggingParser.ERROR_PARSING_FIELD);
        }
    }
}