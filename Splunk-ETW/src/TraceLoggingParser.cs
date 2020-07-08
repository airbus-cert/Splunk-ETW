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
    /// Parse Tracelogging event
    /// Tracelogging are defined using the input definition:
    ///     TL{GUID}
    /// TraceLogging include the scheme into event
    /// But we known the mapping between type and C# type
    /// </summary>
    public class TraceLoggingParser : IParser
    {
        /// <summary>
        /// Error parsing field
        /// </summary>
        public static string ERROR_PARSING_FIELD = "Error during processing field value";

        /// <summary>
        /// Parse an event log base on tracelogging
        /// </summary>
        /// <param name="record">ETW event record</param>
        /// <param name="eventData">dict will be filled with event data</param>
        public void Parse(IEventRecord record, Dictionary<String, dynamic> eventData)
        {
            foreach (var property in record.Properties)
            {
                try
                {
                    switch (property.Type)
                    {
                        case 1:
                            eventData[property.Name] = record.GetUnicodeString(property.Name);
                            break;
                        case 2:
                            eventData[property.Name] = record.GetAnsiString(property.Name);
                            break;
                        case 3:
                            eventData[property.Name] = record.GetInt8(property.Name);
                            break;
                        case 4:
                            eventData[property.Name] = record.GetUInt8(property.Name);
                            break;
                        case 5:
                            eventData[property.Name] = record.GetInt16(property.Name);
                            break;
                        case 6:
                            eventData[property.Name] = record.GetUInt16(property.Name);
                            break;
                        case 7:
                            eventData[property.Name] = record.GetInt32(property.Name);
                            break;
                        case 8:
                            eventData[property.Name] = record.GetUInt32(property.Name);
                            break;
                        case 9:
                            eventData[property.Name] = record.GetInt64(property.Name);
                            break;
                        case 10:
                            eventData[property.Name] = record.GetUInt64(property.Name);
                            break;
                        case 13:
                            eventData[property.Name] = record.GetUInt32(property.Name);
                            break;
                        case 14:
                            eventData[property.Name] = record.GetBinary(property.Name);
                            break;
                        case 15:
                            eventData[property.Name] = record.GetBinary(property.Name);
                            break;
                        case 20:
                            eventData[property.Name] = record.GetUInt32(property.Name);
                            break;
                        case 21:
                            eventData[property.Name] = record.GetUInt64(property.Name);
                            break;
                    }
                }
                catch(Exception)
                {
                    eventData[property.Name] = ERROR_PARSING_FIELD;
                }
            }
        }
    }
}