using net.derpaul.cdstats.model;

namespace net.derpaul.cdstats.plugin
{
    /// <summary>
    /// Plugin to determine various duration statistics
    /// </summary>
    public class PluginTracksArtist : PluginBase
    {

        /// <summary>
        /// Get statistic name
        /// </summary>
        public override string Name { get; } = "Total tracks per Artists";

        /// <summary>
        /// Major entry point of plugin
        /// </summary>
        /// <param name="dbConnection">Valid DB connection object</param>
        /// <param name="outputPath">Path to write own statistics file</param>
        /// <param name="logger">Passed logger to write infomration</param>
        public override void CollectStatistic(CdStats dbConnection, string outputPath, NLog.Logger logger)
        {
            var name_file = GetFilename(outputPath);
            var artists_total = dbConnection.MP3Import.Select(a => a.artist).Distinct().Count();
            var trk_tot = (from myimport in dbConnection.MP3Import select myimport).Count();
            var tracks_artists = dbConnection.MP3Import.GroupBy(a => a.artist).Select(a => new { artist = a.Key, tracks = a.Count() }).OrderByDescending(a => a.tracks).ThenBy(a => a.artist).ToList();
            var dur_tot = (from myimport in dbConnection.MP3Import select myimport).Sum(myimport => myimport.durationms);

            using (StreamWriter statistic_file = new StreamWriter(name_file))
            {
                WriteHeader(statistic_file);

                statistic_file.WriteLine("<b>Tracks:</b> " + trk_tot + " - <b>Artists:</b> " + artists_total + " (" + GetStringFromMs(dur_tot) + ")");
                var tracks_mem = trk_tot;
                foreach (var record in tracks_artists)
                {
                    if (tracks_mem != record.tracks)
                    {
                        statistic_file.WriteLine("<p>");
                        tracks_mem = record.tracks;
                    }
                    else
                    {
                        statistic_file.WriteLine("<br>");
                    }
                    var artists_duration_total = dbConnection.MP3Import.Where(a => a.artist == record.artist).Sum(a => a.durationms);

                    statistic_file.WriteLine("<b>Tracks:</b> " + record.tracks + " - <b>Artist:</b> " + record.artist + " (" + GetStringFromMs(artists_duration_total) + ")");
                }
            }
        }
    }
}