using LanguageExt;
using System;
using static LanguageExt.Prelude;

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
        public static Option<Produs> TryParseProdus(string cantitateString)
        {
            if (decimal.TryParse(cantitateString, out decimal numericCantitate) && IsValid(numericCantitate))
            {
                return Some<Produs>(new(numericCantitate));
            }
            else
            {
            return None;
            }
        }
        private static bool IsValid(decimal numericCantitate) => numericCantitate >= 0;
    }
}
