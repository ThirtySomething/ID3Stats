using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace net.derpaul.cdstats
{
    /// <summary>
    /// Entity for artists
    /// </summary>
    public class MArtist
    {
        /// <summary>
        /// ID as primary key
        /// </summary>
        public ulong id { get; set; }

        /// <summary>
        /// Measurement location
        /// </summary>
        public string artist { get; set; }
    }
}
