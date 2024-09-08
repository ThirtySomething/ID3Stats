using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace net.derpaul.cdstats
{
    /// <summary>
    /// CdHead entity
    /// </summary>
    public class CdHead
    {
        /// <summary>
        /// ID as primary key
        /// </summary>
        public ulong id { get; set; }

        /// <summary>
        /// Total number of discs of album
        /// </summary>
        public ushort disc_number_total { get; set; }

        /// <summary>
        /// Total playtime over all discs of album
        /// </summary>
        public ushort play_time_total { get; set; }

        /// <summary>
        /// Year published
        /// </summary>
        public ushort year_published { get; set; }
    }
}
