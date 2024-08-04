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
    public class CdStats : DbContext
    {
        public DbSet<Album> Album { get; set; }
        public DbSet<Artist> Artist { get; set; }

        public CdStats(DbContextOptions<CdStats> options) : base(options)
        {
        }

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

            // Create the model
            base.OnModelCreating(modelBuilder);
        }
    }
}
