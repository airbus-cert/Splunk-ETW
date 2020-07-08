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
    public class NullParserTest
    {
        [TestMethod]
        public void TestNullParser()
        {
            var eventData = new Dictionary<string, object>();
            
            new NullParser().Parse(new TestEventRecord
            {
                Id = 23,
                ProviderName = "Test",
                ProviderId = Guid.Parse("1418ef04-b0b4-4623-bf7e-d74ab47bbdaa"),
                Fields = new Dictionary<string, object>
                {

                }
            }, eventData);
            Assert.IsTrue(eventData.Count == 0);
        }
    }
}