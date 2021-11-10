using LanguageExt;
using static LanguageExt.Prelude;
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

        public static Option<PretProdus> TryParse(string stringValue)
        {
            if (decimal.TryParse(stringValue, out decimal numericPret) && IsValid(numericPret))
            {
                return Some<PretProdus>(new(numericPret));
            }
            else
            {
                return None;
            }
        }

        private static bool IsValid(decimal numericPret) => numericPret >= 0;
    }
}

