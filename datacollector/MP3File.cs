using System.Security.Cryptography;

namespace net.derpaul.cdstats
{
    /// <summary>
    /// Find MP3 files, import ID3 tag to database, run plugins
    /// </summary>
    internal class MP3File
    {
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
    }
}
