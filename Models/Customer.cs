using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace EcomApp.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; private set; }
        public string Name { get; set; }
        public string  Email { get; set; }
        public ICollection<Order> Orders { get; set; }
        public Customer()
        {
            Orders = new List<Order>();
        }
    }
}
