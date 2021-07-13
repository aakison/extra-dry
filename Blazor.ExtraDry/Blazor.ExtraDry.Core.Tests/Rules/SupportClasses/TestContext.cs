using Microsoft.EntityFrameworkCore;

namespace Blazor.ExtraDry.Core.Tests.Rules {
    public class TestContext : DbContext {

        public TestContext(DbContextOptions<TestContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
