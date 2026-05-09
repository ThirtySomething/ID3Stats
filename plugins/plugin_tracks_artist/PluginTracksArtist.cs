using net.derpaul.id3stats.model;

namespace net.derpaul.id3stats.plugin
{
    /// <summary>
    /// Plugin to determine total number of tracks per artist
    /// </summary>
    public class PluginTracksArtist : PluginBase
    {

        /// <summary>
        /// Get statistic name
        /// </summary>
        public override string Name { get; } = "Total tracks per artist";

        /// <summary>
        /// Major entry point of plugin
        /// </summary>
        /// <param name="dbConnection">Valid DB connection object</param>
        /// <param name="outputPath">Path to write own statistics file</param>
        /// <param name="logger">Passed logger to write infomration</param>
        public override void CollectStatistic(ID3Stats dbConnection, string outputPath, NLog.Logger logger)
        {
            var name_file = GetFilename(outputPath);
            var artists_total = dbConnection.ID3Import.Select(a => a.artist).Distinct().Count();
            var trk_tot = dbConnection.ID3Import.Count();
            var tracks_artists = dbConnection.ID3Import.GroupBy(a => a.artist).Select(a => new { artist = a.Key, tracks = a.Count() }).OrderByDescending(a => a.tracks).ThenBy(a => a.artist).ToList();
            var dur_tot = dbConnection.ID3Import.Sum(myimport => myimport.durationms);

            using (StreamWriter statistic_file = new StreamWriter(name_file))
            {
                ID3StatsUtil.WriteHeader(statistic_file, this.Name, this.GetType().Name);

                ID3StatsUtil.OpenGroupData(statistic_file);
                var tracks_data = String.Format("{0}", trk_tot);
                ID3StatsUtil.WriteTracks(statistic_file, tracks_data);
                var artists_data = String.Format("{0} ({1})", artists_total, ID3StatsUtil.GetStringFromMs(dur_tot));
                ID3StatsUtil.WriteArtist(statistic_file, artists_data);
                ID3StatsUtil.CloseGroupData(statistic_file);
                var tracks_mem = trk_tot;
                var heading = true;
                foreach (var record in tracks_artists)
                {
                    if (tracks_mem != record.tracks)
                    {
                        ID3StatsUtil.CloseGroupData(statistic_file);
                        ID3StatsUtil.OpenGroupData(statistic_file);
                        tracks_mem = record.tracks;
                        heading = true;
                    }
                    var artists_duration_total = dbConnection.ID3Import.Where(a => a.artist == record.artist).Sum(a => a.durationms);
                    if (heading == true)
                    {
                        tracks_data = String.Format("{0}", record.tracks);
                        ID3StatsUtil.WriteTracks(statistic_file, tracks_data);
                        heading = false;
                    }
                    artists_data = String.Format("{0} ({1})", record.artist, ID3StatsUtil.GetStringFromMs(artists_duration_total));
                    ID3StatsUtil.WriteArtist(statistic_file, artists_data);
                }
                ID3StatsUtil.CloseGroupData(statistic_file);
            }
        }
    }
}