using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net.derpaul.cdstats
{
    /// <summary>
    /// Recursive find all MP3 files for given path
    /// </summary>
    internal class FinderMP3
    {
        /// <summary>
        /// Start path where to search for
        /// </summary>
        private string pathStartup;

        /// <summary>
        /// Search pattern, e. g. *.mp3
        /// </summary>
        private string searchPattern;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pathStartup">Where to start search</param>
        /// <param name="searchPattern">What to search for</param>
        public FinderMP3(string pathStartup, string searchPattern)
        {
            this.pathStartup = pathStartup;
            this.searchPattern = searchPattern;
        }

        /// <summary>
        /// Check if prerequisites are fullfilled
        /// </summary>
        /// <returns></returns>
        public bool Init()
        {
            // Startup directory must exist
            bool ret = Directory.Exists(this.pathStartup);

            if (ret)
            {
                // AND searchpatterns must NOT be empty
                ret = !String.IsNullOrEmpty(this.searchPattern);
            }

            return ret;
        }

        /// <summary>
        /// Find all MP3 files
        /// </summary>
        /// <returns></returns>
        public List<MP3File> Process()
        {
            System.Console.WriteLine($"Read files in [{this.pathStartup}] with pattern [{this.searchPattern}]");
            string[] filesraw = Directory.GetFiles(this.pathStartup, this.searchPattern, SearchOption.AllDirectories);
            List<MP3File> files = new List<MP3File>();
            foreach (string currentfile in filesraw)
            {
                MP3File importdata = new MP3File(currentfile);
                files.Add(importdata);
            }
            return files;
        }
    }
}
