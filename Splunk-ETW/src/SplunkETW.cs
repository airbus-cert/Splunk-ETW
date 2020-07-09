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
using IniParser;
using IniParser.Model;

namespace SplunkETW
{
    /// <summary>
    /// Declare the Splunk Plugin meta informations
    /// </summary>
    public class TAEventTracingWindows : ModularInput
    {
        /// <summary>
        /// Declare Splunk Plugin declaration scheme
        /// for inputs.conf
        /// </summary>
        public override Scheme Scheme => new Scheme
        {
            Title = "Splunk-ETW",
            Description = "A Splunk ETW Forwarder"
        };

        public static int Main(string[] args)
        {
            return Run<TAEventTracingWindows>(args, DebuggerAttachPoints.StreamEvents);
            //TraceManager.BuildFromConfig(new FileIniDataParser().ReadFile(@"c:\work\config.ini"), new DebugWriter()).Start();
            //return 0;
        }

        /// <summary>
        /// Splunk callback use to launch the plugin
        /// </summary>
        /// <param name="inputDefinition">Retrieve the config set into inputs.conf</param>
        /// <param name="eventWriter">event writer use to communicate with splunk</param>
        /// <returns></returns>
        public override async Task StreamEventsAsync(InputDefinition inputDefinition, EventWriter eventWriter)
        {
            var splunkHome = Environment.GetEnvironmentVariable("SPLUNK_HOME");
            var profile = inputDefinition.Name.Split(new string[] { "://" }, StringSplitOptions.RemoveEmptyEntries)[1];
            await eventWriter.LogAsync(Severity.Info, "Splunk-ETW select " + profile + " profile");

            string iniPath = Path.Combine(new string[] { splunkHome, "etc", "apps", "Splunk-ETW", "profile", profile + ".ini" });
            await eventWriter.LogAsync(Severity.Info, "Splunk-ETW load " + iniPath);

            try
            {
                var trace = TraceManager.BuildFromConfig(new FileIniDataParser().ReadFile(iniPath), new SplunkWriter(eventWriter, inputDefinition.Name));
                await Task.Run(() => trace.Start());
            }
            catch(Exception e)
            {
                await eventWriter.LogAsync(Severity.Error, e.ToString());
            }
        }
    }
}