using LanguageExt;
using static LanguageExt.Prelude;
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

        public static Option<AdresaLivrare> TryParse(string stringValue)
        {
            if (IsValid(stringValue))
            {
                return Some<AdresaLivrare>(new(stringValue));
            }
            else
            {
                return None;
            }
        }
    }
}
