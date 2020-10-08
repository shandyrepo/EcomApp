using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace EcomApp.Contracts.Request
{
    public class OrderCreateRequest
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }

 
    
   
}
