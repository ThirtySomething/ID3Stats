using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void Process()
        {
            foreach (string filename in filenamesMP3)
            {
                // Read ID3 tag
                var tagID3 = TagLib.File.Create(filename);

                // Handle of album
                var album = tagID3.Tag.Album;
                var Oalbum = DBInstance.Album.Where(a => a.name == album).FirstOrDefault();
                if (Oalbum == null)
                {
                    Oalbum = new Album { name = album };
                    DBInstance.Add(Oalbum);
                    DBInstance.SaveChanges();
                }

                // Handle of artist
                var artist = tagID3.Tag.FirstPerformer;
                var Oartist = DBInstance.Artist.Where(a => a.name == artist).FirstOrDefault();
                if (Oartist == null)
                {
                    Oartist = new Artist { name = artist };
                    DBInstance.Add(Oartist);
                    DBInstance.SaveChanges();
                }
                break;
            }
        }
    }
}
