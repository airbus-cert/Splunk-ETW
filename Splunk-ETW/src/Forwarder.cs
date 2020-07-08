using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.O365.Security.ETW;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SplunkETW
{
    /// <summary>
    /// Forwarder class is used to forward or not message from 
    /// ETW session to a writer
    /// 
    /// Forwarder have match equal filter
    /// 
    /// Actually filter implementation only take a look on string reprsentation
    /// It means that a ToString called is processed on the raw value befaire comparing
    /// </summary>
    public class Forwarder
    {

        /// <summary>
        /// List of all filters
        /// </summary>
        private List<Tuple<string, string>> Filters;

        public IParser Parser { get; }

        /// <summary>
        /// Default ctor for Forwarder
        /// Init filters with empty
        /// </summary>
        public Forwarder(IParser parser)
        {
            this.Parser = parser;
            this.Filters = new List<Tuple<string, string>>();
        }

        /// <summary>
        /// You can add a filter
        /// </summary>
        /// <param name="fieldName">name of field which will be used for comparing</param>
        /// <param name="fieldValue">string representation of the expected value</param>
        public void AddFilter(string fieldName, string fieldValue)
        {
            this.Filters.Add(Tuple.Create(fieldName, fieldValue));
        }

        /// <summary>
        /// Core forward function
        /// Init eventdata with common data for all record:
        ///     ProviderName
        ///     ProviderGuid
        ///     EventID
        ///     
        /// Call the inner parser
        /// Then check filter on eventdata
        /// If a field match and value does not match event is filtering
        /// If field is not present event is NOT filtered
        /// </summary>
        /// <param name="record">Event Record</param>
        /// <param name="writer">Writer destination</param>
        /// <returns></returns>
        public async Task Forward(IEventRecord record, IETWWriter writer)
        {
            Dictionary<String, object> eventData = new Dictionary<string, dynamic>();
            eventData["ProviderName"] = record.ProviderName;
            eventData["ProviderGuid"] = record.ProviderId.ToString();
            eventData["EventID"] = record.Id;
            eventData["ProcessId"] = record.ProcessId;
            eventData["ThreadId"] = record.ThreadId;
            eventData["Timestamp"] = record.Timestamp.ToString();

            this.Parser.Parse(record, eventData);

            foreach(var filter in this.Filters)
            {
                if (!eventData.ContainsKey(filter.Item1))
                    return;
                if (eventData[filter.Item1].ToString() != filter.Item2)
                    return;
            }
            await writer.write(eventData);
        }

        /// <summary>
        /// Build a correct forwarder with the matched Parser
        /// Follow the factory model
        /// </summary>
        /// <param name="provider">ProviderGuid input provider</param>
        /// <returns></returns>
        public static Forwarder Build(ProviderGuid provider)
        {
            switch(provider.Type)
            {
                case ProviderGuid.ProviderType.Manifest:
                    var xml = RegisteredTraceEventParser.GetManifestForRegisteredProvider(provider.Guid);
                    XmlSerializer serializer = new XmlSerializer(typeof(Manifest.Manifest));
                    using (TextReader reader = new StringReader(xml))
                    {
                        var manifest = (Manifest.Manifest)serializer.Deserialize(reader);
                        return new Forwarder(new ManifestParser(manifest));
                    }
                case ProviderGuid.ProviderType.TL:
                    return new Forwarder(new TraceLoggingParser());
                case ProviderGuid.ProviderType.WPP:
                    return new Forwarder(new NullParser());
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Try to build a forwarder
        /// Handle all subexception
        /// </summary>
        /// <param name="providerGuid">provider input definition</param>
        /// <param name="forwarder">result as provider</param>
        /// <returns>true if built successfully</returns>
        public static bool TryBuild(ProviderGuid providerGuid, out Forwarder forwarder)
        {
            try
            {
                var result = Build(providerGuid);
                forwarder = result;
                return true;
            }
            catch(System.ApplicationException)
            {
                forwarder = null;
                return false;
            }
        }
    }
}