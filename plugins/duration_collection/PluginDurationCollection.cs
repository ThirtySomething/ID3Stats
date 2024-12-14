using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Common;

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
        /// <param name="dbConnection"></param>
        /// <param name="outputPath">Path to write own statistics file</param>
        public override void CollectStatistic(CdStats dbConnection, string outputPath)
        {
            var dur_min = (from myimport in dbConnection.MP3Import select myimport).Min(myimport => myimport.durationms);
            var dur_avg = (from myimport in dbConnection.MP3Import select myimport).Average(myimport => myimport.durationms);
            var dur_max = (from myimport in dbConnection.MP3Import select myimport).Max(myimport => myimport.durationms);
            var dur_tot = (from myimport in dbConnection.MP3Import select myimport).Sum(myimport => myimport.durationms);

            var track_short = (from myimport in dbConnection.MP3Import select myimport).Where(track => track.durationms == dur_min).FirstOrDefault();
            var track_long = (from myimport in dbConnection.MP3Import select myimport).Where(track => track.durationms == dur_max).FirstOrDefault();

            var name_file = Path.Combine(outputPath, Name + ".html");
            using (StreamWriter statistic_file = new StreamWriter(name_file))
            {
                statistic_file.WriteLine("<H1>" + Name + "</H1>");

                TimeSpan time = TimeSpan.FromMilliseconds(dur_min);
                DateTime startdate = new DateTime() + time;

                statistic_file.WriteLine("<b>Shortest track length:</b> " + GetStringFromMs(dur_min) + " - " + track_short.title + " (" + track_short.artist + ")<p>");
                statistic_file.WriteLine("<b>Average track length:</b> " + GetStringFromMs(dur_avg) + "<p>");
                statistic_file.WriteLine("<b>Longest track length:</b> " + GetStringFromMs(dur_max) + " - " + track_long.title + " (" + track_long.artist + ")<p>");
                statistic_file.WriteLine("<b>Playtime overall:</b> " + GetStringFromMs(dur_tot) + "<p>");
            }
        }

        /// <summary>
        /// Get statistic name
        /// </summary>
        public override string Name { get; } = "Collection Durations";

        /// <summary>
        /// Convert milliseconds into human readable format
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        private string GetStringFromMs(double ms)
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