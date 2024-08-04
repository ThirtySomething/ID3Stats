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
    public class MModel : DbContext
    {
        public DbSet<MArtist> DBArtist { get; set; }

        public MModel(DbContextOptions<MModel> options) : base(options)
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
            // Entity for measurement types
            modelBuilder.Entity<MArtist>().ToTable("artist");
            modelBuilder.Entity<MArtist>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.HasIndex(e => e.artist).IsUnique();
            });

            // Create the model
            base.OnModelCreating(modelBuilder);
        }
    }
}
