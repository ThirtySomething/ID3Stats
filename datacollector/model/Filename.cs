using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace net.derpaul.cdstats
{
    /// <summary>
    /// Entity for artists
    /// </summary>
    public class Filename
    {
        /// <summary>
        /// ID as primary key
        /// </summary>
        public ulong id { get; set; }

        /// <summary>
        /// Filename
        /// </summary>
        [StringLength(512)]
        public string name { get; set; }

        /// <summary>
        /// Timestamp of import
        /// </summary>
        public DateTime import { get; set; }
    }
}
