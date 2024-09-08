using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;

namespace net.derpaul.cdstats
{
    /// <summary>
    /// ERM model for CD statistics
    /// </summary>
    public class CdStats : DbContext
    {
        /// <summary>
        /// Entity for albums
        /// </summary>
        public DbSet<Album> Album { get; set; }

        /// <summary>
        /// Entity for artists
        /// </summary>
        public DbSet<Artist> Artist { get; set; }

        /// <summary>
        /// Entity for genres
        /// </summary>
        public DbSet<Genre> Genre { get; set; }

        /// <summary>
        /// Entity for titles
        /// </summary>
        public DbSet<Title> Title { get; set; }

        /// <summary>
        /// Entity for CD heads
        /// </summary>
        public DbSet<CdHead> CdHead { get; set; }

        /// <summary>
        /// Entity for CD tracks
        /// </summary>
        public DbSet<CdTrack> CdTrack { get; set; }

        /// <summary>
        /// Entity for filenames
        /// </summary>
        public DbSet<MP3File> MP3File { get; set; }

        /// <summary>
        /// Entity for path names
        /// </summary>
        public DbSet<MP3Path> MP3Path { get; set; }

        /// <summary>
        /// Entity for MP3 imports
        /// </summary>
        public DbSet<MP3Import> MP3Import { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="options"></param>
        public CdStats(DbContextOptions<CdStats> options) : base(options)
        {
        }

        /// <summary>
        /// Initialize connection string and driver version
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = $"server={DataCollectorConfig.Instance.DBServer};port={DataCollectorConfig.Instance.DBPort};database={DataCollectorConfig.Instance.DBDatabase};user={DataCollectorConfig.Instance.DBUserId};password={DataCollectorConfig.Instance.DBPassword}";
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));
            optionsBuilder.UseMySql(connectionString, serverVersion, options =>
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
            // Entity for albums
            modelBuilder.Entity<Album>().ToTable("album");
            modelBuilder.Entity<Album>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.HasIndex(e => e.name).IsUnique();
            });

            // Entity for artists
            modelBuilder.Entity<Artist>().ToTable("artist");
            modelBuilder.Entity<Artist>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.HasIndex(e => e.name).IsUnique();
            });

            // Entity for genres
            modelBuilder.Entity<Genre>().ToTable("genre");
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.HasIndex(e => e.name).IsUnique();
            });

            // Entity for titles
            modelBuilder.Entity<Title>().ToTable("title");
            modelBuilder.Entity<Title>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.HasIndex(e => e.name).IsUnique();
            });

            // Entity for CD heads
            modelBuilder.Entity<CdHead>().ToTable("cdhead");
            modelBuilder.Entity<CdHead>(entity =>
            {
                entity.HasKey(e => e.id);
            });

            // Entity for CD tracks
            modelBuilder.Entity<CdTrack>().ToTable("cdtrack");
            modelBuilder.Entity<CdTrack>(entity =>
            {
                entity.HasKey(e => e.id);
            });

            // Entity for MP3 filenames
            modelBuilder.Entity<MP3File>().ToTable("mp3file");
            modelBuilder.Entity<MP3File>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.HasIndex(e => e.name).IsUnique();
            });

            // Entity for MP3 paths
            modelBuilder.Entity<MP3Path>().ToTable("mp3path");
            modelBuilder.Entity<MP3Path>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.HasIndex(e => e.name).IsUnique();
            });

            // Entity for MP3 import
            modelBuilder.Entity<MP3Import>().ToTable("mp3import");
            modelBuilder.Entity<MP3Import>(entity =>
            {
                entity.HasKey(e => e.id);
            });

            // Foreign key from mp3file to mp3import
            modelBuilder.Entity<MP3Import>()
                    .HasOne(e => e.mp3file)
                    .WithMany(c => c.mp3imports)
                    .HasForeignKey(e => e.id_mp3file_ref);

            // Foreign key from mp3path to mp3import
            modelBuilder.Entity<MP3Import>()
                    .HasOne(e => e.mp3path)
                    .WithMany(c => c.mp3imports)
                    .HasForeignKey(e => e.id_mp3path_ref);

            // Create the model
            base.OnModelCreating(modelBuilder);
        }
    }
}
