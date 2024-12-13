using Microsoft.EntityFrameworkCore;

namespace net.derpaul.cdstats
{
    /// <summary>
    /// Class to call several statistics implemented in plugins
    /// </summary>
    internal class CDStats
    {
        static void Main(string[] args)
        {
            // Show current configuration
            CDStatsConfig.Instance.ShowConfig();
            CDStatsConfig.Instance.Save();

            // Create DB instance for usage in plugins
            CdStats DBInstance = new CdStats(new DbContextOptions<CdStats>());
            DBInstance.Database.EnsureCreated();

            // Bring plugin handler to live
            // var pluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CDStatsConfig.Instance.PathPlugin);
            var pluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "");
            var pluginHandler = new PluginHandler(pluginPath, DBInstance);

            // Abort on init failure
            if (!pluginHandler.Init())
            {
                return;
            }

            // Run plugins
            pluginHandler.Process();
        }
    }
}
