using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace net.derpaul.cdstats
{
    /// <summary>
    /// CdTrack entity
    /// </summary>
    public class CdTrack
    {
        /// <summary>
        /// ID as primary key
        /// </summary>
        public ulong id { get; set; }

        /// <summary>
        /// Track is on disc x
        /// </summary>
        public ushort disc_number { get; set; }

        /// <summary>
        /// Playtime of track
        /// </summary>
        public ushort play_time { get; set; }

        /// <summary>
        /// Number of track
        /// </summary>
        public ushort track_number { get; set; }

        /// <summary>
        /// Track is published in year
        /// </summary>
        public ushort year_published { get; set; }
    }
}
