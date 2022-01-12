using LanguageExt;
using ShoppingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Domain.Repositories
{
    public interface IProductsRepository
    {
        TryAsync<List<ProductCode>> TryGetExistingProduct(IEnumerable<string> productsToCheck);
    }
}
