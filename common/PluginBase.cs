namespace net.derpaul.cdstats
{
    /// <summary>
    /// Abstract base class for all CDStats plugins
    /// </summary>
    public abstract class PluginBase : ICDStatsPlugin
    {
        /// <summary>
        /// Object to lock on when writing
        /// </summary>
        private Object Locker = new Object();

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
        public abstract void CollectStatistic(CdStats dbConnection);

        /// <summary>
        /// Get the name of plugin class
        /// </summary>
        public string Name
        {
            get
            {
                return this.GetType().Name;
            }
        }

        /// <summary>
        /// Lock object for preventing concurrent access to same plugin
        /// </summary>
        protected object WriteLock { get { return Locker; } }
    }
}