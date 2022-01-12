using LanguageExt;
using static LanguageExt.Prelude;
using Microsoft.EntityFrameworkCore;
using ShoppingApp.Data.Models;
using ShoppingApp.Domain.Models;
using static ShoppingApp.Domain.Models.Cart;
using ShoppingApp.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingApp.Data.Repositories
{
    public class OrderLinesRepository : IOrderLinesRepository
    {
        private readonly ProductsContext dbContext;

        public OrderLinesRepository(ProductsContext dbContext)
        {
            this.dbContext = dbContext;
        }

        TryAsync<List<CalculatedProduct>> IOrderLinesRepository.TryGetExistingOrderLines() => async () =>
        {
            var dbList = await (from ol in dbContext.OrderLines
                                join p in dbContext.Products on ol.OrderId equals p.ProductId
                                join oh in dbContext.OrderHeaders on ol.OrderId equals oh.OrderId
                                select new { oh.OrderId, p.Code, ol.Quantity, oh.Address, ol.Price, oh.Total })
                                .AsNoTracking()
                                .ToListAsync();

            var list = dbList
                          .Select(result => new CalculatedProduct(
                                                    Code: new(result.Code),
                                                    Price: new(result.Price),
                                                    Quantity: new(result.Quantity),
                                                    DestinationAddress: new(result.Address),
                                                    TotalPrice: new(result.Total))
                          {
                              OrderId = result.OrderId
                          })
                          .ToList();

            return list;
        };

        public TryAsync<Unit> TrySaveOrderLines(PaidCart pairdCart) => async () =>
        {
            var products = (await dbContext.Products.ToListAsync()).ToLookup(product => product.Code);
            var orders = (await dbContext.OrderHeaders.ToListAsync()).ToLookup(order => order.OrderId);
            var newCart = pairdCart.ProductList
                                    .Where(product => product.IsUpdated && product.OrderId == 0)
                                    .Select(product => new OrderLineDto()
                                    {
                                        ProductId = products[product.Code.Value].Single().ProductId.ToString(),
                                        OrderId = orders[product.OrderId].Single().OrderId,
                                        Quantity = product.Quantity.Value,
                                        Price = product.Price.Value,
                                    });
            var updatedCart = pairdCart.ProductList.Where(product => product.IsUpdated && product.OrderId > 0)
                                    .Select(product => new OrderLineDto()
                                    {
                                        OrderId = product.OrderId,
                                        ProductId = products[product.Code.Value].Single().ProductId.ToString(),
                                        Quantity = product.Quantity.Value,
                                        Price = product.Price.Value
                                    });

            dbContext.AddRange(newCart);
            foreach (var entity in updatedCart)
            {
                dbContext.Entry(entity).State = EntityState.Modified;
            }

            await dbContext.SaveChangesAsync();

            return unit;
        };

       
    }
}
