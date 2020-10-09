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
        public int Id { get; private set; }

        [Range(0,int.MaxValue, ErrorMessage = "Количество не может быть отрицательным")]
        public int Quantity { get; set; }
        public int ProductId  { get; set; }
        public Product Product { get; set; }
    }
}
