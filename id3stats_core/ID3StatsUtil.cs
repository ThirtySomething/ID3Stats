using System.Xml.Linq;

namespace net.derpaul.id3stats
{
    public static class ID3StatsUtil
    {
        /// <summary>
        /// Convert milliseconds into human readable format
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static string GetStringFromMs(double ms)
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

        /// <summary>
        /// Common function to write header of statistics
        /// </summary>
        /// <param name="statistic_file"></param>
        public static void WriteHeader(StreamWriter statistic_file, string headline, string classname)
        {
            DateTime thisDate = DateTime.Now;

            statistic_file.WriteLine("<H1>{0}</H1>", headline);
            statistic_file.WriteLine("<p><small>Generated {0} by {1}</small></p>", thisDate.ToString("dd.MM.yyyy HH:mm:ss"), classname);
        }

    }
}
