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
        private MModel DBInstance;

        public HandleID3(List<string> filenamesMP3)
        {
            this.filenamesMP3 = filenamesMP3;
        }

        public bool Init()
        {
            bool ret = true;

            DBInstance = new MModel(new DbContextOptions<MModel>());
            DBInstance.Database.EnsureCreated();

            return ret;
        }

        public void Process()
        {
            foreach (string filename in filenamesMP3)
            {
                var tagID3 = TagLib.File.Create(filename);
                var artist = tagID3.Tag.FirstArtist;
                var Oartist = DBInstance.DBArtist.Where(a => a.artist == artist).FirstOrDefault();
                if (Oartist == null)
                {
                    Oartist = new MArtist { artist = artist };
                    DBInstance.Add(Oartist);
                    DBInstance.SaveChanges();
                }
            }
        }
    }
}
