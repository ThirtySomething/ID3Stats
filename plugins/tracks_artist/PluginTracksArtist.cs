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
        public override string Name { get; } = "Totoal tracks per Artists";

        /// <summary>
        /// Collect statistics, main method of plugin
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="outputPath">Path to write own statistics file</param>
        public override void CollectStatistic(CdStats dbConnection, string outputPath)
        {
            var name_file = GetFilename(outputPath);
            var artists_total = dbConnection.MP3Import.Select(a => a.artist).Distinct().Count();
            var trk_tot = (from myimport in dbConnection.MP3Import select myimport).Count();

            var query = (from y in (
                            from x in (
                                from myimport in dbConnection.MP3Import
                                group myimport by myimport.artist)
                            select new { Name = x.Key, Total = x.Count() })
                         orderby y.Total descending, y.Name
                         select new { Name = y.Name, Total = y.Total });

            using (StreamWriter statistic_file = new StreamWriter(name_file))
            {
                WriteHeader(statistic_file);

                statistic_file.WriteLine("<b>Artists:</b> " + artists_total + " - <b>Tracks:</b> " + trk_tot);

                var tracks_mem = trk_tot;
                foreach (var record in query)
                {
                    if (tracks_mem != record.Total)
                    {
                        statistic_file.WriteLine("<p>");
                        tracks_mem = record.Total;
                    }
                    else
                    {
                        statistic_file.WriteLine("<br>");
                    }
                    statistic_file.WriteLine("<b>Artists:</b> " + record.Name + " - <b>Tracks:</b> " + record.Total);
                }
            }
        }
    }
}