using Microsoft.EntityFrameworkCore;
using PD.DAL.Entitites.AppEntitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.DAL
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<Plate> Plates { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Plate>()
                .HasMany(e => e.Entries)
                .WithOne(p => p.Plate)
                .HasForeignKey(e => e.PlateId)
                .OnDelete(DeleteBehavior.Cascade);

            
        }
    }
}
