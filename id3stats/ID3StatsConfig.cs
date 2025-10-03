using System.Xml.Serialization;

namespace net.derpaul.id3stats
{
    /// <summary>
    /// Configuration settings of ID3Stats
    /// </summary>
    public class ID3StatsConfig : ConfigLoader<ID3StatsConfig>, IConfigObject
    {
        /// <summary>
        /// To set default values
        /// </summary>
        public void SetDefaults()
        {
            StatisticsMainFile = "ID3Stats.html";
            PathPlugin = "";
            PathOutput = "./id3stats/";
        }

        /// <summary>
        /// Name of statistics main file
        /// </summary>
        public string StatisticsMainFile { get; set; }

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
        public string PluginProductName { get; } = "net.derpaul.id3stats.plugin";
    }
}