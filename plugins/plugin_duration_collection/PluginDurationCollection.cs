using net.derpaul.id3stats.model;

namespace net.derpaul.id3stats.plugin
{
    /// <summary>
    /// Plugin to determine various duration statistics
    /// </summary>
    public class PluginDurationCollection : PluginBase
    {

        /// <summary>
        /// Get statistic name
        /// </summary>
        public override string Name { get; } = "Collection durations";

        /// <summary>
        /// Major entry point of plugin
        /// </summary>
        /// <param name="dbConnection">Valid DB connection object</param>
        /// <param name="outputPath">Path to write own statistics file</param>
        /// <param name="logger">Passed logger to write infomration</param>
        public override void CollectStatistic(MP3Stats dbConnection, string outputPath, NLog.Logger logger)
        {
            var dur_min = dbConnection.MP3Import.Min(a => a.durationms);
            var dur_avg = dbConnection.MP3Import.Average(a => a.durationms);
            var dur_max = dbConnection.MP3Import.Max(myimport => myimport.durationms);
            var dur_tot = dbConnection.MP3Import.Sum(myimport => myimport.durationms);
            var trk_tot = dbConnection.MP3Import.Count();

            var track_short = dbConnection.MP3Import.Where(track => track.durationms == dur_min).FirstOrDefault();
            var track_long = dbConnection.MP3Import.Where(track => track.durationms == dur_max).FirstOrDefault();

            var name_file = GetFilename(outputPath);
            using (StreamWriter statistic_file = new StreamWriter(name_file))
            {
                MP3StatsUtil.WriteHeader(statistic_file, this.Name, this.GetType().Name);

                statistic_file.WriteLine("<b>Shortest track length:</b> {0} - {1} ({2})<br>",
                    MP3StatsUtil.GetStringFromMs(dur_min),
                    track_short.title ?? "",
                    track_short.artist ?? ""
                );
                statistic_file.WriteLine("<b>Average track length:</b> {0} - {1} tracks<br>",
                    MP3StatsUtil.GetStringFromMs(dur_avg),
                    trk_tot
                );
                statistic_file.WriteLine("<b>Longest track length:</b> {0} - {1} ({2})<br>",
                    MP3StatsUtil.GetStringFromMs(dur_max),
                    track_long.title ?? "",
                    track_long.artist ?? ""
                );
                statistic_file.WriteLine("<b>Playtime overall:</b> {0} - {1} tracks<br>",
                    MP3StatsUtil.GetStringFromMs(dur_tot),
                    trk_tot
                );
            }
        }
    }
}