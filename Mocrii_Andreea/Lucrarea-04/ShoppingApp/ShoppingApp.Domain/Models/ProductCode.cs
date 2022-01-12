using System;
using LanguageExt;
using static LanguageExt.Prelude;
using System.Text.RegularExpressions;


namespace ShoppingApp.Domain.Models
{
    public record ProductCode
    {
        private static readonly Regex ValidPattern = new("^[0-9]*$");

        public string Value { get; }

        public ProductCode(string value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new Exception("");
            }
        }

        private static bool IsValid(string stringValue) => ValidPattern.IsMatch(stringValue);

        public override string ToString()
        {
            return Value;
        }

        public static Option<ProductCode> TryParse(string stringValue)
        {
            if (IsValid(stringValue))
            {
                return Some<ProductCode>(new(stringValue));
            }
            else
            {
                return None;
            }
        }
    }
}
