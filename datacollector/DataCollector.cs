namespace net.derpaul.cdstats
{
    /// <summary>
    /// Find MP3 files, import ID3 tag to database, run plugins
    /// </summary>
    internal class DataCollector
    {
        static void Main(string[] args)
        {
            DataCollectorConfig.Instance.ShowConfig();
            var finderMP3 = new FinderMP3(DataCollectorConfig.Instance.MP3Path, DataCollectorConfig.Instance.MP3Pattern);
            if (!finderMP3.Init())
            {
                // Either path does not exist or filepattern not set
                return;
            }

            // Retrieve list of MP3 files
            List<string> fileList = finderMP3.Process();
            Console.WriteLine(fileList);
        }
    }
}
