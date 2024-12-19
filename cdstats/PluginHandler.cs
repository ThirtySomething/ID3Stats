using net.derpaul.cdstats.model;

namespace net.derpaul.cdstats
{
    /// <summary>
    /// Class to deal with collections of plugins of type ICDStatsPlugin
    /// </summary>
    internal class PluginHandler
    {
        /// <summary>
        /// The path to the plugins
        /// </summary>
        private string PluginPath { get; set; }

        /// <summary>
        /// DB Connection object
        /// </summary>
        private CdStats DBConnection { get; set; }

        /// <summary>
        /// List of statistic plugins
        /// </summary>
        private List<ICDStatsPlugin> StatisticPlugins { get; set; }

        /// <summary>
        /// Constructor of plugin handler
        /// </summary>
        /// <param name="pluginPath">Path to plugins</param>
        /// <param name="dbConnection">Connection to DB</param>
        internal PluginHandler(string pluginPath, CdStats dbConnection)
        {
            PluginPath = pluginPath;
            DBConnection = dbConnection;
            StatisticPlugins = new List<ICDStatsPlugin>();
        }

        /// <summary>
        /// Initialize statistic plugins
        /// </summary>
        /// <returns>true on success, otherwise false</returns>
        private bool InitCDStatsPlugins()
        {
            StatisticPlugins = PluginLoader<ICDStatsPlugin>.PluginsLoad(PluginPath, CDStatsConfig.Instance.PluginProductName);
            if (StatisticPlugins.Count == 0)
            {
                System.Console.WriteLine("InitCDStatsPlugins: No statistic plugins found in [{0}].", PluginPath);
                return false;
            }

            foreach (var plugin in StatisticPlugins)
            {
                try
                {
                    plugin.IsInitialized = plugin.Init();
                    if (plugin.IsInitialized)
                    {
                        System.Console.WriteLine("InitCDStatsPlugins: Initialized [{0}] plugin.", plugin.InternalName);
                    }
                    else
                    {
                        System.Console.WriteLine("InitCDStatsPlugins: Failed to initialize [{0}] plugin.", plugin.InternalName);
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("InitCDStatsPlugins: Cannot init plugin [{0}] => [{1}]", plugin.GetType(), e.Message);
                    System.Console.WriteLine("InitCDStatsPlugins: Inner exception => [{0}]", e.InnerException);
                    continue;
                }
            }

            return true;
        }

        /// <summary>
        /// Initialize all plugins
        /// </summary>
        /// <returns>true on success, otherwise false</returns>
        internal bool Init()
        {
            bool InitDone = InitCDStatsPlugins();
            return InitDone;
        }

        /// <summary>
        /// Run CDStats plugins
        /// </summary>
        /// <param name="logger">Logger instance</param>
        internal void Process(NLog.Logger logger)
        {
            var name_dir = Path.GetFullPath(CDStatsConfig.Instance.PathOutput);
            if (!System.IO.File.Exists(name_dir))
            {
                Directory.CreateDirectory(name_dir);
            }
            var name_file = Path.Combine(name_dir, CDStatsConfig.Instance.StatisticsMainFile);
            using (StreamWriter statistic_file = new StreamWriter(name_file))
            {
                statistic_file.WriteLine("<H1>CDStats</H1>");
                foreach (var plugin in StatisticPlugins)
                {
                    if (!(plugin is ICDStatsPlugin))
                    {
                        // Skip invalid plugins
                        continue;
                    }

                    try
                    {
                        // Call plugin statistics
                        System.Console.WriteLine("Process: Running plugin [{0}].", plugin.InternalName);
                        plugin.PreCollect(logger);
                        plugin.CollectStatistic(DBConnection, name_dir, logger);
                        plugin.PostCollect(logger);
                        statistic_file.WriteLine("<a href='{0}.html'>{1}</a><br>", plugin.Name, plugin.Name);
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("Exception in plugin '{0}':", plugin.Name);
                        logger.Fatal(ex);
                    }
                }
            }
        }
    }
}