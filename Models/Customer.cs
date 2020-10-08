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
        public int id { get; private set; }
        public string name { get; set; }
        public string  email { get; set; }
        public ICollection<Order> Orders { get; set; }
        public Customer()
        {
            Orders = new List<Order>();
        }
    }
}
