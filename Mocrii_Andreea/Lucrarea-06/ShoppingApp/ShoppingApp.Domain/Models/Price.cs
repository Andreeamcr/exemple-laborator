using System;
using LanguageExt;
using static LanguageExt.Prelude;

namespace ShoppingApp.Domain.Models
{
    public record Price
    {
        public decimal Value { get; }

        public Price(decimal value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new Exception($"{value:0.##} este un pret invalid");
            }
        }

        public static Price operator *(Price a, decimal b) => new Price(a.Value * b);

        public override string ToString()
        {
            return $"{Value:0.##}";
        }

        public static Option<Price> TryParse(string stringValue)
        {
            if (decimal.TryParse(stringValue, out decimal price) && IsValid(price))
            {
                return Some<Price>(new(price));
            }
            else
            {
                return None;
            }
        }

        private static bool IsValid(decimal numericPret) => numericPret >= 0;
    }
}
