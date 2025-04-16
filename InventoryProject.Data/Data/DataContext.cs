using InventoryProject.Core.Model;
using InventoryProject.Data.Seed;
using Microsoft.EntityFrameworkCore;

namespace InventoryProject.Data.Data
{
    public sealed class DataContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<Variation> Variations { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SeedData.Load(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
    }
}
