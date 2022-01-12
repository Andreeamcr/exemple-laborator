using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Data.Models
{
    public class OrderLineDto
    {
        public int OrderId { get; set; }
        public string ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        
    }
}
