using EcomApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EcomApp.Data
{
    public class DataContext : DbContext
    {
        public static bool iscreated = false;
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            if (!iscreated)
            {

               // Database.EnsureDeleted();
                Database.EnsureCreated();
                iscreated = true;
            }
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<LineItem> LineItems { get; set; }

    }
}
