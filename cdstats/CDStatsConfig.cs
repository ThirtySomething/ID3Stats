using System.Xml.Serialization;

namespace net.derpaul.cdstats
{
    /// <summary>
    /// Configuration settings of CDStats
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
        /// Path for statistic plugins
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