using System;
using LanguageExt;
using static LanguageExt.Prelude;
using System.Text.RegularExpressions;


namespace ShoppingApp.Domain.Models
{
    public record ProductCode
    {
        public const string ValidPattern = "^[0-9]*$";
        private static readonly Regex PatternRegex = new(ValidPattern);

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

        private static bool IsValid(string stringValue) => PatternRegex.IsMatch(stringValue);

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
