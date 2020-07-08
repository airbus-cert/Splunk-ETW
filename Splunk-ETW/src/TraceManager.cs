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
using IniParser.Model;

namespace SplunkETW
{
    
    public class TraceManager
    {
        /// <summary>
        /// This function build an ETW usertrace from
        /// a configuration INI file and a selected IETWWriter
        /// </summary>
        /// <param name="config">configuration that come from an ini file</param>
        /// <param name="writer">how to writer etw</param>
        /// <returns></returns>
        public static UserTrace BuildFromConfig(IniData config, IETWWriter writer)
        {
            var providers = new Dictionary<Guid, Provider>();

            foreach (var providerConfig in config.Sections)
            {
                // try to parse filtering provider
                var providerDeclaration = providerConfig.SectionName.Split(new string[] { "://" }, StringSplitOptions.RemoveEmptyEntries);
                if (providerDeclaration.Length > 2)
                    continue;

                string providerName = providerDeclaration[0];

                // Try to parse provider name
                ProviderGuid providerGuid = null;
                if (!ProviderGuid.TryParse(providerName, out providerGuid))
                {
                    continue;
                }

                Forwarder forwarder = null;
                if (!Forwarder.TryBuild(providerGuid, out forwarder))
                {
                    continue;
                }

                UInt16? eventId = null;
                if (providerDeclaration.Length == 2)
                {
                    UInt16 tmp = 0;
                    if (!UInt16.TryParse(providerDeclaration[1], out tmp))
                        continue;
                    eventId = tmp;
                }

                if (!providers.ContainsKey(providerGuid.Guid))
                {
                    providers.Add(providerGuid.Guid, new Provider(providerGuid.Guid));
                }

                var provider = providers[providerGuid.Guid];

                var predicate = Filter.AnyEvent();

                if (eventId != null)
                {
                    predicate = Filter.EventIdIs(eventId.Value);
                }

                var filter = new EventFilter(predicate);
                foreach (var keyValue in providerConfig.Keys)
                {
                    forwarder.AddFilter(keyValue.KeyName, keyValue.Value);
                }

                filter.OnEvent += (IEventRecord record) =>
                {
                    try
                    {
                        forwarder.Forward(record, writer).Wait();
                    }
                    catch (System.AggregateException) { } // Some event ae not documented even for Microsoft
                };

                provider.AddFilter(filter);
            }

            if (providers.Count == 0)
            {
                throw new Exception("Unable to create a trace without provider");
            }

            UserTrace trace = new UserTrace(String.Format("Splunk-ETW-{0}", Guid.NewGuid().ToString()));
            foreach(var provider in providers.Values)
            {
                trace.Enable(provider);
            }

            return trace;
        }
    }
}