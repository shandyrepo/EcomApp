using System;
using System.ComponentModel.DataAnnotations;

namespace EcomApp.Contracts.Request
{
    public class OrderCreateRequest
    {
        public int Id { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Количество не может быть отрицательным")]
        public int Quantity { get; set; }
    }




}
