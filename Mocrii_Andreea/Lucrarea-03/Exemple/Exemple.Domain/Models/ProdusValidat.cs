using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    public record ProdusValidat(Produs cantitate, PretProdus pret, CodProdus codProdus, AdresaLivrare adresaLivrare);
}
