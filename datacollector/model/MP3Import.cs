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
        /// Filename
        /// </summary>
        [StringLength(512)]
        public string filename { get; set; }

        /// <summary>
        /// Date import
        /// https://stackoverflow.com/questions/21219797/how-to-get-correct-timestamp-in-c-sharp
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime date_import { get; set; }

        /// <summary>
        /// Date of last file modification
        /// </summary>
        public DateTime date_file_mod { get; set; }

        /// <summary>
        /// File hash
        /// </summary>
        [StringLength(64)]
        public string file_hash { get; set; }
    }
}
