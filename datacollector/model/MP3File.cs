using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace net.derpaul.cdstats
{
    /// <summary>
    /// MP3File entity
    /// </summary>
    public class MP3File
    {
        /// <summary>
        /// ID as primary key
        /// </summary>
        public ulong id { get; set; }

        /// <summary>
        /// File name
        /// </summary>
        [StringLength(512)]
        public string name { get; set; }

        /// <summary>
        /// Foreign key constraint - one MP3File, many MP3Imports
        /// </summary>
        public ICollection<MP3Import> mp3imports { get; set; }
    }
}
