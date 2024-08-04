using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

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
        /// Entity for filenames
        /// </summary>
        public DbSet<Filename> Filename { get; set; }

        /// <summary>
        /// Entity for genres
        /// </summary>
        public DbSet<Genre> Genre { get; set; }

        /// <summary>
        /// Entity for titles
        /// </summary>
        public DbSet<Title> Title { get; set; }

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

            // Entity for filenames
            modelBuilder.Entity<Filename>().ToTable("filename");
            modelBuilder.Entity<Filename>(entity =>
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

            // Create the model
            base.OnModelCreating(modelBuilder);
        }
    }
}
