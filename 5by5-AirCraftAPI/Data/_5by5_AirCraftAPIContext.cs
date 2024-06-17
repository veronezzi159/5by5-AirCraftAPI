using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using _5by5_AirCraftAPI.Models;

namespace _5by5_AirCraftAPI.Data
{
    public class _5by5_AirCraftAPIContext : DbContext
    {
        public _5by5_AirCraftAPIContext (DbContextOptions<_5by5_AirCraftAPIContext> options)
            : base(options)
        {
        }

        public DbSet<_5by5_AirCraftAPI.Models.AirCraft> AirCraft { get; set; } = default!;
        public DbSet<_5by5_AirCraftAPI.Models.Removed> Removed { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AirCraft>().ToTable("AirCraft");
            modelBuilder.Entity<Removed>().ToTable("Removed");
        }
    }
}
