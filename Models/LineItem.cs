using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EcomApp.Models
{
    public class LineItem
    {
        [Key]
        public int id { get; private set; }
        public int quantity { get; set; }
        public int ProductId  { get; set; }
        public Product Product { get; set; }
    }
}
