using Microsoft.EntityFrameworkCore;
using PracticeSandBox.Models;

namespace PracticeSandBox.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }

        //this represents Products table of database
        public DbSet<Product> Products { get; set; }
    }
}
