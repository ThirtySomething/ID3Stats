using net.derpaul.cdstats.model;

namespace net.derpaul.cdstats.plugin
{
    /// <summary>
    /// Plugin to determine various duration statistics
    /// </summary>
    public class PluginArtistsTracks : PluginBase
    {

        /// <summary>
        /// Get statistic name
        /// </summary>
        public override string Name { get; } = "All tracks per Artists";

        /// <summary>
        /// Collect statistics, main method of plugin
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="outputPath">Path to write own statistics file</param>
        public override void CollectStatistic(CdStats dbConnection, string outputPath)
        {
            var name_file = GetFilename(outputPath);
            var artists_total = dbConnection.MP3Import.Select(a => a.artist).Distinct().OrderBy(a => a).ToList();

            using (StreamWriter statistic_file = new StreamWriter(name_file))
            {
                WriteHeader(statistic_file);

                foreach (var artist in artists_total)
                {
                    var artists_tracks = dbConnection.MP3Import.Where(a => a.artist == artist).Count();
                    var artists_duration_total = dbConnection.MP3Import.Where(a => a.artist == artist).Sum(a => a.durationms);

                    statistic_file.WriteLine("<b>Artists:</b> " + artist + " - " + artists_tracks + " (" + GetStringFromMs(artists_duration_total) + ")<br>");

                    var tracks_total = dbConnection.MP3Import.Where(a => a.artist == artist).OrderBy(a => a.title).ThenBy(a => a.album).ThenBy(a => a.durationms).ToList();
                    foreach (var track in tracks_total)
                    {
                        statistic_file.WriteLine("<b>Track:</b> " + track.title + ", " + track.album + ", (" + GetStringFromMs(track.durationms) + ")<br>");
                    }

                    statistic_file.WriteLine("<p>");
                }
            }
        }
    }
}
