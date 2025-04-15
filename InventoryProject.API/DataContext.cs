﻿using InventoryProject.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace InventoryProject.API
{
    public sealed class DataContext : DbContext
    {
        public DbSet<Item> Items { get; set; }

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
