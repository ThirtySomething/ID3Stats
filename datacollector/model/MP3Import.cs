using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace net.derpaul.cdstats
{
    /// <summary>
    /// MP3Import entity
    /// </summary>
    public class MP3Import
    {
        /// <summary>
        /// ID as primary key
        /// </summary>
        public ulong id { get; set; }

        /// <summary>
        /// Date import
        /// https://stackoverflow.com/questions/21219797/how-to-get-correct-timestamp-in-c-sharp
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime date_import { get; set; }

        /// <summary>
        /// File hash
        /// </summary>
        [StringLength(64)]
        public string file_hash { get; set; }

        /// <summary>
        /// Reference to MP3 filename by object
        /// </summary>
        public MP3File mp3file { get; set; }

        /// <summary>
        /// Reference to MP3 filename by id
        /// </summary>
        public ulong id_mp3file_ref { get; set; }

        /// <summary>
        /// Reference to MP3 path by object
        /// </summary>
        public MP3Path mp3path { get; set; }

        /// <summary>
        /// Reference to MP3 path by id
        /// </summary>
        public ulong id_mp3path_ref { get; set; }
    }
}
