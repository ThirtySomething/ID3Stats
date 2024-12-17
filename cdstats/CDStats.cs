using Microsoft.EntityFrameworkCore;
using net.derpaul.cdstats.model;
using NLog;

namespace net.derpaul.cdstats
{
    /// <summary>
    /// Class to call several statistics implemented in plugins
    /// </summary>
    internal class CDStats
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
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "cdstats.log" };
            configuration.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, logfile);
            NLog.LogManager.Configuration = configuration;

            // Show current configuration
            CDStatsConfig.Instance.ShowConfig();
            CDStatsConfig.Instance.Save();

            logger.Debug("Startup of CDStats at {now}", DateTime.Now);

            // Create DB instance for usage in plugins
            CdStats DBInstance = new CdStats(new DbContextOptions<CdStats>());
            DBInstance.Database.EnsureCreated();

            // Bring plugin handler to live
            var pluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CDStatsConfig.Instance.PathPlugin);
            var pluginHandler = new PluginHandler(pluginPath, DBInstance);

            // Abort on init failure
            if (!pluginHandler.Init())
            {
                return;
            }

            // Run plugins
            pluginHandler.Process(logger);
        }
    }
}
