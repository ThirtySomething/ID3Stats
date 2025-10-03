using net.derpaul.id3stats.model;
using System.Diagnostics;

namespace net.derpaul.id3stats
{
    /// <summary>
    /// Abstract base class for all MP3Stats plugins
    /// </summary>
    public abstract class PluginBase : IMP3StatsPlugin
    {
        /// <summary>
        /// Internal timer for time measurement
        /// </summary>
        Stopwatch watch;

        /// <summary>
        /// Init method of MP3Stats plugin
        /// </summary>
        /// <returns>signal success with true</returns>
        public bool Init()
        {
            return true;
        }

        /// <summary>
        /// Flags successful initialization
        /// </summary>
        public bool IsInitialized { get; set; } = false;

        /// <summary>
        /// Actions before collection of statistics
        /// </summary>
        /// <param name="logger">Passed logger to write infomration</param>
        public void PreCollect(NLog.Logger logger)
        {
            watch = System.Diagnostics.Stopwatch.StartNew();
        }

        /// <summary>
        /// Dummy method, needs to be implemented in plugin
        /// </summary>
        /// <param name="dbConnection">Valid DB connection object</param>
        /// <param name="outputPath">Path to write own statistics file</param>
        /// <param name="logger">Passed logger to write infomration</param>
        public abstract void CollectStatistic(MP3Stats dbConnection, string outputPath, NLog.Logger logger);

        /// <summary>
        /// Actions after collections of statistics
        /// </summary>
        /// <param name="logger">Passed logger to write infomration</param>
        public void PostCollect(NLog.Logger logger)
        {
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            logger.Info("Running plugin '{0}' tooks {1}", Name, MP3StatsUtil.GetStringFromMs(elapsedMs));
        }

        /// <summary>
        /// Get the name of plugin class
        /// </summary>
        public string InternalName
        {
            get
            {
                return this.GetType().Name;
            }
        }

        /// <summary>
        /// Name of statistic
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Common function to generate output path
        /// </summary>
        /// <param name="outputPath"></param>
        /// <returns></returns>
        public string GetFilename(string outputPath)
        {
            var name_file = Path.Combine(outputPath, Name + ".html");
            return name_file;
        }
    }
}