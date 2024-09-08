using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace net.derpaul.cdstats
{
    /// <summary>
    /// MP3Path entity
    /// </summary>
    public class MP3Path
    {
        /// <summary>
        /// ID as primary key
        /// </summary>
        public ulong id { get; set; }

        /// <summary>
        /// Path name
        /// </summary>
        [StringLength(512)]
        public string name { get; set; }

        /// <summary>
        /// Foreign key constraint - one MP3Path, many MP3Imports
        /// </summary>
        public ICollection<MP3Import> mp3imports { get; set; }

    }
}
