using Microsoft.EntityFrameworkCore;
using Sample.Shared;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace Sample.Data {
    public class SampleContext : DbContext {

        public SampleContext(DbContextOptions<SampleContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; } 

        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Company>().Property(e => e.SocialMedia).HasConversion(
                e => JsonSerializer.Serialize(e, null),
                e => JsonSerializer.Deserialize<SocialMedia>(e, null));

            modelBuilder.Entity<Company>().Property(e => e.Videos).HasConversion(
                e => JsonSerializer.Serialize(e, null),
                e => JsonSerializer.Deserialize<Collection<Video>>(e, null));

        }
    }
}
