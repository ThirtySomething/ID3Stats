using System.Security.Cryptography;

namespace net.derpaul.id3stats
{
    /// <summary>
    /// Meta data of physical MP3 file
    /// </summary>
    internal class MP3File
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="filename"></param>
        public MP3File(string filename)
        {
            this.filename = filename;
            this.date_file_mod = File.GetLastWriteTime(this.filename);
        }

        /// <summary>
        /// Absolute path and filename of MP3 file
        /// </summary>
        public string filename { get; }

        /// <summary>
        /// Last modification date of file
        /// </summary>
        public DateTime date_file_mod { get; }

        /// <summary>
        /// MD5 hash of MP3 file
        /// </summary>
        public string filehash { get; set; } = "";

        /// <summary>
        /// Calculation of MD5 checksum as fingerprint
        /// </summary>
        /// <param name="ofile"></param>
        /// <returns></returns>
        public string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToUpperInvariant();
                }
            }
        }
    }
}
