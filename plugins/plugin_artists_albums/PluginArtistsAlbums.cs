using net.derpaul.mp3stats.model;

namespace net.derpaul.mp3stats.plugin
{
    /// <summary>
    /// Plugin to determine various duration statistics
    /// </summary>
    public class PluginArtistsAlbums : PluginBase
    {

        /// <summary>
        /// Get statistic name
        /// </summary>
        public override string Name { get; } = "All albums per artist";

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
                    var artist_albums = dbConnection.MP3Import.Where(a => a.artist == artist).GroupBy(a => new { a.artist, a.album }).Select(a => new { a.Key.artist, a.Key.album }).ToList();

                    foreach (var album in artist_albums)
                    {
                        var artists_tracks = dbConnection.MP3Import.Where(a => a.artist == artist && a.album == album.album).Count();
                        var artists_duration_total = dbConnection.MP3Import.Where(a => a.artist == artist && a.album == album.album).Sum(a => a.durationms);
                        statistic_file.WriteLine("<b>Artist:</b> {0} - <b>Album:</b> {1} - <b>Tracks:</b> {2} ({3})<br>",
                            album.artist,
                            album.album,
                            artists_tracks,
                            MP3StatsUtil.GetStringFromMs(artists_duration_total));
                    }
                    statistic_file.WriteLine("<p>");
                }
            }
        }
    }
}
