using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain
{
    public record Cart
    {
        public decimal Value { get; }

        public Cart(decimal value)
        {
            if (value > 0 && value <= 100)
            {
                Value = value;
            }
            else
            {
                throw new InvalidCartException($"{value:0.##} is an invalid quantity value.");
            }
        }

        public Cart Round()
        {
            var roundedValue = Math.Round(Value);
            return new Cart(roundedValue);
        }

        public override string ToString()
        {
            return $"{Value:0.##}";
        }
    }
}
