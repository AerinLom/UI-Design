using Microsoft.EntityFrameworkCore;
using ShopSmartAPI.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
using ShopSmartAPI.Models;
namespace ShopSmartAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
