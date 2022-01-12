using CSharp.Choices;
using System;
using System.Collections.Generic;

namespace ShoppingApp.Domain.Models
{

    [AsChoice]
    public static partial class Cart
    {
        public interface ICart { }

        public record EmptyCart: ICart
        {
            internal EmptyCart () { }
        }

        public record UnvalidatedCart : ICart
        {
            public UnvalidatedCart(IReadOnlyCollection<UnvalidatedProduct> procuctList) 
            {
                ProductList = procuctList;
            }

            public IReadOnlyCollection<UnvalidatedProduct> ProductList { get; }
        }

        public record ValidatedCart: ICart
        {
            internal ValidatedCart(IReadOnlyCollection<ValidatedProduct> productList)
            {
                ProductList = productList; 
            }

            public IReadOnlyCollection<ValidatedProduct> ProductList { get; }
        }

        public record CalculatedCart: ICart
        {
            internal CalculatedCart(IReadOnlyCollection<CalculatedProduct> productList)
            {
                ProductList = productList;

            }
            public IReadOnlyCollection<CalculatedProduct> ProductList { get; }
        }

        public record PaidCart : ICart
        {
            internal PaidCart(IReadOnlyCollection<CalculatedProduct> productList, string csv, DateTime publishedDate)
            {
                ProductList = productList;
                PublishedDate = publishedDate;
                Csv = csv;
            }

            public IReadOnlyCollection<CalculatedProduct> ProductList { get; }
            public DateTime PublishedDate { get; }
            public string Csv { get; }
        }

        public record FailedCart: ICart
        {
            internal FailedCart(IReadOnlyCollection<UnvalidatedProduct> productList, Exception exception)
            {
                ProductList = productList;
                Exception = exception;
            }
            public IReadOnlyCollection<UnvalidatedProduct> ProductList { get; }
            public Exception Exception { get; }
        }
    }

}
