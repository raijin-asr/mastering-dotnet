using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductManagementApi.Models;

namespace ProductManagementApi.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } // represents the collection of products EF Core can query, insert, update, and delete in the database
        

        //Configure Unique Index to prevent duplicate values at same time inserted using EF Core
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //need to call the base implementation so Identity can configure its tables, relationships, indexes, and constraints.
            base.OnModelCreating(modelBuilder);  //Identity has its own database configuration.

            modelBuilder.Entity<Product>()
                .Property(p => p.ProductName)
                .UseCollation("NOCASE");  //makes SQLite compare product names without distinguishing ASCII letter case.

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.ProductName) //tells EF Core to create a unique database index on ProductName
                .IsUnique();


        }
    }
}