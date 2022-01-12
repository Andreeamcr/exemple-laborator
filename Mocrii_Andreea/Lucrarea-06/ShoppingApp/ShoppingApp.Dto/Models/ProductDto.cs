using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Dto.Models
{
    public record ProductDto
    {
        public string Code { get; init; }
        public decimal Quantity { get; init; }
        public decimal Price { get; init; }
        public string Address { get; init; }
        public decimal TotalPrice { get; init; }
    }
}
