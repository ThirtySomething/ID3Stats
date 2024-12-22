using net.derpaul.mp3stats.model;

namespace net.derpaul.mp3stats
{
    /// <summary>
    /// Class to deal with collections of plugins of type IMP3StatsPlugin
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
        private model.MP3Stats DBConnection { get; set; }

        /// <summary>
        /// List of statistic plugins
        /// </summary>
        private List<IMP3StatsPlugin> StatisticPlugins { get; set; }

        /// <summary>
        /// Constructor of plugin handler
        /// </summary>
        /// <param name="pluginPath">Path to plugins</param>
        /// <param name="dbConnection">Connection to DB</param>
        internal PluginHandler(string pluginPath, model.MP3Stats dbConnection)
        {
            PluginPath = pluginPath;
            DBConnection = dbConnection;
            StatisticPlugins = new List<IMP3StatsPlugin>();
        }

        /// <summary>
        /// Initialize statistic plugins
        /// </summary>
        /// <param name="logger">Logger instance</param>
        /// <returns>true on success, otherwise false</returns>
        private bool InitMP3StatsPlugins(NLog.Logger logger)
        {
            StatisticPlugins = PluginLoader<IMP3StatsPlugin>.PluginsLoad(PluginPath, MP3StatsConfig.Instance.PluginProductName);
            if (StatisticPlugins.Count == 0)
            {
                logger.Error("InitMP3StatsPlugins: No statistic plugins found in [{0}].", PluginPath);
                return false;
            }

            foreach (var plugin in StatisticPlugins)
            {
                try
                {
                    plugin.IsInitialized = plugin.Init();
                    if (plugin.IsInitialized)
                    {
                        logger.Info("InitMP3StatsPlugins: Initialized [{0}] plugin.", plugin.InternalName);
                    }
                    else
                    {
                        logger.Error("InitMP3StatsPlugins: Failed to initialize [{0}] plugin.", plugin.InternalName);
                    }
                }
                catch (Exception e)
                {
                    logger.Fatal("InitMP3StatsPlugins: Cannot init plugin [{0}] => [{1}]", plugin.GetType(), e.Message);
                    logger.Fatal("InitMP3StatsPlugins: Inner exception => [{0}]", e.InnerException);
                    continue;
                }
            }

            return true;
        }

        /// <summary>
        /// Initialize all plugins
        /// </summary>
        /// <param name="logger">Logger instance</param>
        /// <returns>true on success, otherwise false</returns>
        internal bool Init(NLog.Logger logger)
        {
            bool InitDone = InitMP3StatsPlugins(logger);
            return InitDone;
        }

        /// <summary>
        /// Run MP3Stats plugins
        /// </summary>
        /// <param name="logger">Logger instance</param>
        internal void Process(NLog.Logger logger)
        {
            var name_dir = Path.GetFullPath(MP3StatsConfig.Instance.PathOutput);
            if (!System.IO.File.Exists(name_dir))
            {
                Directory.CreateDirectory(name_dir);
            }
            var name_file = Path.Combine(name_dir, MP3StatsConfig.Instance.StatisticsMainFile);
            using (StreamWriter statistic_file = new StreamWriter(name_file))
            {
                MP3StatsUtil.WriteHeader(statistic_file, "MP3Stats");

                foreach (var plugin in StatisticPlugins)
                {
                    if (!(plugin is IMP3StatsPlugin))
                    {
                        // Skip invalid plugins
                        continue;
                    }

                    try
                    {
                        // Call plugin statistics
                        logger.Info("Process: Running plugin [{0}].", plugin.InternalName);
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