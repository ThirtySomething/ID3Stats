using Microsoft.VisualBasic;
using System.Text.Json.Nodes;
using System.Xml.Serialization;

namespace net.derpaul.cdstats
{
    /// <summary>
    /// Configuration settings of datacollector to read the bricklets
    /// </summary>
    public class CDStatsConfig : ConfigLoader<CDStatsConfig>, IConfigObject
    {
        /// <summary>
        /// To set default values
        /// </summary>
        public void SetDefaults()
        {
            PathPlugin = "./plugins/";
            PathOutput = "./cdstats/";
        }

        /// <summary>
        /// Path to statistic plugins
        /// </summary>
        public string PathPlugin { get; set; }

        /// <summary>
        /// Path for output files
        /// </summary>
        public string PathOutput { get; set; }

        /// <summary>
        /// Product name of plugin set in AssemblyInfo.cs
        /// This is hardcoded and not configurable!
        /// </summary>
        [XmlIgnore]
        public string PluginProductName { get; } = "net.derpaul.cdstats.plugin";
    }
}