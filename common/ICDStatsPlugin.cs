using net.derpaul.cdstats.model;

namespace net.derpaul.cdstats
{
    /// <summary>
    /// Interface for all used plugins
    /// </summary>
    public interface ICDStatsPlugin
    {
        /// <summary>
        /// Common initialization method
        /// </summary>
        /// <returns></returns>
        bool Init();

        /// <summary>
        /// Actions before collection of statistics
        /// </summary>
        /// <param name="logger">Passed logger to write infomration</param>
        void PreCollect(NLog.Logger logger);

        /// <summary>
        /// Major entry point of plugin
        /// </summary>
        /// <param name="dbConnection">Valid DB connection object</param>
        /// <param name="outputPath">Path to write own statistics file</param>
        /// <param name="logger">Passed logger to write infomration</param>
        void CollectStatistic(CdStats dbConnection, string outputPath, NLog.Logger logger);

        /// <summary>
        /// Actions after collections of statistics
        /// </summary>
        /// <param name="logger">Passed logger to write infomration</param>
        void PostCollect(NLog.Logger logger);

        /// <summary>
        /// Flags successful initialization
        /// </summary>
        bool IsInitialized { get; set; }

        /// <summary>
        /// InternalName of plugin
        /// </summary>
        string InternalName { get; }

        /// <summary>
        /// Name of statistic
        /// </summary>
        string Name { get; }

    }
}
