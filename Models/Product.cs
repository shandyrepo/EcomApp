using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EcomApp.Models
{
    public class Product
    {
        [Key]
        public int id { get; private set; }
        public string productName { get; set; }
        public decimal price { get; set; }
    }
}
