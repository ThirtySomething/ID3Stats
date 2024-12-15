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
        /// Major entry point
        /// </summary>
        /// <param name="dbConnection">Valid DB connection object</param>
        /// <param name="outputPath">Path to write own statistics file</param>
        void CollectStatistic(CdStats dbConnection, string outputPath);

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
