using System;
using LanguageExt;
using static LanguageExt.Prelude;

namespace ShoppingApp.Domain.Models
{
    public record DestinationAddress
    {
        public string Value { get; }

        public DestinationAddress(string value)
        {
            if(IsValid(value))
            {
                this.Value = value;
            }
            else
            {
                throw new Exception();
            }
        }

        private static bool IsValid(string stringValue) => stringValue.Length > 0 && stringValue.Length <= 25;

        public override string ToString()
        {
            return Value;
        }

        public static Option<DestinationAddress> TryParse(string stringValue)
        {
            if (IsValid(stringValue))
            {
                return Some<DestinationAddress>(new(stringValue));
            }
            else
            {
                return None;
            }
        }

    }
}
