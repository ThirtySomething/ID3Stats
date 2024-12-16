using net.derpaul.cdstats.model;

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
        public bool Init() {
            return true;
        }

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

        /// <summary>
        /// Common function to generate output path
        /// </summary>
        /// <param name="outputPath"></param>
        /// <returns></returns>
        public string GetFilename(string outputPath)
        {
            var name_file = Path.Combine(outputPath, Name + ".html");
            return name_file;
        }

        /// <summary>
        /// Common function to write header of statistics
        /// </summary>
        /// <param name="statistic_file"></param>
        public void WriteHeader(StreamWriter statistic_file)
        {
            statistic_file.WriteLine("<H1>" + Name + "</H1>");
        }

        /// <summary>
        /// Convert milliseconds into human readable format
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        protected string GetStringFromMs(double ms)
        {
            TimeSpan t = TimeSpan.FromMilliseconds(ms);
            string hrfms = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}:{4:D3}",
                                    t.Days,
                                    t.Hours,
                                    t.Minutes,
                                    t.Seconds,
                                    t.Milliseconds);
            return hrfms;
        }
    }
}