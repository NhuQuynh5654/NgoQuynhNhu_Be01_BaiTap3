using BaitapBackend3.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BaitapBackend3.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Intern> Interns { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<AllowAccess> AllowAccesses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AllowAccess>()
                .HasOne(a => a.Role)
                .WithMany(r => r.AllowAccesses)
                .HasForeignKey(a => a.RoleId);
        }
    }
}
