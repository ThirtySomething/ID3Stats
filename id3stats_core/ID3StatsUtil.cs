using System.Runtime.InteropServices.Swift;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace net.derpaul.id3stats
{
    public static class ID3StatsUtil
    {
        /// <summary>
        /// Convert milliseconds into human readable format
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static string GetStringFromMs(double ms)
        {
            TimeSpan t = TimeSpan.FromMilliseconds(ms);
            string hrfms = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}:{4:D3}",
                                    t.Days,
                                    t.Hours,
                                    t.Minutes,
                                    t.Seconds,
                                    t.Milliseconds);
            return hrfms;
        }

        /// <summary>
        /// Write the opening structure of the statistics HTML document
        /// </summary>
        /// <param name="statistic_file">File to write to</param>
        /// <param name="title">Document title</param>
        public static void WriteHtmlDocumentStart(StreamWriter statistic_file, string title)
        {
            statistic_file.WriteLine("<!DOCTYPE html>");
            statistic_file.WriteLine("<html lang='de-DE' class='full'>");
            statistic_file.WriteLine("<head>");
            statistic_file.WriteLine("<meta charset='utf-8'>");
            statistic_file.WriteLine("<link rel='stylesheet' href='id3stats.css'>");
            statistic_file.WriteLine("<title>{0}</title>", title);
            statistic_file.WriteLine("</head>");
            statistic_file.WriteLine("<body>");
        }

        /// <summary>
        /// Write the shared stylesheet to the output directory
        /// </summary>
        /// <param name="outputPath">Directory for generated HTML and CSS</param>
        public static void WriteHtmlStylesheet(string outputPath)
        {
            const string resourceName = "net.derpaul.id3stats.id3stats.css";
            using Stream stylesheet = typeof(ID3StatsUtil).Assembly.GetManifestResourceStream(resourceName)
                ?? throw new InvalidOperationException($"Embedded stylesheet resource '{resourceName}' was not found.");
            using FileStream output = File.Create(Path.Combine(outputPath, "id3stats.css"));
            stylesheet.CopyTo(output);
        }

        /// <summary>
        /// Write the closing structure of the statistics HTML document
        /// </summary>
        /// <param name="statistic_file">File to write to</param>
        public static void WriteHtmlDocumentEnd(StreamWriter statistic_file)
        {
            statistic_file.WriteLine("</body>");
            statistic_file.WriteLine("</html>");
        }

        /// <summary>
        /// Common function to write header of statistics
        /// </summary>
        /// <param name="statistic_file">File to write to</param>
        /// <param name="headline">Headline to write</param>
        /// <param name="classname">Classname to write</param>
        public static void WriteHeader(StreamWriter statistic_file, string headline, string classname)
        {
            DateTime thisDate = DateTime.Now;

            ID3StatsUtil.GroupOpen(statistic_file, "id3stats_groupheader");

            ID3StatsUtil.GroupOpen(statistic_file, "id3stats_header");
            statistic_file.WriteLine("{0}", headline);
            ID3StatsUtil.GroupClose(statistic_file);

            ID3StatsUtil.GroupOpen(statistic_file, "id3stats_generator");
            statistic_file.WriteLine("Generated {0} by {1}", thisDate.ToString("dd.MM.yyyy HH:mm:ss"), classname);
            ID3StatsUtil.GroupClose(statistic_file);

            ID3StatsUtil.GroupClose(statistic_file);
        }

        /// <summary>
        /// Open group for writing
        /// </summary>
        /// <param name="statistic_file">File to write to</param>
        /// <param name="groupname">Name of group</param>
        private static void GroupOpen(StreamWriter statistic_file, string groupname)
        {
            statistic_file.WriteLine("<div id='{0}'>", groupname);
        }

        private static void GroupOpen(StreamWriter statistic_file, string groupname, string className)
        {
            statistic_file.WriteLine("<div id='{0}' class='{1}'>", groupname, className);
        }

        /// <summary>
        /// Close group for writing
        /// </summary>
        /// <param name="statistic_file">File to write to</param>
        private static void GroupClose(StreamWriter statistic_file)
        {
            statistic_file.WriteLine("</div>");
        }

        /// <summary>
        /// Write complete group with label and data to statistic file
        /// </summary>
        /// <param name="statistic_file">File to write to</param>
        /// <param name="groupname">Name of group</param>
        /// <param name="label">Data label</param>
        /// <param name="data">Data</param>
        private static void WriteGroupData(StreamWriter statistic_file, string groupname, string label, string data)
        {
            ID3StatsUtil.GroupOpen(statistic_file, groupname);

            ID3StatsUtil.GroupOpen(statistic_file, "id3stats_label");
            statistic_file.WriteLine("{0}:", label);
            ID3StatsUtil.GroupClose(statistic_file);

            ID3StatsUtil.GroupOpen(statistic_file, "id3stats_data");
            statistic_file.WriteLine("{0}", data);
            ID3StatsUtil.GroupClose(statistic_file);

            ID3StatsUtil.GroupClose(statistic_file);
        }

        /// <summary>
        /// Write artist information to statistic file
        /// </summary>
        /// <param name="statistic_file">File to write to</param>
        /// <param name="artist">Artist name</param>
        public static void WriteArtist(StreamWriter statistic_file, string artist)
        {
            ID3StatsUtil.WriteGroupData(statistic_file, "id3stats_artist", "Artist", artist);
        }

        /// <summary>
        /// Write album information to statistic file
        /// </summary>
        /// <param name="statistic_file">File to write to</param>
        /// <param name="album">Album name</param>
        public static void WriteAlbum(StreamWriter statistic_file, string album)
        {
            ID3StatsUtil.WriteGroupData(statistic_file, "id3stats_album", "Album", album);
        }

        /// <summary>
        /// Write tracks information to statistic file
        /// </summary>
        /// <param name="statistic_file">File to write to</param>
        /// <param name="tracks">Track data</param>
        public static void WriteTracks(StreamWriter statistic_file, string tracks)
        {
            ID3StatsUtil.WriteGroupData(statistic_file, "id3stats_tracks", "Tracks", tracks);
        }

        /// <summary>
        /// Write track information to statistic file
        /// </summary>
        /// <param name="statistic_file">File to write to</param>
        /// <param name="track">Track data</param>
        public static void WriteTrack(StreamWriter statistic_file, string track)
        {
            ID3StatsUtil.WriteGroupData(statistic_file, "id3stats_track", "Track", track);
        }

        /// <summary>
        /// Write count information to statistic file
        /// </summary>
        /// <param name="statistic_file">File to write to</param>
        /// <param name="count">Count data</param>
        public static void WriteCount(StreamWriter statistic_file, string count)
        {
            ID3StatsUtil.WriteGroupData(statistic_file, "id3stats_count", "Count", count);
        }

        /// <summary>
        /// To open a data group
        /// </summary>
        /// <param name="statistic_file">File to write to</param>
        public static void OpenGroupData(StreamWriter statistic_file)
        {
            ID3StatsUtil.GroupOpen(statistic_file, "id3stats_group");
        }

        /// <summary>
        /// Open a data group for the all-albums-per-artist layout
        /// </summary>
        /// <param name="statistic_file">File to write to</param>
        public static void OpenAlbumArtistGroupData(StreamWriter statistic_file)
        {
            ID3StatsUtil.GroupOpen(statistic_file, "id3stats_group", "id3stats_album_artist");
        }

        /// <summary>
        /// Open a data group for the all-tracks-per-artist layout
        /// </summary>
        /// <param name="statistic_file">File to write to</param>
        public static void OpenArtistTracksGroupData(StreamWriter statistic_file)
        {
            ID3StatsUtil.GroupOpen(statistic_file, "id3stats_group", "id3stats_artist_tracks");
        }

        /// <summary>
        /// To close a data group
        /// </summary>
        /// <param name="statistic_file">File to write to</param>
        public static void CloseGroupData(StreamWriter statistic_file)
        {
            ID3StatsUtil.GroupClose(statistic_file);
        }

        /// <summary>
        /// To write statistics
        /// </summary>
        /// <param name="statistic_file">File to write to</param>
        /// <param name="stats_min">Min data</param>
        /// <param name="stats_avg">Avg data</param>
        /// <param name="stats_max">Max data</param>
        /// <param name="stats_tot">Tot data</param>
        public static void WriteArtistStats(StreamWriter statistic_file, string stats_min, string stats_avg, string stats_max, string stats_tot)
        {
            ID3StatsUtil.WriteGroupData(statistic_file, "id3stats_tme_min", "Shortest track length", stats_min);
            ID3StatsUtil.WriteGroupData(statistic_file, "id3stats_tme_avg", "Average track length", stats_avg);
            ID3StatsUtil.WriteGroupData(statistic_file, "id3stats_tme_max", "Longest track length", stats_max);
            ID3StatsUtil.WriteGroupData(statistic_file, "id3stats_tme_tot", "Playtime overall", stats_tot);
        }
    }
}
