namespace net.derpaul.cdstats
{
    public interface IPlugin
    {
        /// <summary>
        /// Flags successful initialization
        /// </summary>
        bool IsInitialized { get; set; }

        /// <summary>
        /// Enable plugin to shutdown some resources
        /// </summary>
        void Shutdown();
    }
}
