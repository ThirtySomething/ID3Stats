using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace net.derpaul.cdstats
{
    /// <summary>
    /// Artist entity
    /// </summary>
    public class Artist
    {
        /// <summary>
        /// ID as primary key
        /// </summary>
        public ulong id { get; set; }

        /// <summary>
        /// Artist name
        /// </summary>
        [StringLength(512)]
        public string name { get; set; }

        /// <summary>
        /// Foreign key constraint - one Album, many CdHeads
        /// </summary>
        public ICollection<CdHead> cdheads { get; set; }
    }
}
