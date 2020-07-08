using Microsoft.O365.Security.ETW;
using Newtonsoft.Json;
using Splunk.ModularInputs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SplunkETW
{
    /// <summary>
    /// Use to abstract parser
    /// Useful for dev and test purpose
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// All parser must implement tyhis method
        /// Use to parse event record and fill eventdata with meta informations
        /// </summary>
        /// <param name="record">original etw event</param>
        /// <param name="eventData">data struct that include all event fields</param>
        void Parse(IEventRecord record, Dictionary<String, object> eventData);
    }
}