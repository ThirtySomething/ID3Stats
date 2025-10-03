using System.Xml.Serialization;

namespace net.derpaul.id3stats
{
    /// <summary>
    /// Configuration settings of MP3Stats
    /// </summary>
    public class MP3StatsConfig : ConfigLoader<MP3StatsConfig>, IConfigObject
    {
        /// <summary>
        /// To set default values
        /// </summary>
        public void SetDefaults()
        {
            StatisticsMainFile = "MP3Stats.html";
            PathPlugin = "";
            PathOutput = "./mp3stats/";
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