using Microsoft.VisualBasic;
using System.Text.Json.Nodes;
using System.Xml.Serialization;

namespace net.derpaul.cdstats
{
    /// <summary>
    /// Configuration settings of datacollector to read the bricklets
    /// </summary>
    public class DBConfig : ConfigLoader<DBConfig>, IConfigObject
    {
        /// <summary>
        /// To set default values
        /// </summary>
        public void SetDefaults()
        {
            DBDatabase = "cdstats";
            DBPassword = "CJ-SF5EvZ/uH*BaU";
            DBPort = 3306;
            DBServer = "192.168.71.15";
            DBUserId = "cdstats";
        }

        /// <summary>
        /// The database schema used to write to
        /// </summary>
        public string DBDatabase { get; set; }

        /// <summary>
        /// The password for the username to connect
        /// </summary>
        public string DBPassword { get; set; }

        /// <summary>
        /// The port of the database instance
        /// </summary>
        public int DBPort { get; set; }

        /// <summary>
        /// The server name or IP of the database
        /// </summary>
        public string DBServer { get; set; }

        /// <summary>
        /// The username to connect to
        /// </summary>
        public string DBUserId { get; set; }
    }
}