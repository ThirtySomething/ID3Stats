using net.derpaul.id3stats.model;

namespace net.derpaul.id3stats.plugin
{
    /// <summary>
    /// Plugin to determine find tracks in various versions (different artists, different durations)
    /// </summary>
    public class PluginTrackDoubles : PluginBase
    {

        /// <summary>
        /// Get statistic name
        /// </summary>
        public override string Name { get; } = "Titles with multiple artists";

        /// <summary>
        /// Major entry point of plugin
        /// </summary>
        /// <param name="dbConnection">Valid DB connection object</param>
        /// <param name="outputPath">Path to write own statistics file</param>
        /// <param name="logger">Passed logger to write infomration</param>
        public override void CollectStatistic(ID3Stats dbConnection, string outputPath, NLog.Logger logger)
        {
            var name_file = GetFilename(outputPath);
            var tracks_double_raw = dbConnection.ID3Import.GroupBy(a => new { a.title })
                .Select(a => new { a.Key.title, Count = a.Count() })
                .Where(a => a.Count > 1)
                .OrderBy(a => a.title)
                .ThenBy(a => a.Count)
                .ToList();

            using (StreamWriter statistic_file = new StreamWriter(name_file))
            {
                ID3StatsUtil.WriteHeader(statistic_file, this.Name, this.GetType().Name);

                foreach (var record in tracks_double_raw)
                {
                    ID3StatsUtil.OpenGroupData(statistic_file);
                    ID3StatsUtil.WriteTrack(statistic_file, record.title);
                    var count_data = String.Format("{0}", record.Count);
                    ID3StatsUtil.WriteCount(statistic_file, count_data);

                    var tracks_double = dbConnection.ID3Import.Where(a => a.title == record.title)
                        .OrderBy(a => a.title)
                        .ThenBy(a => a.artist)
                        .ThenBy(a => a.album)
                        .ThenBy(a => a.durationms)
                        .ToList();

                    foreach (var rec in tracks_double)
                    {
                        var album_data = String.Format("{0} ({1})", rec.album, ID3StatsUtil.GetStringFromMs(rec.durationms));
                        ID3StatsUtil.WriteArtist(statistic_file, rec.artist);
                        ID3StatsUtil.WriteAlbum(statistic_file, album_data);
                    }
                    ID3StatsUtil.CloseGroupData(statistic_file);
                }
            }
        }
    }
}