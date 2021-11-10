using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    public record CodProdus
    {
        private static readonly Regex ValidPattern = new("^[0-9]*$");

        public string Value { get; }

        private CodProdus(string value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidCodProdusException("");
            }
        }

        private static bool IsValid(string stringValue) => ValidPattern.IsMatch(stringValue);

        public override string ToString()
        {
            return Value;
        }

        public static bool TryParse(string stringValue, out CodProdus codProdus)
        {
            bool isValid = false;
            codProdus = null;

            if (IsValid(stringValue))
            {
                isValid = true;
                codProdus = new(stringValue);
            }

            return isValid;
        }
    }
}
