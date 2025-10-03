namespace net.derpaul.id3stats
{
    /// <summary>
    /// Recursive find all tagged files for given path
    /// </summary>
    internal class FinderID3
    {
        /// <summary>
        /// Start path where to search for
        /// </summary>
        private string pathStartup;

        /// <summary>
        /// Search pattern, e. g. *.mp3 or *.flac
        /// </summary>
        private string searchPattern;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pathStartup">Where to start search</param>
        /// <param name="searchPattern">What to search for</param>
        public FinderID3(string pathStartup, string searchPattern)
        {
            this.pathStartup = pathStartup;
            this.searchPattern = searchPattern;
        }

        /// <summary>
        /// Check if prerequisites are fullfilled
        /// </summary>
        /// <param name="logger">Logger instance</param>
        /// <returns>true on success, otherwise false</returns>
        public bool Init(NLog.Logger logger)
        {
            // Startup directory must exist
            bool ret = Directory.Exists(this.pathStartup);

            if (ret)
            {
                // AND searchpatterns must NOT be empty
                ret = !String.IsNullOrEmpty(this.searchPattern);

                if (!ret)
                {
                    logger.Error("Invalid search pattern [{0}]!", this.searchPattern);
                }
            }
            else
            {
                logger.Error("Path [{0}] does not exist!", this.pathStartup);
            }

            return ret;
        }

        /// <summary>
        /// Find all tagged files
        /// </summary>
        /// <param name="logger">Logger instance</param>
        /// <returns>list of ID3File objects</returns>
        public List<ID3File> Process(NLog.Logger logger)
        {
            logger.Info("Read files in [{0}] with pattern [{1}]", this.pathStartup, this.searchPattern);
            string[] filesraw = Directory.GetFiles(this.pathStartup, this.searchPattern, SearchOption.AllDirectories);
            List<ID3File> files = new List<ID3File>();
            foreach (string currentfile in filesraw)
            {
                ID3File importdata = new ID3File(currentfile);
                files.Add(importdata);
            }
            return files;
        }
    }
}
