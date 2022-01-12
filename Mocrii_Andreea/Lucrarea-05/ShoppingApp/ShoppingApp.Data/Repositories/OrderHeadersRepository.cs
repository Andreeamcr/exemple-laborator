using LanguageExt;
using ShoppingApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using ShoppingApp.Domain.Repositories;
using System.Linq;
using System.Collections.Generic;

namespace ShoppingApp.Data.Repositories
{
    public class OrderHeadersRepository : IOrderHeadersRepository
    {
        private readonly ProductsContext dbContext;

        public OrderHeadersRepository(ProductsContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public TryAsync<List<int>> TryGetExistingOrderHeaders(IEnumerable<int> shoppingCartsToCheck) => async () =>
        {
            var orders = await dbContext.OrderHeaders
                                                .Where(order => shoppingCartsToCheck.Contains(order.OrderId))
                                                .AsNoTracking()
                                                .ToListAsync();
            return orders.Select(order => order.OrderId)
                            .ToList();
        };
    }
}
