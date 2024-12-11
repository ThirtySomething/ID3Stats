using ATL;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text.Json.Nodes;

namespace net.derpaul.cdstats
{
    internal class HandleID3
    {
        private List<MP3File> filenamesMP3;
        private CdStats DBInstance;

        public HandleID3(List<MP3File> filenamesMP3)
        {
            this.filenamesMP3 = filenamesMP3;
        }

        public bool Init()
        {
            bool ret = true;

            DBInstance = new CdStats(new DbContextOptions<CdStats>());
            DBInstance.Database.EnsureCreated();

            return ret;
        }

        public string CalculateMD5(MP3File ofile)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(ofile.filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToUpperInvariant();
                }
            }
        }

        private void setMetaData(MP3Import dbObject, Track metaData)
        {
            dbObject.artist = metaData.Artist;
            dbObject.album = metaData.Album;
            dbObject.title = metaData.Title;
            dbObject.genre = metaData.Genre;
            dbObject.durationms = metaData.DurationMs;
            dbObject.tracknumber = metaData.TrackNumber ?? 0;
            dbObject.tracktotal = metaData.TrackTotal ?? 0;
            dbObject.year = metaData.Year ?? 0;
            dbObject.discnumber = metaData.DiscNumber ?? 0;
            dbObject.disctotal = metaData.DiscTotal ?? 0;
            dbObject.bitrate = metaData.Bitrate;
            dbObject.samplerate = metaData.SampleRate;

            if (dbObject.filename.StartsWith("ac_dc"))
            {
                var dataTranslation = JObject.Parse(DataCollectorConfig.Instance.DataTranslation);
                foreach (var item in dataTranslation)
                {
                    if (dbObject.artist.Equals(item.Key))
                    {
                        dbObject.artist = item.Value.ToString();
                    }
                }
            }
        }

        public void Process(string pathprefix)
        {
            foreach (MP3File ofile in filenamesMP3)
            {
                // Read ID3 tag
                var tagID3 = new Track(ofile.filename);

                // Strip prefix
                string pname = ofile.filename;
                pname = pname.Replace(pathprefix, "");

                //string file_hash = CalculateMD5(ofile);

                // Handle MP3 import
                var OMP3Import = DBInstance.MP3Import.Where(a => a.filename == pname).FirstOrDefault();
                if (null == OMP3Import)
                {
                    // Record not in database, init with file data data
                    OMP3Import = new MP3Import();
                    OMP3Import.filename = pname;
                    //OMP3Import.file_hash = file_hash;
                    OMP3Import.date_file_mod = ofile.date_file_mod;
                    DBInstance.Add(OMP3Import);

                    // Enlarge record with ID3 meta data
                    setMetaData(OMP3Import, tagID3);

                    // Save to database
                    DBInstance.SaveChanges();

                    System.Console.WriteLine($"Fresh import of track [{pname}]");

                    // Continue with next file
                    continue;
                }

                if ((OMP3Import.date_file_mod == ofile.date_file_mod)
                    //&& (OMP3Import.file_hash == file_hash)
                    )
                {
                    System.Console.WriteLine($"Skip unchanged track [{pname}]");

                    // No changes since last import, data already present
                    continue;
                }

                // Update file meta data
                OMP3Import.date_file_mod = ofile.date_file_mod;
                //OMP3Import.file_hash = file_hash;

                // Update ID3 meta data in record
                setMetaData(OMP3Import, tagID3);

                // Save to database
                DBInstance.SaveChanges();

                System.Console.WriteLine($"Update of track [{pname}]");
            }
        }
    }
}
