using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EcomApp.Models
{
    public class Order
    {
        [Key]
        public int id { get; private set; }
        public DateTime creationDate { get; set; }
        public ICollection<LineItem> LineItems { get; set; }
        public Order()
        {
            LineItems = new List<LineItem>();
        }
    }
}
