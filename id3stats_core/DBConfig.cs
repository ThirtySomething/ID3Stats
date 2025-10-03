namespace net.derpaul.id3stats
{
    /// <summary>
    /// Configuration settings of MP3Stats database access
    /// </summary>
    public class DBConfig : ConfigLoader<DBConfig>, IConfigObject
    {
        /// <summary>
        /// To set default values
        /// </summary>
        public void SetDefaults()
        {
            DBFilename = "mp3stats.db";
        }

        /// <summary>
        /// The name of the database file
        /// </summary>
        public string DBFilename { get; set; }
    }
}
