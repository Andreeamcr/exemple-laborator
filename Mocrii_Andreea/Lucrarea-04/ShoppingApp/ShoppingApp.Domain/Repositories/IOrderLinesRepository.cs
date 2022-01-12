using LanguageExt;
using ShoppingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ShoppingApp.Domain.Models.Cart;

namespace ShoppingApp.Domain.Repositories
{
    public interface IOrderLinesRepository
    {
        TryAsync<List<CalculatedProduct>> TryGetExistingOrderLines();

        TryAsync<Unit> TrySaveOrderLines(PaidCart cart);
    }
}
