using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Exemple.Domain.Models.CaruciorProduse;

namespace Exemple.Domain.Models
{
    public record PlataCaruciorCommand
    {
        public PlataCaruciorCommand(IReadOnlyCollection<ProdusNevalidat> inputProduse)
        {
            InputProduse = inputProduse;
        }

        public IReadOnlyCollection<ProdusNevalidat> InputProduse { get; }
    }
}
