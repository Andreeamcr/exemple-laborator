using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Domain.Models
{
    public record CalculatedProduct(ProductCode Code, Price Price, Quantity Quantity, DestinationAddress DestinationAddress, Price TotalPrice)
    {
        public int OrderId { get; set; }
        public bool IsUpdated { get; set; }
    }
}
