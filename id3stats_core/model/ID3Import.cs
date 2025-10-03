using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace net.derpaul.id3stats.model
{
    /// <summary>
    /// ID3Import entity
    /// </summary>
    public class ID3Import
    {
        /// <summary>
        /// ID as primary key
        /// </summary>
        public ulong id { get; set; }

        /// <summary>
        /// Filename
        /// </summary>
        [StringLength(512)]
        public string filename { get; set; }

        /// <summary>
        /// Date import
        /// </summary>
        public DateTime date_import { get; set; }

        /// <summary>
        /// Date of last file modification
        /// </summary>
        public DateTime date_file_mod { get; set; }

        ///// <summary>
        ///// File hash
        ///// </summary>
        [StringLength(64)]
        public string filehash { get; set; }

        /// <summary>
        /// Track artist
        /// </summary>
        [StringLength(256)]
        public string artist { get; set; }

        /// <summary>
        /// Track album
        /// </summary>
        [StringLength(256)]
        public string album { get; set; }

        /// <summary>
        /// Track title
        /// </summary>
        [StringLength(256)]
        public string title { get; set; }

        /// <summary>
        /// Track genre
        /// </summary>
        [StringLength(256)]
        public string genre { get; set; }

        /// <summary>
        /// Track duration in ms
        /// </summary>
        public double durationms { get; set; }

        /// <summary>
        /// Track number
        /// </summary>
        public int tracknumber { get; set; }

        /// <summary>
        /// Tracks total
        /// </summary>
        public int tracktotal { get; set; }

        /// <summary>
        /// Track year
        /// </summary>
        public int year { get; set; }

        /// <summary>
        /// Disc of track
        /// </summary>
        public int discnumber { get; set; }

        /// <summary>
        /// Total discs
        /// </summary>
        public int disctotal { get; set; }

        /// <summary>
        /// Bitrate of track
        /// </summary>
        public int bitrate { get; set; }

        /// <summary>
        /// Samplerate of track
        /// </summary>
        public double samplerate { get; set; }
    }
}
