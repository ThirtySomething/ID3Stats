namespace net.derpaul.cdstats.plugin
{
    /// <summary>
    /// Plugin to determine various duration statistics
    /// </summary>
    public class PluginDurationCollection : PluginBase
    {
        /// <summary>
        ///  Required implementation
        /// </summary>
        /// <returns></returns>
        public override bool Init()
        {
            return true;
        }

        /// <summary>
        /// Collect statistics, main method of plugin
        /// </summary>
        /// <param name="dbConnection">Connection to CDStats database</param>
        public override void CollectStatistic(CdStats dbConnection)
        {

        }
    }
}
