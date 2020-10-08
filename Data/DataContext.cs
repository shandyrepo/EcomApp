using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EcomApp.Models;

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
                Database.EnsureDeleted();
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
