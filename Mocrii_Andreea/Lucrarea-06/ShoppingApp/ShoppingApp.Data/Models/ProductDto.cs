using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Data.Models
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Code { get; set; }
        public int Quantity { get; set; }
    }
}
