using LanguageExt;
using ShoppingApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using ShoppingApp.Domain.Repositories;
using System.Linq;
using System.Collections.Generic;

namespace ShoppingApp.Data.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly ProductsContext dbContext;

        public ProductsRepository(ProductsContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public TryAsync<List<ProductCode>> TryGetExistingProduct(IEnumerable<string> productCode) => async () =>
        {
            var products = await dbContext.Products
                                                .Where(product => productCode.Contains(product.Code.ToString()))
                                                .AsNoTracking()
                                                .ToListAsync();
            return products.Select(product => new ProductCode(product.Code.ToString()))
                            .ToList();
        };

    }
}
