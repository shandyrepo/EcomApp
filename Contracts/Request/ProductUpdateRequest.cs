using System;
using System.ComponentModel.DataAnnotations;

namespace EcomApp.Contracts.Request
{
    public class ProductUpdateRequest
    {
        public int ID { get; set; }
        public string Name { get; set; }

        [Range(0.0, double.MaxValue, ErrorMessage = "Цена не может быть отрицательным")]
        public decimal Price { get; set; }

    }
}
