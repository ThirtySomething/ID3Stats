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
        /// Reference by album id
        /// </summary>
        public ulong id_album_ref { get; set; }

        /// <summary>
        /// Reference of album object
        /// </summary>
        public Album album { get; set; }

        /// <summary>
        /// Reference by artist id
        /// </summary>
        public ulong id_artist_ref { get; set; }

        /// <summary>
        /// Reference of artist object
        /// </summary>
        public Artist artist { get; set; }

        /// <summary>
        /// Reference genre by id
        /// </summary>
        public ulong id_genre_ref { get; set; }

        /// <summary>
        /// Reference genre as object
        /// </summary>
        public Genre genre { get; set; }

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
