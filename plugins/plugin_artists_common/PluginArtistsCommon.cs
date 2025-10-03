using net.derpaul.id3stats.model;

namespace net.derpaul.id3stats.plugin
{
    /// <summary>
    /// Plugin to determine various duration statistics
    /// </summary>
    public class PluginArtistsCommon : PluginBase
    {

        /// <summary>
        /// Get statistic name
        /// </summary>
        public override string Name { get; } = "Common artist stats";

        /// <summary>
        /// Major entry point of plugin
        /// </summary>
        /// <param name="dbConnection">Valid DB connection object</param>
        /// <param name="outputPath">Path to write own statistics file</param>
        /// <param name="logger">Passed logger to write infomration</param>
        public override void CollectStatistic(MP3Stats dbConnection, string outputPath, NLog.Logger logger)
        {
            var name_file = GetFilename(outputPath);
            var artists_total = dbConnection.MP3Import.Select(a => a.artist).Distinct().OrderBy(a => a).ToList();

            using (StreamWriter statistic_file = new StreamWriter(name_file))
            {
                MP3StatsUtil.WriteHeader(statistic_file, this.Name, this.GetType().Name);

                foreach (var artist in artists_total)
                {
                    var dur_min = dbConnection.MP3Import.Where(a => a.artist == artist).Min(a => a.durationms);
                    var dur_avg = dbConnection.MP3Import.Where(a => a.artist == artist).Average(a => a.durationms);
                    var dur_max = dbConnection.MP3Import.Where(a => a.artist == artist).Max(myimport => myimport.durationms);
                    var dur_tot = dbConnection.MP3Import.Where(a => a.artist == artist).Sum(myimport => myimport.durationms);
                    var trk_tot = dbConnection.MP3Import.Where(a => a.artist == artist).Count();
                    var alb_tot = dbConnection.MP3Import.Where(a => a.artist == artist).GroupBy(a => new { a.artist, a.album }).Select(a => new { a.Key.album }).Distinct().Count();

                    var track_short = dbConnection.MP3Import.Where(a => a.artist == artist && a.durationms == dur_min).FirstOrDefault();
                    var track_long = dbConnection.MP3Import.Where(a => a.artist == artist && a.durationms == dur_max).FirstOrDefault();

                    statistic_file.WriteLine("<p>");
                    statistic_file.WriteLine("<b>Artist:</b> {0} - <b>Albums:</b> {1} - <b>Tracks:</b> {2}<br>", artist, alb_tot, trk_tot);
                    statistic_file.WriteLine("<b>Shortest track length:</b> {0} - {1} ({2})<br>",
                        MP3StatsUtil.GetStringFromMs(dur_min),
                        track_short.title ?? "",
                        track_short.album ?? ""
                    );
                    statistic_file.WriteLine("<b>Average track length:</b> {0}<br>",
                        MP3StatsUtil.GetStringFromMs(dur_avg)
                    );
                    statistic_file.WriteLine("<b>Longest track length:</b> {0} - {1} ({2})<br>",
                        MP3StatsUtil.GetStringFromMs(dur_max),
                        track_long.title ?? "",
                        track_long.album ?? ""
                    );
                    statistic_file.WriteLine("<b>Playtime overall:</b> {0}<br>",
                        MP3StatsUtil.GetStringFromMs(dur_tot)
                    );
                    statistic_file.WriteLine("</p>");
                }
            }
        }
    }
}
