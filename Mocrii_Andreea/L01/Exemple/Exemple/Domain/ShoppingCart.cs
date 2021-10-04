using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain
{
    [AsChoice]
    public static partial class ShoppingCart
    {
        public interface IShoppingCart { }

        public record EmptyCart(IReadOnlyCollection<UnvalidatedCart> ProductsList) : IShoppingCart;

        public record InvalidatedCart(IReadOnlyCollection<UnvalidatedCart> ProductsList, string reason) : IShoppingCart;

        public record ValidatedCartProducts(IReadOnlyCollection<ValidatedCart> ProductsList) : IShoppingCart;

        public record PayedCart(IReadOnlyCollection<ValidatedCart> ProductsList, DateTime PublishedDate) : IShoppingCart;
    }
}
