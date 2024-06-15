using Microsoft.EntityFrameworkCore;
using ProductService.Models;

namespace ProductService.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        DbSet<Product>? Products { get; set; }
    }
}
