using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Domain.Models
{
    public record UnvalidatedProduct(string ProductCode, string Quantity, string Price, string DestinationAddress);
}
