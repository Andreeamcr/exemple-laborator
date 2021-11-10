using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    public record PretProdus
    {
        public decimal Value { get; }

        public PretProdus(decimal value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidProdusException($"{value:0.##} este un pret invalid");
            }
        }

        public static PretProdus operator *(PretProdus a,  decimal b) => new PretProdus(a.Value * b);

        public override string ToString()
        {
            return $"{Value:0.##}";
        }

        public static bool TryParse(string pretString, out PretProdus produs)
        {
            bool isValid = false;
            produs = null;
            if (decimal.TryParse(pretString, out decimal numericPret))
            {
                if (IsValid(numericPret))
                {
                    isValid = true;
                    produs = new(numericPret);
                }
            }

            return isValid;
        }

        private static bool IsValid(decimal numericPret) => numericPret >= 0;
    }
}

