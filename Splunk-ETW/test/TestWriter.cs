using Microsoft.O365.Security.ETW;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SplunkETW;

namespace SplunkETWTest
{
    public class TestWriter : IETWWriter
    {
        public delegate Task OnWriteHandler(Dictionary<string, object> eventData);
        public event OnWriteHandler OnWrite;

        public async Task write(Dictionary<string, object> eventData)
        {
            if (this.OnWrite != null)
            {
                await this.OnWrite.Invoke(eventData);
            }
        }
    }
}