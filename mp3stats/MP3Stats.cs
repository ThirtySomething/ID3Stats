using Microsoft.EntityFrameworkCore;
using NLog;
using System.Diagnostics;

namespace net.derpaul.mp3stats
{
    /// <summary>
    /// Class to call several statistics implemented in plugins
    /// </summary>
    internal class MP3Stats
    {
        /// <summary>
        /// Logger for writing log files
        /// </summary>
        private static NLog.Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Entrance point for creating statistics
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Setup of logger
            var configuration = new NLog.Config.LoggingConfiguration();
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "mp3stats.log" };
            configuration.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, logfile);
            NLog.LogManager.Configuration = configuration;

            // Ensure configuration
            MP3StatsConfig.Instance.ShowConfig(logger);
            MP3StatsConfig.Instance.Save();

            try
            {
                Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();

                logger.Info("Startup of MP3Stats at {now}", DateTime.Now);

                // Create DB instance for usage in plugins
                model.MP3Stats DBInstance = new model.MP3Stats(new DbContextOptions<model.MP3Stats>());
                DBInstance.Database.EnsureCreated();

                // Bring plugin handler to live
                var pluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, MP3StatsConfig.Instance.PathPlugin);
                var pluginHandler = new PluginHandler(pluginPath, DBInstance);

                // Abort on init failure
                if (!pluginHandler.Init(logger))
                {
                    logger.Error("Init of MP3Stats failed!");
                    return;
                }

                // Run plugins
                pluginHandler.Process(logger);

                // End message
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                logger.Info("Generation of statistics tooks {0}", MP3StatsUtil.GetStringFromMs(elapsedMs));
            }
            catch (Exception ex)
            {
                logger.Fatal("Exception in MP3Stats");
                logger.Fatal(ex);
            }
        }
    }
}
