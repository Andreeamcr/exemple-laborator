using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    public record AdresaLivrare
    {
        public string Value { get; }

        private AdresaLivrare(string value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidAdresaLivrareException("");
            }
        }

        private static bool IsValid(string stringValue) => stringValue.Length > 0 && stringValue.Length <= 25;

        public override string ToString()
        {
            return Value;
        }

        public static bool TryParse(string stringValue, out AdresaLivrare adresaLivrare)
        {
            bool isValid = false;
            adresaLivrare = null;

            if (IsValid(stringValue))
            {
                isValid = true;
                adresaLivrare = new(stringValue);
            }

            return isValid;
        }
    }
}
