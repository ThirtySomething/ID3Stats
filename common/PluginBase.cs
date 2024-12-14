namespace net.derpaul.cdstats
{
    /// <summary>
    /// Abstract base class for all CDStats plugins
    /// </summary>
    public abstract class PluginBase : ICDStatsPlugin
    {
        /// <summary>
        /// Init method of CDStats plugin
        /// </summary>
        /// <returns>signal success with true</returns>
        public abstract bool Init();

        /// <summary>
        /// Flags successful initialization
        /// </summary>
        public bool IsInitialized { get; set; } = false;

        /// <summary>
        /// Dummy method, needs to be implemented in plugin
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="outputPath">Path to write own statistics file</param>
        public abstract void CollectStatistic(CdStats dbConnection, string outputPath);

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
    }
}