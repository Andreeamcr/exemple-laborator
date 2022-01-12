using LanguageExt;
using ShoppingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ShoppingApp.Domain.Models.Cart;
using static ShoppingApp.Domain.Models.PaidCartEvent;
using static ShoppingApp.Domain.CartOperations;
using static LanguageExt.Prelude;
using ShoppingApp.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace ShoppingApp.Domain
{
    public class CartWorkflow
    {

        private readonly IOrderHeadersRepository orderHeadersRepository;
        private readonly IOrderLinesRepository orderLinesRepository;
        private readonly IProductsRepository productsRepository;
        private readonly ILogger<CartWorkflow> logger;


        public CartWorkflow(IProductsRepository productsRepository, IOrderLinesRepository orderLinesRepository, IOrderHeadersRepository orderHeadersRepository, ILogger<CartWorkflow> logger)
        {
            this.orderHeadersRepository = orderHeadersRepository;
            this.orderLinesRepository = orderLinesRepository;
            this.productsRepository = productsRepository;
            this.logger = logger;
        }

        public async Task<IPaidCartEvent> ExecuteAsync(CartCommand command)
        {
            UnvalidatedCart unvalidatedProducts = new UnvalidatedCart(command.InputProducts);

            var result = from products in productsRepository.TryGetExistingProduct(unvalidatedProducts.ProductList.Select(product => product.ProductCode))
                         .ToEither(ex => new FailedCart(unvalidatedProducts.ProductList, ex) as ICart)
                         from existingOrderLines in orderLinesRepository.TryGetExistingOrderLines()
                          .ToEither(ex => new FailedCart(unvalidatedProducts.ProductList, ex) as ICart)
                         let checkProducts = (Func<ProductCode, Option<ProductCode>>)(product => CheckProductExists(products, product))
                         from paidCarts in ExecuteWorkflowAsync(unvalidatedProducts, existingOrderLines, checkProducts).ToAsync()
                         from _ in orderLinesRepository.TrySaveOrderLines(paidCarts)
                         .ToEither(ex => new FailedCart(unvalidatedProducts.ProductList, ex) as ICart)
                         select paidCarts;


            return await result.Match(
                   Left: products => GenerateFailedEvent(products) as IPaidCartEvent,
                   Right: paidProducts => new SuccessPaidCartEvent(paidProducts.Csv, paidProducts.PublishedDate)
               );
        }

        private async Task<Either<ICart, PaidCart>> ExecuteWorkflowAsync(UnvalidatedCart unvalidatedProducts,
                                                                              IEnumerable<CalculatedProduct> existingProducts,
                                                                              Func<ProductCode, Option<ProductCode>> checkProducts)
        {
            ICart products = await ValidateProducts(checkProducts, unvalidatedProducts);
            products = CalculateCart(products);
            products = MergeProducts(products, existingProducts);
            products = PayCart(products);

            return products.Match<Either<ICart, PaidCart>>(
                whenEmptyCart: emptyCart => Left(emptyCart as ICart),
                whenUnvalidatedCart: unvalidatedCart => Left(unvalidatedCart as ICart),
                whenCalculatedCart: calculatedCart => Left(calculatedCart as ICart),
                whenValidatedCart: validatedCart => Left(validatedCart as ICart),
                whenFailedCart: failedCart => Left(failedCart as ICart),
                whenPaidCart: paidCart => Right(paidCart)
                );
        }

        private Option<ProductCode> CheckProductExists(IEnumerable<ProductCode> products, ProductCode productCode)
        {
            if (products.Any(p => p == productCode) && products.Any())
            {
                return Some(productCode);
            }
            else
            {
                return None;
            }
        }

        private FailedPaidCartEvent GenerateFailedEvent(ICart cart) =>
            cart.Match<FailedPaidCartEvent>(
                whenEmptyCart: emptyCart => new($"Invalid state {nameof(EmptyCart)}"),
                whenUnvalidatedCart: unvalidatedCart => new($"Invalid state {nameof(UnvalidatedCart)}"),
                whenValidatedCart: validatedCart => new($"Invalid state {nameof(ValidatedCart)}"),
                whenFailedCart: failedCart =>
                {
                    logger.LogError(failedCart.Exception, failedCart.Exception.Message);
                    return new(failedCart.Exception.Message);
                },
                whenCalculatedCart: calculatedCart => new($"Invalid state {nameof(CalculatedCart)}"),
                whenPaidCart: paidCart => new($"Invalid state {nameof(PaidCart)}"));
    }
}
