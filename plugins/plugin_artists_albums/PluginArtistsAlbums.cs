using net.derpaul.id3stats.model;

namespace net.derpaul.id3stats.plugin
{
    /// <summary>
    /// Plugin to determine all albums per artist
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
        public override void CollectStatistic(ID3Stats dbConnection, string outputPath, NLog.Logger logger)
        {
            var name_file = GetFilename(outputPath);
            var artists_total = dbConnection.ID3Import.Select(a => a.artist).Distinct().OrderBy(a => a).ToList();

            using (StreamWriter statistic_file = new StreamWriter(name_file))
            {
                ID3StatsUtil.WriteHeader(statistic_file, this.Name, this.GetType().Name);

                foreach (var artist in artists_total)
                {
                    var artist_albums = dbConnection.ID3Import.Where(a => a.artist == artist).GroupBy(a => new { a.artist, a.album }).Select(a => new { a.Key.artist, a.Key.album }).ToList();

                    ID3StatsUtil.OpenGroupData(statistic_file);
                    ID3StatsUtil.WriteArtist(statistic_file, artist);
                    foreach (var album in artist_albums)
                    {
                        var artists_tracks = dbConnection.ID3Import.Where(a => a.artist == artist && a.album == album.album).Count();
                        var artists_duration_total = dbConnection.ID3Import.Where(a => a.artist == artist && a.album == album.album).Sum(a => a.durationms);
                        ID3StatsUtil.WriteAlbum(statistic_file, album.album);
                        var tracks_data = String.Format("{0} ({1})", artists_tracks, ID3StatsUtil.GetStringFromMs(artists_duration_total));
                        ID3StatsUtil.WriteTracks(statistic_file, tracks_data);
                    }
                    ID3StatsUtil.CloseGroupData(statistic_file);
                }
            }
        }
    }
}
