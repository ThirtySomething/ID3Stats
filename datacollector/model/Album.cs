using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace net.derpaul.cdstats
{
    /// <summary>
    /// Album entity
    /// </summary>
    public class Album
    {
        /// <summary>
        /// ID as primary key
        /// </summary>
        public ulong id { get; set; }

        /// <summary>
        /// Album name
        /// </summary>
        [StringLength(512)]
        public string name { get; set; }
    }
}
