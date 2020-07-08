using Microsoft.O365.Security.ETW;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SplunkETW
{
    /// <summary>
    /// This class is used to parse event that are Manifest based
    /// The manifest is used to retrieve name and type
    /// </summary>
    public class ManifestParser : IParser
    {
        /// <summary>
        /// Error message
        /// </summary>
        public static string ERROR_PARSING_FIELD = "Error during processing field value";

        /// <summary>
        /// Scheme is loaded by the caller
        /// </summary>
        private Manifest.Manifest Scheme { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="scheme">Manifest</param>
        public ManifestParser(Manifest.Manifest scheme)
        {
            this.Scheme = scheme;
        }

        /// <summary>
        /// Try to parse an event record base on the manifest
        /// </summary>
        /// <param name="record">ETW event record</param>
        /// <param name="eventData">eventdata that will be filled by the parser</param>
        public void Parse(IEventRecord record, Dictionary<String, dynamic> eventData)
        {
            foreach (var eventDefinition in this.Scheme.instrumentation.events.provider.events)
            {
                if (Int16.Parse(eventDefinition.value) != record.Id)
                    continue;
                
                var template = this.Scheme.instrumentation.events.provider.templates.Where(x => x.tid == eventDefinition.template).Single();
                foreach (var data in template.datas)
                {
                    try
                    {
                        switch (data.inType)
                        {
                            case Manifest.Data.InType.UnicodeString:
                                eventData[data.name] = record.GetUnicodeString(data.name);
                                break;
                            case Manifest.Data.InType.AnsiString:
                                eventData[data.name] = record.GetAnsiString(data.name);
                                break;
                            case Manifest.Data.InType.GUID:
                                eventData[data.name] = record.GetBinary(data.name);
                                break;
                            case Manifest.Data.InType.UInt32:
                                eventData[data.name] = record.GetUInt32(data.name);
                                break;
                            case Manifest.Data.InType.HexInt32:
                                eventData[data.name] = record.GetInt32(data.name);
                                break;
                            case Manifest.Data.InType.HexInt64:
                                eventData[data.name] = record.GetInt64(data.name);
                                break;
                            case Manifest.Data.InType.Boolean:
                                eventData[data.name] = record.GetUInt32(data.name);
                                break;
                            case Manifest.Data.InType.UInt16:
                                eventData[data.name] = record.GetUInt16(data.name);
                                break;
                            case Manifest.Data.InType.Binary:
                                eventData[data.name] = record.GetBinary(data.name);
                                break;
                            case Manifest.Data.InType.UInt64:
                                eventData[data.name] = record.GetUInt64(data.name);
                                break;
                            case Manifest.Data.InType.Double:
                                eventData[data.name] = record.GetUInt64(data.name);
                                break;
                            case Manifest.Data.InType.UInt8:
                                eventData[data.name] = record.GetUInt8(data.name);
                                break;
                            case Manifest.Data.InType.Int8:
                                eventData[data.name] = record.GetInt64(data.name);
                                break;
                            case Manifest.Data.InType.Int16:
                                eventData[data.name] = record.GetInt16(data.name);
                                break;
                            case Manifest.Data.InType.Int32:
                                eventData[data.name] = record.GetInt32(data.name);
                                break;
                            case Manifest.Data.InType.Int64:
                                eventData[data.name] = record.GetInt64(data.name);
                                break;
                            case Manifest.Data.InType.FILETIME:
                                eventData[data.name] = record.GetBinary(data.name);
                                break;
                            case Manifest.Data.InType.Pointer:
                                eventData[data.name] = record.GetBinary(data.name);
                                break;
                            case Manifest.Data.InType.SYSTEMTIME:
                                eventData[data.name] = record.GetDateTime(data.name);
                                break;
                            case Manifest.Data.InType.SID:
                                eventData[data.name] = record.GetBinary(data.name);
                                break;
                            case Manifest.Data.InType.Float:
                                eventData[data.name] = record.GetUInt32(data.name);
                                break;
                        }
                    }
                    catch(Exception)
                    {
                        eventData[data.name] = ERROR_PARSING_FIELD;
                    }
                }

                break;
            }
        }
    }
}