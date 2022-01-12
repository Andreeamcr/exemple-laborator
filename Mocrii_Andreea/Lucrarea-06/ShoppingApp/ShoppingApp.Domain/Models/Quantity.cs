using LanguageExt;
using static LanguageExt.Prelude;
using System;


namespace ShoppingApp.Domain.Models
{
    public record Quantity
    {
        public decimal Value { get; }

        public Quantity(decimal value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new Exception();
            }
        }

        public override string ToString()
        {
            return $"{Value:0.##}";
        }
        public static Option<Quantity> TryParseProdus(string quantityString)
        {
            if (decimal.TryParse(quantityString, out decimal quantity) && IsValid(quantity))
            {
                return Some<Quantity>(new(quantity));
            }
            else
            {
                return None;
            }
        }
        private static bool IsValid(decimal quantity) => quantity >= 0;
    }
}
