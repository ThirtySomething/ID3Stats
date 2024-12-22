using net.derpaul.mp3stats.model;

namespace net.derpaul.mp3stats.plugin
{
    /// <summary>
    /// Plugin to determine various duration statistics
    /// </summary>
    public class PluginArtistsTracks : PluginBase
    {

        /// <summary>
        /// Get statistic name
        /// </summary>
        public override string Name { get; } = "All tracks per artist";

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
                MP3StatsUtil.WriteHeader(statistic_file, this.Name);

                foreach (var artist in artists_total)
                {
                    var artists_tracks = dbConnection.MP3Import.Where(a => a.artist == artist).Count();
                    var artists_duration_total = dbConnection.MP3Import.Where(a => a.artist == artist).Sum(a => a.durationms);

                    statistic_file.WriteLine("<b>Artists:</b> {0} - {1} ({2})<br>",
                        artist,
                        artists_tracks,
                        MP3StatsUtil.GetStringFromMs(artists_duration_total)
                    );

                    var tracks_total = dbConnection.MP3Import.Where(a => a.artist == artist).OrderBy(a => a.title).ThenBy(a => a.album).ThenBy(a => a.durationms).ToList();
                    foreach (var track in tracks_total)
                    {
                        statistic_file.WriteLine("<b>Track:</b> {0} - {1} ({2})<br>",
                            track.title,
                            track.album,
                            MP3StatsUtil.GetStringFromMs(track.durationms)
                        );
                    }

                    statistic_file.WriteLine("<p>");
                }
            }
        }
    }
}
