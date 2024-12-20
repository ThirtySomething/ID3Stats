using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace net.derpaul.mp3stats.model
{
    /// <summary>
    /// ERM model for CD statistics
    /// </summary>
    public class MP3Stats : DbContext
    {
        /// <summary>
        /// Entity for MP3 imports
        /// </summary>
        public DbSet<MP3Import> MP3Import { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="options"></param>
        public MP3Stats(DbContextOptions<MP3Stats> options) : base(options)
        {
        }

        /// <summary>
        /// Initialize connection string and driver version for use with MariaDB
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DBConfig.Instance.DBFilename}", options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });

            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// ERM as code
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Entity for MP3 import
            modelBuilder.Entity<MP3Import>().ToTable("mp3import");
            modelBuilder.Entity<MP3Import>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.HasIndex(e => e.filename).IsUnique();
            });

            modelBuilder.Entity<MP3Import>()
                .HasIndex(e => new { e.album })
                .HasDatabaseName("ix_album");

            modelBuilder.Entity<MP3Import>()
                .HasIndex(e => new { e.artist })
                .HasDatabaseName("ix_artist");

            modelBuilder.Entity<MP3Import>()
                .HasIndex(e => new { e.genre })
                .HasDatabaseName("ix_genre");

            modelBuilder.Entity<MP3Import>()
                .HasIndex(e => new { e.filehash })
                .HasDatabaseName("ix_filehash");

            modelBuilder.Entity<MP3Import>()
                .HasIndex(e => new { e.filename })
                .HasDatabaseName("ix_filename");

            modelBuilder.Entity<MP3Import>()
                .HasIndex(e => new { e.title })
                .HasDatabaseName("ix_title");

            // Create the model
            base.OnModelCreating(modelBuilder);
        }
    }
}
