using ATL;
using Microsoft.EntityFrameworkCore;
using net.derpaul.id3stats.model;
using Newtonsoft.Json.Linq;

namespace net.derpaul.id3stats
{
    /// <summary>
    /// Extract meta data of tagged file
    /// </summary>
    internal class HandleID3
    {
        /// <summary>
        /// List of tagged files
        /// </summary>
        private List<ID3File> filenamesID3;

        /// <summary>
        /// Connection to database (Entity Framework)
        /// </summary>
        private ID3Stats DBInstance;

        /// <summary>
        /// Pass list of tagged files to handler in constructor
        /// </summary>
        /// <param name="filenamesID3"></param>
        public HandleID3(List<ID3File> filenamesID3)
        {
            this.filenamesID3 = filenamesID3;
        }

        /// <summary>
        /// Init of database
        /// </summary>
        /// <param name="logger">Logger instance</param>
        /// <returns>true on success</returns>
        /// <returns>false on failure</returns>
        public bool Init(NLog.Logger logger)
        {
            bool ret;

            try
            {
                DBInstance = new ID3Stats(new DbContextOptions<ID3Stats>());
                DBInstance.Database.EnsureCreated();
                ret = true;
            }
            catch (Exception ex)
            {
                logger.Fatal("Cannot init DB connection");
                logger.Fatal("{0}", ex);
                ret = false;
            }

            return ret;
        }

        /// <summary>
        /// Update ERM object with ID3 data
        /// </summary>
        /// <param name="dbObject"></param>
        /// <param name="metaData"></param>
        private void setMetaData(ID3Import dbObject, Track metaData)
        {
            dbObject.artist = metaData.Artist;
            dbObject.artist_sort = metaData.SortArtist;
            dbObject.album = metaData.Album;
            dbObject.album_sort = metaData.SortAlbum;
            dbObject.album_artist = metaData.AlbumArtist;
            dbObject.album_artist_sort = metaData.SortAlbumArtist;
            dbObject.title = metaData.Title;
            dbObject.title_sort = metaData.SortTitle;
            dbObject.genre = metaData.Genre;
            dbObject.durationms = metaData.DurationMs;
            dbObject.tracknumber = metaData.TrackNumber ?? 0;
            dbObject.tracktotal = metaData.TrackTotal ?? 0;
            dbObject.year = metaData.Year ?? 0;
            dbObject.discnumber = metaData.DiscNumber ?? 0;
            dbObject.disctotal = metaData.DiscTotal ?? 0;
            dbObject.bitrate = metaData.Bitrate;
            dbObject.samplerate = metaData.SampleRate;

            var dataTranslation = JObject.Parse(DataCollectorConfig.Instance.DataTranslation);
            foreach (var item in dataTranslation)
            {
                if (dbObject.artist.Equals(item.Key))
                {
                    dbObject.artist = item.Value.ToString();
                }
            }
        }

        /// <summary>
        /// Major process of import
        /// </summary>
        /// <param name="logger">Logger instance</param>
        /// <param name="pathprefix"></param>
        public void Process(string pathprefix, NLog.Logger logger)
        {
            foreach (ID3File ofile in filenamesID3)
            {
                // Read ID3 tag
                var tagID3 = new Track(ofile.filename);

                // Strip prefix
                string pname = ofile.filename;
                pname = pname.Replace(pathprefix, "");

                // Hash handling
                if (true == DataCollectorConfig.Instance.UseHash)
                {
                    ofile.filehash = ofile.CalculateMD5(ofile.filename);
                }

                // Handle ID3 import
                var OID3Import = DBInstance.ID3Import.Where(a => a.filename == pname).FirstOrDefault();
                if (null == OID3Import)
                {
                    // Record not in database, init with file data data
                    OID3Import = new ID3Import();
                    OID3Import.filename = pname;
                    OID3Import.filehash = ofile.filehash;
                    OID3Import.date_import = DateTime.Now;
                    OID3Import.date_file_mod = ofile.date_file_mod;
                    DBInstance.Add(OID3Import);

                    // Enlarge record with ID3 meta data
                    setMetaData(OID3Import, tagID3);

                    // Save to database
                    DBInstance.SaveChanges();

                    logger.Info("Fresh import of track [{0}]", pname);

                    // Continue with next file
                    continue;
                }

                if ((OID3Import.date_file_mod == ofile.date_file_mod)
                    && (OID3Import.filehash == ofile.filehash)
                    )
                {
                    logger.Info("Skip unchanged track [{0}]", pname);

                    // No changes since last import, data already present
                    continue;
                }

                // Update file meta data
                OID3Import.date_file_mod = ofile.date_file_mod;
                OID3Import.filehash = ofile.filehash;

                // Update ID3 meta data in record
                setMetaData(OID3Import, tagID3);

                // Save to database
                DBInstance.SaveChanges();

                logger.Info("Updated [{0}]", pname);
            }
        }
    }
}
