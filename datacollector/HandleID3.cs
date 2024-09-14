using ATL;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

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
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
            return string.Empty;
        }

        public void Process(string pathprefix)
        {
            foreach (MP3File ofile in filenamesMP3)
            {
                // Strip prefix
                string pname = ofile.filename;
                pname = pname.Replace(pathprefix, "");

                // Memorize database read
                bool record_read = true;
                string file_hash = CalculateMD5(ofile);

                // Handle MP3 import
                var OMP3Import = DBInstance.MP3Import.Where(a => a.filename == pname).FirstOrDefault();
                if (null == OMP3Import)
                {
                    record_read = false;
                    OMP3Import = new MP3Import();
                    OMP3Import.filename = pname;
                    OMP3Import.file_hash = file_hash;
                    OMP3Import.date_file_mod = ofile.date_file_mod;
                    DBInstance.Add(OMP3Import);
                    DBInstance.SaveChanges();
                }

                if ((true == record_read)
                    && (OMP3Import.date_file_mod == ofile.date_file_mod)
                    && (OMP3Import.file_hash == file_hash))
                {
                    // No changes since last import, data already present
                    continue;
                }

                // Read ID3 tag
                var tagID3 = TagLib.File.Create(ofile.filename);

                var theTrack = new Track(ofile.filename);

                // Handle of album
                var album = tagID3.Tag.Album;
                var Oalbum = DBInstance.Album.Where(a => a.name == album).FirstOrDefault();
                if (null == Oalbum)
                {
                    Oalbum = new Album();
                    Oalbum.name = album;
                    DBInstance.Add(Oalbum);
                    DBInstance.SaveChanges();
                }

                // Handle of artist
                var artist = tagID3.Tag.FirstPerformer;
                var Oartist = DBInstance.Artist.Where(a => a.name == artist).FirstOrDefault();
                if (null == Oartist)
                {
                    Oartist = new Artist();
                    Oartist.name = artist;
                    DBInstance.Add(Oartist);
                    DBInstance.SaveChanges();
                }

                // Handle of genres
                var genre = tagID3.Tag.FirstGenre;
                var Ogenre = DBInstance.Genre.Where(a => a.name == genre).FirstOrDefault();
                if (null == Ogenre)
                {
                    Ogenre = new Genre();
                    Ogenre.name = genre;
                    DBInstance.Add(Ogenre);
                    DBInstance.SaveChanges();
                }

                // Handle of CdHead
                var Ocdhead = DBInstance.CdHead.Where(c => c.album == Oalbum)
                                                .Where(c => c.artist == Oartist)
                                                // .Where(c => c.genre == Ogenre)
                                                .FirstOrDefault();
                if (null == Ocdhead)
                {
                    Ocdhead = new CdHead();
                    Ocdhead.artist = Oartist;
                    Ocdhead.album = Oalbum;
                    Ocdhead.genre = Ogenre;
                    Ocdhead.disc_number_total = (ushort)tagID3.Tag.DiscCount;
                    Ocdhead.play_time_total = (null == tagID3.Tag.Length) ? 0 : UInt32.Parse(tagID3.Tag.Length);
                    Ocdhead.year_published = (ushort)tagID3.Tag.Year;
                    DBInstance.Add(Ocdhead);
                }
                else
                {
                    Ocdhead.year_published = Math.Max(Ocdhead.year_published, (ushort)tagID3.Tag.Year);
                    Ocdhead.play_time_total += (null == tagID3.Tag.Length) ? 0 : UInt32.Parse(tagID3.Tag.Length);
                    DBInstance.Update(Ocdhead);
                }
                DBInstance.SaveChanges();

                /*
                                // Handle of titles
                                var title = tagID3.Tag.Title;
                                var Otitle= DBInstance.Title.Where(a => a.name == title).FirstOrDefault();
                                if (null == Otitle)
                                {
                                    Otitle = new Title { name = title };
                                    DBInstance.Add(Otitle);
                                    DBInstance.SaveChanges();
                                }

                                // Handle of CD titles
                                var Ocdheader = DBInstance.CdHeader.Where(a => a.album == Oalbum).FirstOrDefault();
                                if (null == Ocdheader)
                                {
                                    Ocdheader = new CdHeader();
                                    Ocdheader.album = Oalbum;
                                    DBInstance.Add(Ocdheader);
                                    DBInstance.SaveChanges();
                                }
                */
            }
        }
    }
}
