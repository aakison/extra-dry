using Blazor.ExtraDry;
using Microsoft.EntityFrameworkCore;
using Sample.Shared;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace Sample.Data {
    public class SampleContext : DbContext {

        public SampleContext(DbContextOptions<SampleContext> options) : base(options) { }

        public DbSet<Service> Services { get; set; }

        public DbSet<Employee> Employees { get; set; } 

        public DbSet<Company> Companies { get; set; }

        public DbSet<Content> Contents { get; set; }

        public DbSet<BlobInfo> Blobs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Company>().Property(e => e.BankingDetails).HasConversion(
                e => JsonSerializer.Serialize(e, null),
                e => JsonSerializer.Deserialize<BankingDetails>(e, null));

            modelBuilder.Entity<Company>().Property(e => e.Videos).HasConversion(
                e => JsonSerializer.Serialize(e, null),
                e => JsonSerializer.Deserialize<Collection<Video>>(e, null));

            modelBuilder.Entity<Content>().Property(e => e.Layout).HasConversion(
                e => JsonSerializer.Serialize(e, null),
                e => JsonSerializer.Deserialize<ContentLayout>(e, null));
        }
    }
}
