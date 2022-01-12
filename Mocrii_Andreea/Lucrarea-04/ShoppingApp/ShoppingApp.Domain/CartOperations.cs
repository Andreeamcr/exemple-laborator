using ShoppingApp.Domain.Models;
using static LanguageExt.Prelude;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ShoppingApp.Domain.Models.Cart;


namespace ShoppingApp.Domain
{
    public static class CartOperations
    {
        public static Task<ICart> ValidateProducts(Func<ProductCode, Option<ProductCode>> checkProductExists, UnvalidatedCart unvalidatedCart) =>
              unvalidatedCart.ProductList
                             .Select(ValidateProduct(checkProductExists))
                             .Aggregate(CreateEmptyList().ToAsync(), ReduceValidProducts)
                             .MatchAsync(
                                Right: validatedProducts => new ValidatedCart(validatedProducts),
                                LeftAsync: errorMessage => Task.FromResult((ICart)new EmptyCart())
                    );

        private static Func<UnvalidatedProduct, EitherAsync<string, ValidatedProduct>> ValidateProduct(Func<ProductCode, Option<ProductCode>> checkProductExists) =>
           unvalidatedProduct => ValidateProduct(checkProductExists, unvalidatedProduct);

        private static EitherAsync<string, ValidatedProduct> ValidateProduct(Func<ProductCode, Option<ProductCode>> checkProdusExists, UnvalidatedProduct unvalidatedProduct) =>
            from quantity in Quantity.TryParseProdus(unvalidatedProduct.Quantity)
                                 .ToEitherAsync(() => $"Cantitate invalida ({unvalidatedProduct.ProductCode}, {unvalidatedProduct.Quantity})")
            from productPrice in Price.TryParse(unvalidatedProduct.Price)
                                .ToEitherAsync(() => $"Pret invalid ({unvalidatedProduct.ProductCode}, {unvalidatedProduct.Price})")
            from productCode in ProductCode.TryParse(unvalidatedProduct.ProductCode)
                                .ToEitherAsync(() => $"Cod produs invalid ({unvalidatedProduct.ProductCode})")
            from destination in DestinationAddress.TryParse(unvalidatedProduct.DestinationAddress)
                                .ToEitherAsync(() => $"Adresa introdusa este invalida ({unvalidatedProduct.DestinationAddress})")
            from produsExists in checkProdusExists(productCode)
                                .ToEitherAsync($"Product with {productCode.Value} does not exist.")
            select new ValidatedProduct(quantity, productPrice, productCode, destination);


        private static Either<string, List<ValidatedProduct>> CreateEmptyList() =>
           Right(new List<ValidatedProduct>());

        private static EitherAsync<string, List<ValidatedProduct>> ReduceValidProducts(EitherAsync<string, List<ValidatedProduct>> acc, EitherAsync<string, ValidatedProduct> next) =>
         from list in acc
         from nextProdus in next
         select list.AppendValidatedProduct(nextProdus);

        private static List<ValidatedProduct> AppendValidatedProduct(this List<ValidatedProduct> list, ValidatedProduct produsValid)
        {
            list.Add(produsValid);
            return list;
        }

        public static ICart CalculateCart (ICart cart) => cart.Match(
            whenEmptyCart: emptyCart => emptyCart,
            whenFailedCart: failedCart => failedCart,
            whenUnvalidatedCart: unvalidatedCart => unvalidatedCart,
            whenCalculatedCart: calculatedCart => calculatedCart,
            whenPaidCart: paidCart => paidCart,
            whenValidatedCart: CreateCalculatedCart
        );

        private static ICart CreateCalculatedCart(ValidatedCart validatedCart) =>
            new CalculatedCart(validatedCart.ProductList
                                                      .Select(CalculateProduct)
                                                      .ToList()
                                                      .AsReadOnly());

        private static CalculatedProduct CalculateProduct(ValidatedProduct validProduct) =>
            new CalculatedProduct(validProduct.productCode,
                                  validProduct.price,
                                  validProduct.quantity,
                                  validProduct.destinationAddress,
                                  validProduct.price * validProduct.quantity.Value);

        public static ICart MergeProducts(ICart products, IEnumerable<CalculatedProduct> existingProducts) => products.Match(
            whenEmptyCart: emptyCart => emptyCart,
            whenFailedCart: failedCart => failedCart,
            whenUnvalidatedCart: unvalidatedCart => unvalidatedCart,
            whenPaidCart: paidCart => paidCart,
            whenValidatedCart: unvalidatedCart => unvalidatedCart,
            whenCalculatedCart: calculatedCart => MergeProducts(calculatedCart.ProductList, existingProducts)
            );

        private static ICart MergeProducts(IReadOnlyCollection<CalculatedProduct> productList, IEnumerable<CalculatedProduct> existingProducts)
        {
            var updatedAndNewProducts = productList.Select(product => product with { OrderId = existingProducts.FirstOrDefault(p => p.Code == product.Code)?.OrderId ?? 0, IsUpdated = true });
            var oldProducts = existingProducts.Where(product => !productList.Any(p => p.Code == product.Code));
            var allProducts = updatedAndNewProducts.Union(oldProducts)
                                                   .ToList()
                                                   .AsReadOnly();
            return new CalculatedCart(allProducts);
        }

        public static ICart PayCart(ICart cart) => cart.Match(
            whenEmptyCart: emptyCart => emptyCart,
            whenFailedCart: failedCart => failedCart,
            whenUnvalidatedCart: unvalidatedCart => unvalidatedCart,
            whenValidatedCart: validatedCart => validatedCart,
            whenPaidCart: paidCart => paidCart,
            whenCalculatedCart: calculatedCart =>
            {
                StringBuilder csv = new();
                calculatedCart.ProductList.Aggregate(csv, (export, product) => export.AppendLine($"{product.Code.Value}, {product.Quantity}, {product.DestinationAddress},  {product.TotalPrice}"));

                PaidCart paidCart = new(calculatedCart.ProductList, csv.ToString(), DateTime.Now);

                return paidCart;
            });

        private static ICart GenerateExport(CalculatedCart calculatedCart) =>
        new PaidCart(calculatedCart.ProductList,
                                   calculatedCart.ProductList.Aggregate(new StringBuilder(), CreateCsvLine).ToString(),
                                   DateTime.Now);

        private static StringBuilder CreateCsvLine(StringBuilder export, CalculatedProduct product) =>
            export.AppendLine($"{product.Code.Value}, {product.Quantity}, {product.Price}, {product.DestinationAddress}, {product.TotalPrice}");
    }
}
