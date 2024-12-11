namespace net.derpaul.cdstats
{
    internal class CDStats
    {
        static void Main(string[] args)
        {
            // Show current configuration
            CDStatsConfig.Instance.ShowConfig();
            CDStatsConfig.Instance.Save();
        }
    }
}
