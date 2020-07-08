using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Diagnostics.Tracing.Session;

namespace SplunkETW
{
    /// <summary>
    /// ProviderGuid class is in charge to parse input definition of the provider
    /// 
    /// Named Provider:
    ///     Microsoft-Windows-WMI-Activity
    /// 
    /// Tracelogging Provider:
    ///     TL{8e805eb3-6a8f-4a1e-90fa-a831d94e54a1}
    /// 
    /// WPP Provider:
    ///     WPP{8e805eb3-6a8f-4a1e-90fa-a831d94e54a1}
    /// </summary>
    public class ProviderGuid
    {
        /// <summary>
        /// Define the type of the parsed provider
        /// Actually this class can handle 3 kinds of provider definition
        /// 
        /// Manifest : Manifest based provider
        /// TL: TraceLogging provider
        /// WPP: Windows PreProcessing provider
        /// </summary>
        public enum ProviderType
        {
            Manifest,   // Manifest based provider
            TL,         // TraceLogging
            WPP         // Windows PreProcessing provider
        }

        /// <summary>
        /// Regular expression use to parse Guid Provider definition
        /// 
        /// Scheme:
        ///     NAME{GUID}
        ///     
        /// Example:
        ///     TL{8e805eb3-6a8f-4a1e-90fa-a831d94e54a1}
        ///     WPP{8e805eb3-6a8f-4a1e-90fa-a831d94e54a1}
        /// </summary>
        private static Regex PATTERN = new Regex(@"(?<type>\w+)\{(?<guid>[\w\d\-]+)\}", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Type of the provider
        /// </summary>
        public ProviderType Type { set;  get; }

        /// <summary>
        /// Associated GUID of the provider
        /// </summary>
        public Guid Guid { set;  get; }

        /// <summary>
        /// Parse a provider input definition
        /// This function can retrieve provider from Name definition or Guid definition
        /// 
        /// A name definition is associated with a manifest based provider and function will 
        /// check if the provider is recorded on current workstation
        /// 
        /// If name is not found we will try to parse Guid definition.
        /// A Guid definition must follow :
        ///     TYPE{GUID}
        /// When 
        ///     TYPE: Manifest | TL | WPP
        ///     GUID: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
        ///             
        /// </summary>
        /// <param name="name">input provider definition</param>
        /// <returns>ProviderGuid definition</returns>
        public static ProviderGuid Parse(string name)
        {
            var providerGuid = TraceEventProviders.GetProviderGuidByName(name);
            if (providerGuid != Guid.Empty)
            {
                return new ProviderGuid
                {
                    Type = ProviderType.Manifest,
                    Guid = providerGuid
                };
            }
            
            var matches = PATTERN.Matches(name);

            if (matches.Count != 1)
            {
                throw new Exception("Invalid Provider Format");
            }

            GroupCollection matchGroup = matches[0].Groups;

            return new ProviderGuid
            {
                Type = (ProviderType)Enum.Parse(typeof(ProviderType), matchGroup["type"].Value),
                Guid = Guid.Parse(matchGroup["guid"].Value)
            };
        }

        /// <summary>
        /// Parse a provider input definition
        /// This function can retrieve provider from Name definition or Guid definition
        /// 
        /// A name definition is associated with a manifest based provider and function will 
        /// check if the provider is recorded on current workstation
        /// 
        /// If name is not found we will try to parse Guid definition.
        /// A Guid definition must follow :
        ///     TYPE{GUID}
        /// When 
        ///     TYPE: Manifest | TL | WPP
        ///     GUID: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
        ///             
        /// </summary>
        /// <param name="name">provider input definition</param>
        /// <param name="provider">ProviderGuid</param>
        /// <returns>true if parsed succesfully</returns>
        public static bool TryParse(string name, out ProviderGuid provider)
        {
            try
            {
                provider = Parse(name);
                return true;
            }
            catch(Exception)
            {
                provider = null;
                return false;
            }
        }
    }
}
