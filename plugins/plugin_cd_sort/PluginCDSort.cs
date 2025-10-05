using net.derpaul.id3stats.model;

namespace net.derpaul.id3stats.plugin
{
    /// <summary>
    /// Plugin to sort CD collection by artist sort name and album sort name
    /// </summary>
    public class PluginCDSort : PluginBase
    {

        /// <summary>
        /// Get statistic name
        /// </summary>
        public override string Name { get; } = "CD Sort";

        /// <summary>
        /// Major entry point of plugin
        /// </summary>
        /// <param name="dbConnection">Valid DB connection object</param>
        /// <param name="outputPath">Path to write own statistics file</param>
        /// <param name="logger">Passed logger to write infomration</param>
        public override void CollectStatistic(ID3Stats dbConnection, string outputPath, NLog.Logger logger)
        {
            const string VariousArtists = "Various Artists";
            var name_file = GetFilename(outputPath);
            // Fix: Move the lambda out of the expression tree context to avoid optional argument issues
            var artists = dbConnection.ID3Import.ToList();
            var artists_sorted = artists
                .GroupBy(a => new
                {
                    artist_sort = a.filename.Substring(1, VariousArtists.Length) == VariousArtists
                        ? VariousArtists
                        : a.album_artist_sort.Split(';').First(),
                    a.album
                })
                .Select(a => new { a.Key.artist_sort, a.Key.album })
                .OrderBy(a => a.artist_sort)
                .ThenBy(a => a.album)
                .Distinct()
                .ToList();

            using (StreamWriter statistic_file = new StreamWriter(name_file))
            {
                ID3StatsUtil.WriteHeader(statistic_file, this.Name, this.GetType().Name);
                statistic_file.WriteLine("<p>");
                var artist_mem = "";
                var album_count = 1;
                foreach (var record in artists_sorted)
                {
                    if (0 == record.artist_sort.Length)
                    {
                        continue;
                    }
                    if (artist_mem != record.artist_sort)
                    {
                        statistic_file.WriteLine("</p>");
                        statistic_file.WriteLine("<p>");
                        statistic_file.WriteLine("<b>Artist:</b> {0}<br>", record.artist_sort);
                        artist_mem = record.artist_sort;
                    }
                    statistic_file.WriteLine("<b>Album ({0}):</b> {1}<br>", album_count, record.album);
                    album_count++;
                }
                statistic_file.WriteLine("</p>");
            }
        }
    }
}