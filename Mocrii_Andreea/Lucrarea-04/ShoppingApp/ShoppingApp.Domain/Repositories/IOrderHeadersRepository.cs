using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Domain.Repositories
{
    public interface IOrderHeadersRepository
    {
        TryAsync<List<int>> TryGetExistingOrderHeaders(IEnumerable<int> shoppingCartsToCheck);
    }
}
