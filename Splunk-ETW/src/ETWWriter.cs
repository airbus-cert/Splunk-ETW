using Newtonsoft.Json;
using Splunk.ModularInputs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SplunkETW
{
    /// <summary>
    /// Use to abstract writer
    /// Useful for dev and test purpose
    /// </summary>
    public interface IETWWriter
    {
        /// <summary>
        /// Async task use to write data
        /// </summary>
        /// <param name="eventData">input data to write on write</param>
        /// <returns></returns>
        Task write(Dictionary<String, object> eventData);
    }

    /// <summary>
    /// This writer is designed for debug purpose
    /// </summary>
    public class DebugWriter : IETWWriter
    {
        /// <summary>
        /// Write directly on console output
        /// </summary>
        /// <param name="data">test to write on console output</param>
        /// <returns></returns>
        public async Task write(Dictionary<String, object> eventData)
        {
            await Console.Out.WriteLineAsync(JsonConvert.SerializeObject(eventData));
        }
    }

    /// <summary>
    /// Implementation of the Splunk writer
    /// </summary>
    public class SplunkWriter : IETWWriter
    {
        /// <summary>
        /// The real Splunk writer implementation
        /// </summary>
        private EventWriter Writer { get; }

        /// <summary>
        /// Also known as sourcetype, identity the kind of source
        /// </summary>
        private string Stanza { get; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="writer">Splunk writer</param>
        /// <param name="stanza">SourceType</param>
        public SplunkWriter(EventWriter writer, string stanza)
        {
            this.Writer = writer;
            this.Stanza = stanza;
        }

        /// <summary>
        /// Async write function
        /// This will use internally QueueEventForWriting of the Splunk API
        /// </summary>
        /// <param name="eventData">data to write</param>
        /// <returns></returns>
        public async Task write(Dictionary<String, object> eventData)
        {
            await this.Writer.QueueEventForWriting(new Event
            {
                Stanza = this.Stanza,
                Data = JsonConvert.SerializeObject(eventData)
            });
        }
    }
}