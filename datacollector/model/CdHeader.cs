using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace net.derpaul.cdstats
{
    /// <summary>
    /// Entity for genres
    /// </summary>
    public class CdHeader
    {
        /// <summary>
        /// ID as primary key
        /// </summary>
        public ulong id { get; set; }

        public Album album { get; set; }
    }
}
