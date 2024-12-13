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
        void CollectStatistic(CdStats dbConnection);

        /// <summary>
        /// Flags successful initialization
        /// </summary>
        bool IsInitialized { get; set; }

        /// <summary>
        /// Name of plugin
        /// </summary>
        string Name { get; }

    }
}
