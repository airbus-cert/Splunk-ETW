using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.O365.Security.ETW;
using Splunk.ModularInputs;
using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Session;
using System.Xml.Serialization;
using System.Linq;

namespace SplunkETW
{
    /// <summary>
    /// Null parser is used in case of WPP
    /// Actually with can't parse WPP but just the existing of an event can interesting
    /// </summary>
    public class NullParser : IParser
    {
        public void Parse(IEventRecord record, Dictionary<String, dynamic> eventData)
        {
           // This function do nothing actually
        }
    }
}