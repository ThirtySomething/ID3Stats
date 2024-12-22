using net.derpaul.mp3stats.model;

namespace net.derpaul.mp3stats.plugin
{
    /// <summary>
    /// Plugin to determine various duration statistics
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
        public override void CollectStatistic(MP3Stats dbConnection, string outputPath, NLog.Logger logger)
        {
            var name_file = GetFilename(outputPath);
            var tracks_double_raw = dbConnection.MP3Import.GroupBy(a => new { a.title })
                .Select(a => new { a.Key.title, Count = a.Count() })
                .Where(a => a.Count > 1)
                .OrderBy(a => a.title)
                .ThenBy(a => a.Count)
                .ToList();

            using (StreamWriter statistic_file = new StreamWriter(name_file))
            {
                MP3StatsUtil.WriteHeader(statistic_file, this.Name);

                foreach (var record in tracks_double_raw)
                {
                    statistic_file.WriteLine("<p>");
                    statistic_file.WriteLine("<b>Track:</b> {0} - <b>Count:</b> {1}<br>",
                        record.title,
                        record.Count
                    );

                    var tracks_double = dbConnection.MP3Import.Where(a => a.title == record.title)
                        .OrderBy(a => a.title)
                        .ThenBy(a => a.artist)
                        .ThenBy(a => a.album)
                        .ThenBy(a => a.durationms)
                        .ToList();

                    foreach (var rec in tracks_double)
                    {
                        statistic_file.WriteLine("<b>Artist:</b> {0} - <b>Album:</b> {1} ({2})<br>",
                            rec.artist,
                            rec.album,
                            MP3StatsUtil.GetStringFromMs(rec.durationms)
                        );
                    }
                    statistic_file.WriteLine("</p>");
                }
            }
        }
    }
}