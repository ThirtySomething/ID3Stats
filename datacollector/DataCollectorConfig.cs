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
            // MP3Path = "m:\\";
            MP3Path = "R:\\tagged";
            // MP3Pattern = "*.mp3";
            MP3Pattern = "*.flac";
            DataTranslation = "{'AC;DC':'AC/DC'}";
            UseHash = false;
        }

        /// <summary>
        /// Root path of MP3 collection
        /// </summary>
        public string MP3Path { get; set; }

        /// <summary>
        /// Pattern to search for
        /// </summary>
        public string MP3Pattern { get; set; }

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