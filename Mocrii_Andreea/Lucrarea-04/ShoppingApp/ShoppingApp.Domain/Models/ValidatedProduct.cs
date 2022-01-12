using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Domain.Models
{
    public record ValidatedProduct(Quantity quantity, Price price, ProductCode productCode, DestinationAddress destinationAddress);
}
