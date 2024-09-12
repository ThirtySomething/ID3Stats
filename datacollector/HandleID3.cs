using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace net.derpaul.cdstats
{
    internal class HandleID3
    {
        private List<string> filenamesMP3;
        private CdStats DBInstance;

        public HandleID3(List<string> filenamesMP3)
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

        string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public void Process(string pathprefix)
        {
            foreach (string filename in filenamesMP3)
            {
                // Strip prefix
                string pname = filename;
                pname = pname.Replace(pathprefix, "");

                // Handle MP3 import
                var OMP3Import = DBInstance.MP3Import.Where(a => a.filename == pname).FirstOrDefault();
                if (null == OMP3Import)
                {
                    OMP3Import = new MP3Import();
                    OMP3Import.filename = pname;
                    OMP3Import.file_hash = CalculateMD5(filename);
                    DBInstance.Add(OMP3Import);
                    DBInstance.SaveChanges();
                }

                /*
                                // Read ID3 tag
                                var tagID3 = TagLib.File.Create(filename);

                                // Handle of album
                                var album = tagID3.Tag.Album;
                                var Oalbum = DBInstance.Album.Where(a => a.name == album).FirstOrDefault();
                                if (null == Oalbum)
                                {
                                    Oalbum = new Album { name = album };
                                    DBInstance.Add(Oalbum);
                                    DBInstance.SaveChanges();
                                }

                                // Handle of artist
                                var artist = tagID3.Tag.FirstPerformer;
                                var Oartist = DBInstance.Artist.Where(a => a.name == artist).FirstOrDefault();
                                if (null == Oartist)
                                {
                                    Oartist = new Artist { name = artist };
                                    DBInstance.Add(Oartist);
                                    DBInstance.SaveChanges();
                                }

                                // Handle of genres
                                var genre = tagID3.Tag.FirstGenre;
                                var Ogenre = DBInstance.Genre.Where(a => a.name == genre).FirstOrDefault();
                                if (null == Ogenre)
                                {
                                    Ogenre = new Genre { name = genre };
                                    DBInstance.Add(Ogenre);
                                    DBInstance.SaveChanges();
                                }

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
