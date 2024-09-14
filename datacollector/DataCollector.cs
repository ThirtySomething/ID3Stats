namespace net.derpaul.cdstats
{
    /// <summary>
    /// Find MP3 files, import ID3 tag to database, run plugins
    /// </summary>
    internal class DataCollector
    {
        static void Main(string[] args)
        {
            // Show current configuration
            DataCollectorConfig.Instance.ShowConfig();
            // Initalize finder with path and pattern
            var finderMP3 = new FinderMP3(DataCollectorConfig.Instance.MP3Path, DataCollectorConfig.Instance.MP3Pattern);
            if (!finderMP3.Init())
            {
                // Either path does not exist or filepattern not set
                return;
            }
            // Retrieve list of MP3 files
            List<MP3File> fileList = finderMP3.Process();
            // Initialize
            HandleID3 id3todb = new HandleID3(fileList);
            if (!id3todb.Init())
            {
                // Something went wrong during initialization
                return;
            }
            // Read ID3 tags and write them to database
            id3todb.Process(DataCollectorConfig.Instance.MP3Path);
        }
    }
}
