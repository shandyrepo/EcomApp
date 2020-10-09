using EcomApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EcomApp.Data
{
    /// <summary>
    /// Контекст данных для работы с БД
    /// </summary>
    public class DataContext : DbContext
    {
        public static bool iscreated = false;
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<LineItem> LineItems { get; set; }

    }
}
