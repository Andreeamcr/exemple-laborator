using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Domain.Models
{
    public record CartCommand
    {
        public CartCommand(IReadOnlyCollection<UnvalidatedProduct> inputProduse)
        {
            InputProduse = inputProduse;
        }

        public IReadOnlyCollection<UnvalidatedProduct> InputProduse { get; }
    }
}
