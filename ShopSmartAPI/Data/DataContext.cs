using Microsoft.EntityFrameworkCore;
using ShopSmartAPI.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
namespace ShopSmartAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserList> Userlists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure many-to-many relationship for UserProduct entity
            modelBuilder.Entity<UserList>()
                .HasKey(up => new { up.UserId, up.Id }); // Composite key

            modelBuilder.Entity<UserList>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserLists) // One user can have many user products
                .HasForeignKey(up => up.UserId); // Foreign key

            modelBuilder.Entity<UserList>()
                .HasOne(up => up.Product)
                .WithMany(p => p.UserLists) // One product can belong to many users
                .HasForeignKey(up => up.Id); // Foreign key

            base.OnModelCreating(modelBuilder); // Call base method for further configurations
        }
    }
}
