namespace net.derpaul.id3stats
{
    /// <summary>
    /// Configuration settings of datacollector
    /// </summary>
    public class DataCollectorConfig : ConfigLoader<DataCollectorConfig>, IConfigObject
    {
        /// <summary>
        /// To set default values
        /// </summary>
        public void SetDefaults()
        {
            // ID3Path = "m:\\";
            ID3Path = "R:\\tagged";
            // ID3Pattern = "*.mp3";
            ID3Pattern = "*.flac";
            DataTranslation = "{'AC;DC':'AC/DC'}";
            UseHash = false;
        }

        /// <summary>
        /// Root path of tagged files collection
        /// </summary>
        public string ID3Path { get; set; }

        /// <summary>
        /// Pattern to search for
        /// </summary>
        public string ID3Pattern { get; set; }

        /// <summary>
        /// JSON String with mappings
        /// </summary>
        public string DataTranslation { get; set; }

        /// <summary>
        /// Use file MD5 hash on import
        /// </summary>
        public bool UseHash { get; set; }
    }
}