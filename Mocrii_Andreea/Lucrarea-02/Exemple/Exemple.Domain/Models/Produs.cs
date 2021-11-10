using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    public record Produs
    {
        public decimal Cantitate { get; }

        public Produs(decimal value)
        {
            if (IsValid(value))
            {
                Cantitate = value;
            }
            else
            {
                throw new InvalidProdusException($"{value:0.##} este o cantitate invalida.");
            }
        }

        public override string ToString()
        {
            return $"{Cantitate:0.##}";
        }

        public static bool TryParseProdus(string cantitateString, out Produs produs)
        {
            bool isValid = false;
            produs = null;
            if(decimal.TryParse(cantitateString, out decimal numericCantitate))
            {
                if (IsValid(numericCantitate))
                {
                    isValid = true;
                    produs = new(numericCantitate);
                }
            }

            return isValid;
        }

        private static bool IsValid(decimal numericCantitate) => numericCantitate >= 0;
    }
}
