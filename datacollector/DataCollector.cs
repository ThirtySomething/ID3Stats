using NLog;
using System.Diagnostics;

namespace net.derpaul.id3stats
{
    /// <summary>
    /// Find tagged files, import filename and meta data to database
    /// </summary>
    internal class DataCollector
    {
        /// <summary>
        /// Logger for writing log files
        /// </summary>
        private static NLog.Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            // Setup of logger
            var configuration = new NLog.Config.LoggingConfiguration();
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "datacollector.log" };
            configuration.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, logfile);
            NLog.LogManager.Configuration = configuration;

            // Ensure configuration
            DataCollectorConfig.Instance.ShowConfig(logger);
            DataCollectorConfig.Instance.Save();

            try
            {
                Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();

                logger.Info("Startup of DataCollector at {now}", DateTime.Now);

                // Initalize finder with path and pattern
                var finderID3 = new FinderID3(DataCollectorConfig.Instance.ID3Path, DataCollectorConfig.Instance.ID3Pattern);
                if (!finderID3.Init(logger))
                {
                    // Either path does not exist or filepattern not set
                    logger.Error("Init of DataCollector failed!");
                    return;
                }

                // Retrieve list of tagged files
                List<ID3File> fileList = finderID3.Process(logger);

                // Initialize
                HandleID3 id3todb = new HandleID3(fileList);
                if (!id3todb.Init(logger))
                {
                    // Something went wrong during initialization
                    logger.Error("Init of DB Connection failed!");
                    return;
                }

                // Read ID3 tags and write them to database
                id3todb.Process(DataCollectorConfig.Instance.ID3Path, logger);

                // End message
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                logger.Info("Import of data tooks {0}", ID3StatsUtil.GetStringFromMs(elapsedMs));
            }
            catch (Exception ex)
            {
                logger.Fatal("Exception in DataCollector");
                logger.Fatal(ex);
            }
        }
    }
}
